using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree;
using NetConsole.Core.Actions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Grammar
{
    public class CommandExtractor : CommandGrammarBaseVisitor<CommandAction[]>
    {

        public int LastOperationStatus { get; private set; }

        private ICache<ICommand> _cache;

        private List<object> _parameters;

        private Dictionary<string, object> _options;

        private object _currentValue;

        public CommandExtractor(ICache<ICommand> cache)
        {
            _cache = cache;
            _parameters = new List<object>();
            LastOperationStatus = 0;
        }

        # region Public Methods

        public override CommandAction[] VisitCompile(CommandGrammarParser.CompileContext context)
        {
            var outputs = new List<CommandAction>();
            foreach (var instruction in context.instruction())
                outputs.AddRange(this.Visit(instruction));

            if (outputs.Last().Status != 0)
                LastOperationStatus = outputs.Last().Status;

            return outputs.ToArray();
        }

        public override CommandAction[] VisitAndInstruction(CommandGrammarParser.AndInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus != 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override CommandAction[] VisitOrInstruction(CommandGrammarParser.OrInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus == 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override CommandAction[] VisitAtomicInstruction(CommandGrammarParser.AtomicInstructionContext context)
        {
            var output = this.Visit(context.atomic_instruction());
            return output;
        }

        public override CommandAction[] VisitCommand(CommandGrammarParser.CommandContext context)
        {
            _parameters = new List<object>();
            _options = new Dictionary<string, object>();
            this.Visit(context.command_header());
            this.Visit(context.list_params());

            return new[] {ExtractCommand(context.command_header())};
        }

        public override CommandAction[] VisitPipeCommand(CommandGrammarParser.PipeCommandContext context)
        {
            CommandAction[] initialOutput = this.Visit(context.command());

            if (initialOutput[0].Status != 0)
                return initialOutput;

            var lastOutput = initialOutput[0];

            foreach (var header in context.command_header())
            {
                this.Visit(header);
                _parameters = new List<object> { lastOutput.Perform()[0] };

                if (lastOutput.Status != 0)
                    break;

                lastOutput = ExtractCommand(header); 
            }

            return new[] {lastOutput};
        }

        public override CommandAction[] VisitRedirectCommand(CommandGrammarParser.RedirectCommandContext context)
        {
            return base.VisitRedirectCommand(context);
        }

        public override CommandAction[] VisitInputCommand(CommandGrammarParser.InputCommandContext context)
        {
            return base.VisitInputCommand(context);
        }

        public override CommandAction[] VisitStringParam(CommandGrammarParser.StringParamContext context)
        {
            this.Visit(context.text());
            _parameters.Add(_currentValue);
            return base.VisitStringParam(context);
        }

        public override CommandAction[] VisitDoubleParam(CommandGrammarParser.DoubleParamContext context)
        {
            _currentValue = double.Parse(context.DOUBLE().ToString());
            return base.VisitDoubleParam(context);
        }

        public override CommandAction[] VisitIntParam(CommandGrammarParser.IntParamContext context)
        {
            _currentValue = int.Parse(context.INT().ToString());
            return base.VisitIntParam(context);
        }

        public override CommandAction[] VisitOptionParam(CommandGrammarParser.OptionParamContext context)
        {
            string option = context.ID().ToString();
            _currentValue = null;
            if (context.EQUAL() != null)
                this.Visit(context.type_param());

            if (!_options.ContainsKey(option))
                _options[option] = _currentValue;

            return base.VisitOptionParam(context);
        }

        public override CommandAction[] VisitIDText(CommandGrammarParser.IDTextContext context)
        {
            _currentValue = context.ID().ToString();
            return base.VisitIDText(context);
        }

        public override CommandAction[] VisitStringText(CommandGrammarParser.StringTextContext context)
        {
            var regex = new Regex("\"(.*?)\"", RegexOptions.Singleline);
            var match = regex.Match(context.STRING().ToString());
            _currentValue = null;
            if (match.Captures.Count > 0)
            {
                // Get the unquoted text:
                var captureQuotedText = new Regex("[^\"]*[^\"]");
                var quoted = captureQuotedText.Match(match.Captures[0].Value);
                _currentValue = quoted.Captures[0].Value;
            }
            return base.VisitStringText(context);
        }

        # endregion

        # region Private Methods

        private CommandAction ExtractCommand(CommandGrammarParser.Command_headerContext header)
        {
            string cmdName = header.GetChild(0).GetText();

            var parameters = _parameters;

            if (!_cache.Contains(cmdName))
            {
                LastOperationStatus = 1;
                return new CommandAction("Command not present in factory.", 1);
            }

            var cmd = _cache.GetInstance(cmdName);
            string action = header.ChildCount > 2 ? header.GetChild(2).GetText() : null;

            MethodInfo actionInfo = null;

            // Adding options for command
            foreach (var kv in _options)
            {
                if (cmd.Accessor.HasOptionDefined(kv.Key))
                {
                    var def = cmd.Accessor.GetOptionDefinition(kv.Key);
                    if (!def.DeclarableOnly)
                        cmd.Accessor.AddOptionValue(kv.Key, kv.Value);
                    else if (kv.Value != null)
                    {
                        LastOperationStatus = 3;
                        return new CommandAction(string.Format("{0} option is declarable only.", kv.Key), 3);
                    }

                    if (def.OverrideExecution)
                    {
                        actionInfo = cmd.GetMethodForOption(kv.Key);

                        if (actionInfo == null)
                        {
                            LastOperationStatus = 4;
                            return
                                new CommandAction(
                                    string.Format("{0} self-executing option without corresponding method", kv.Key), 4);

                        }
                        parameters = new List<object> {action};
                        if(kv.Value != null)
                            parameters.Add(kv.Value);
                        break;
                    }
                }
                else
                {
                    LastOperationStatus = 2;
                    return new CommandAction(string.Format("{0} option is not defined for the command.", kv.Key), 2);
                }
            }

            if (actionInfo == null)
                actionInfo = cmd.FindAction(action, parameters.ToArray());

            if (actionInfo == null)
            {
                LastOperationStatus = 1;
                return new CommandAction("There is not any compatible action for this command.", 1);
            }

            //var result = cmd.Perform(infoMatch, _parameters);
            LastOperationStatus = 0;
            return new CommandAction("Ok", 0, cmd, actionInfo, parameters.ToArray());
        }

        # endregion

    }
}

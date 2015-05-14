using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using NetConsole.Core.Extensions;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class CommandExtractor : CommandGrammarBaseVisitor<CommandActionInfo[]>
    {

        public int LastOperationStatus { get; private set; }

        private ICommandFactory _factory;

        private List<ParamInfo> _parameters;

        private Dictionary<string, object> _options;

        private object _currentOptionValue; 

        public CommandExtractor(ICommandFactory factory)
        {
            _factory = factory;
            _parameters = new List<ParamInfo>();
            LastOperationStatus = 0;
        }

        # region Public Methods

        public override CommandActionInfo[] VisitCompile(CommandGrammarParser.CompileContext context)
        {
            var output = this.Visit(context.instruction());
            return output;
        }

        public override CommandActionInfo[] VisitAndInstruction(CommandGrammarParser.AndInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus != 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override CommandActionInfo[] VisitOrInstruction(CommandGrammarParser.OrInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus == 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override CommandActionInfo[] VisitAtomicInstruction(CommandGrammarParser.AtomicInstructionContext context)
        {
            var output = this.Visit(context.atomic_instruction());
            return output;
        }

        public override CommandActionInfo[] VisitCommand(CommandGrammarParser.CommandContext context)
        {
            _parameters = new List<ParamInfo>();
            _options = new Dictionary<string, object>();
            this.Visit(context.command_header());
            this.Visit(context.list_params());

            return new[] {ExtractCommand(context.command_header())};
        }

        public override CommandActionInfo[] VisitPipeCommand(CommandGrammarParser.PipeCommandContext context)
        {
            CommandActionInfo[] leftCommand = this.Visit(context.command());

            var outputs = new List<CommandActionInfo>(leftCommand);

            foreach (var header in context.command_header())
            {
                this.Visit(header);

                _parameters = new List<ParamInfo> { new ParamInfo(outputs.Last().Action.ReturnType.Name) };
                var result = ExtractCommand(header);
                outputs.Add(result);
                if (result.Status != 0)
                    break;
            }

            return outputs.ToArray();
        }

        public override CommandActionInfo[] VisitRedirectCommand(CommandGrammarParser.RedirectCommandContext context)
        {
            return base.VisitRedirectCommand(context);
        }

        public override CommandActionInfo[] VisitInputCommand(CommandGrammarParser.InputCommandContext context)
        {
            return base.VisitInputCommand(context);
        }

        public override CommandActionInfo[] VisitStringParam(CommandGrammarParser.StringParamContext context)
        {
            _parameters.Add(new ParamInfo(context.st.Text));
            return base.VisitStringParam(context);
        }

        public override CommandActionInfo[] VisitIntParam(CommandGrammarParser.IntParamContext context)
        {
            _parameters.Add(new ParamInfo(int.Parse(context.INT().ToString())));
            return base.VisitIntParam(context);
        }

        public override CommandActionInfo[] VisitDoubleParam(CommandGrammarParser.DoubleParamContext context)
        {
            _parameters.Add(new ParamInfo(Double.Parse(context.DOUBLE().ToString())));
            return base.VisitDoubleParam(context);
        }

        public override CommandActionInfo[] VisitBoolParam(CommandGrammarParser.BoolParamContext context)
        {
            _parameters.Add(new ParamInfo(bool.Parse(context.BOOL().ToString())));
            return base.VisitBoolParam(context);
        }

        public override CommandActionInfo[] VisitOptionParam(CommandGrammarParser.OptionParamContext context)
        {
            string option = context.ID().ToString();
            _currentOptionValue = null;
            if(context.EQUAL() != null)
                this.Visit(context.option_value());

            if (!_options.ContainsKey(option))
                _options[option] = _currentOptionValue;

            return base.VisitOptionParam(context);
        }

        public override CommandActionInfo[] VisitBoolOption(CommandGrammarParser.BoolOptionContext context)
        {
            _currentOptionValue = bool.Parse(context.BOOL().ToString());
            return base.VisitBoolOption(context);
        }

        public override CommandActionInfo[] VisitDoubleOption(CommandGrammarParser.DoubleOptionContext context)
        {
            _currentOptionValue = double.Parse(context.DOUBLE().ToString());
            return base.VisitDoubleOption(context);
        }

        public override CommandActionInfo[] VisitIntOption(CommandGrammarParser.IntOptionContext context)
        {
            _currentOptionValue = int.Parse(context.INT().ToString());
            return base.VisitIntOption(context);
        }

        public override CommandActionInfo[] VisitStringOption(CommandGrammarParser.StringOptionContext context)
        {
            _currentOptionValue = context.st.Text;
            return base.VisitStringOption(context);
        }

        # endregion

        # region Private Methods

        private CommandActionInfo ExtractCommand(CommandGrammarParser.Command_headerContext header)
        {
            string cmdName = header.GetChild(0).GetText();

            var parameters = _parameters;

            if (!_factory.Contains(cmdName))
            {
                LastOperationStatus = 1;
                return new CommandActionInfo("Command not present in factory.", 1);
            }

            var cmd = _factory.GetInstance(cmdName);
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

                    if (def.OverrideExecution)
                    {
                        actionInfo = cmd.GetActionForOption(kv.Key);
                        
                        parameters = new List<ParamInfo>();
                        break;
                    }
                }
            }

            if (actionInfo == null)
                actionInfo = cmd.FindAction(action, _parameters.Select(p => p.Type).ToArray());

            if (actionInfo == null)
            {
                LastOperationStatus = 1;
                return new CommandActionInfo("There is not any compatible action for this command.", 1);
            }

            //var result = cmd.Perform(infoMatch, _parameters);
            LastOperationStatus = 0;
            return new CommandActionInfo("Ok", 0, cmd, actionInfo, parameters);
        }

        # endregion

    }
}

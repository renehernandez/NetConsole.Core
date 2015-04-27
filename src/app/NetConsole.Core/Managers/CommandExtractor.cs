using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using NetConsole.Core.Extensions;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class CommandExtractor : CommandGrammarBaseVisitor<ReturnInfo[]>
    {

        public int LastOperationStatus { get; private set; }

        private ICommandFactory _factory;

        private List<ParamInfo> _parameters; 

        public CommandExtractor(ICommandFactory factory)
        {
            _factory = factory;
            _parameters = new List<ParamInfo>();

            LastOperationStatus = 0;

        }

        # region Public Methods

        public override ReturnInfo[] VisitCompile(CommandGrammarParser.CompileContext context)
        {
            var output = this.Visit(context.instruction());
            return output;
        }

        public override ReturnInfo[] VisitPipeCommand(CommandGrammarParser.PipeCommandContext context)
        {
            var leftCommand = this.Visit(context.command());

            var outputs = new List<ReturnInfo>(leftCommand);

            foreach (var header in context.command_header())
            {
                this.Visit(header);

                _parameters = new List<ParamInfo> { new ParamInfo(outputs.Last().Output) };
                var result = ExtractCommand(header);
                outputs.Add(result);
                if (result.Status != 0)
                    break;
            }

            return outputs.ToArray();
        }

        public override ReturnInfo[] VisitAndInstruction(CommandGrammarParser.AndInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus != 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override ReturnInfo[] VisitOrInstruction(CommandGrammarParser.OrInstructionContext context)
        {
            var leftInst = this.Visit(context.instruction(0));

            if (LastOperationStatus == 0)
            {
                return leftInst;
            }

            var rightInst = this.Visit(context.instruction(1));

            return leftInst.Concat(rightInst).ToArray();
        }

        public override ReturnInfo[] VisitAtomicInstruction(CommandGrammarParser.AtomicInstructionContext context)
        {
            var output = this.Visit(context.atomic_instruction());
            return output;
        }

        public override ReturnInfo[] VisitCommand(CommandGrammarParser.CommandContext context)
        {
            _parameters = new List<ParamInfo>();
            this.Visit(context.command_header());
            this.Visit(context.list_params());

            return new[] {ExtractCommand(context.command_header())};
        }

        public override ReturnInfo[] VisitStringParam(CommandGrammarParser.StringParamContext context)
        {
            _parameters.Add(new ParamInfo(context.st.Text));
            return base.VisitStringParam(context);
        }

        public override ReturnInfo[] VisitIntParam(CommandGrammarParser.IntParamContext context)
        {
            _parameters.Add(new ParamInfo(int.Parse(context.INT().ToString())));
            return base.VisitIntParam(context);
        }

        public override ReturnInfo[] VisitDoubleParam(CommandGrammarParser.DoubleParamContext context)
        {
            _parameters.Add(new ParamInfo(Double.Parse(context.DOUBLE().ToString())));
            return base.VisitDoubleParam(context);
        }

        public override ReturnInfo[] VisitBoolParam(CommandGrammarParser.BoolParamContext context)
        {
            _parameters.Add(new ParamInfo(bool.Parse(context.BOOL().ToString())));
            return base.VisitBoolParam(context);
        }

        # endregion

        # region Private Methods

        private ReturnInfo ExtractCommand(CommandGrammarParser.Command_headerContext header)
        {
            string cmdName = header.GetChild(0).GetText();

            if (!_factory.Contains(cmdName))
            {
                LastOperationStatus = 1;
                return new ReturnInfo("Command not present in factory.", 1);
            }

            var cmd = _factory.GetInstance(cmdName);
            string action = header.ChildCount > 2 ? header.GetChild(2).GetText() : null;

            var infoMatch = cmd.HasMatch(action, _parameters.Select(p => p.Type).ToArray());

            if (infoMatch == null)
            {
                LastOperationStatus = 1;
                return new ReturnInfo("There is not any compatible action for this command.", 1);
            }

            var result = cmd.Perform(infoMatch, _parameters);
            LastOperationStatus = cmd.Status;
            return new ReturnInfo(result, cmd.Status);
        }

        # endregion

    }
}

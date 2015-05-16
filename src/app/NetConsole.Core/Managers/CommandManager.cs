using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NetConsole.Core.Errors;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class CommandManager : IManager<ICommand>
    {
        # region Private Fields

        private CommandExtractor _extractor;

        # endregion

        # region Public Properties

        public IFactory<ICommand> Factory { get; private set; }

        # endregion

        # region Constructors

        public CommandManager(): this(new CommandFactory())
        {
        }

        public CommandManager(IFactory<ICommand> factory)
        {
            Factory = factory;
            _extractor = new CommandExtractor(Factory);
            ImportAllCommands();
        }

        # endregion

        # region Public Methods

        public ReturnInfo[] ProcessInput(string input)
        {
            if(input == null)
                throw new ArgumentNullException("input");
            return GetOutput(new CommandGrammarLexer(new AntlrInputStream(input)));           
        }

        public ReturnInfo[] ProcessFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            return GetOutput(new CommandGrammarLexer(new AntlrFileStream(filePath)));
        }

        # endregion

        # region Private Methods

        private ReturnInfo[] GetOutput(CommandGrammarLexer lexer)
        {
            var output = new List<ReturnInfo>();
            object result;
            try
            {
                lexer.RemoveErrorListeners();
                lexer.AddErrorListener(LexerErrorListener.Instance);
                var tokens = new CommonTokenStream(lexer);
                var parser = new CommandGrammarParser(tokens);
                parser.ErrorHandler = new BailErrorStrategy();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(ParserErrorListener.Instance);
                var tree = parser.compile();
                var commands = _extractor.Visit(tree);

                foreach (var commandInfo in commands)
                {
                    if (commandInfo.Status == 0)
                    {
                        result = commandInfo.Perform()[0];
                        output.Add(new ReturnInfo(result, commandInfo.Command.Status));
                    }
                    else
                    {
                        output.Add(new ReturnInfo(commandInfo.Message, commandInfo.Status));
                    }
                    
                }

                return output.ToArray();
            }
            catch (ParseCanceledException e)
            {
                return output.Concat(new []{ new ReturnInfo(e.Message, 1) }).ToArray();
            }
        }

        private void ImportAllCommands()
        {
            Factory.RegisterAll();
        }

        # endregion

    }
}

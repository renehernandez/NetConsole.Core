using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class CommandManager
    {
        # region Private Fields

        private CommandExtractor _extractor;

        # endregion

        # region Public Properties

        public ICommandFactory Factory { get; private set; }

        # endregion

        # region Constructors

        public CommandManager(): this(new CommandFactory())
        {
        }

        public CommandManager(ICommandFactory factory)
        {
            Factory = factory;
            _extractor = new CommandExtractor(Factory);
            ImportAllCommands();
        }

        # endregion

        # region Public Methods

        public ReturnInfo[] GetOutputFromString(string input)
        {
            return GetOutput(new CommandGrammarLexer(new AntlrInputStream(input)));           
        }

        public ReturnInfo[] GetOutputFromFile(string filePath)
        {
            return GetOutput(new CommandGrammarLexer(new AntlrFileStream(filePath)));
        }

        # endregion

        # region Private Methods

        private ReturnInfo[] GetOutput(CommandGrammarLexer lexer)
        {
            var tokens = new CommonTokenStream(lexer);
            var parser = new CommandGrammarParser(tokens);
            var tree = parser.compile();
            return _extractor.Visit(tree);
        }

        private void ImportAllCommands()
        {
            Factory.RegisterAll();
        }

        # endregion

    }
}

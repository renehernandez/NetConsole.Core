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
    public class CommandImporter
    {
        # region Private Fields

        private CommandExtractor _extractor;

        # endregion

        # region Constructors

        public CommandImporter()
        {
            _extractor = new CommandExtractor(new CommandFactory());
        }

        public CommandImporter(ICommandFactory factory)
        {
            _extractor = new CommandExtractor(factory);
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

        # endregion

    }
}

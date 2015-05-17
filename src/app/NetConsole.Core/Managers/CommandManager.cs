using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NetConsole.Core.Errors;
using NetConsole.Core.Caching;
using NetConsole.Core.Extensions;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class CommandManager : IManager<ICommand>
    {
        # region Private Fields

        private readonly CommandExtractor _extractor;

        # endregion

        # region Public Properties

        public IFactory<ICommand> Factory { get; private set; } 

        public ICache<ICommand> Cache { get; private set; }

        # endregion

        # region Constructors

        public CommandManager(): this(CommandCache.GetCache(), new CommandFactory())
        {
        }

        public CommandManager(ICache<ICommand> cache, IFactory<ICommand> factory)
        {
            Cache = cache;
            Factory = factory;
            ImportCommands();

            _extractor = new CommandExtractor(Cache);
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
                var actions = _extractor.Visit(tree);

                foreach (var actionInfo in actions)
                {
                    if (actionInfo.Status == 0)
                    {
                        result = actionInfo.Perform()[0];
                        output.Add(new ReturnInfo(result, actionInfo.Command.Status));
                    }
                    else
                    {
                        output.Add(new ReturnInfo(actionInfo.Message, actionInfo.Status));
                    }
                    
                }

                return output.ToArray();
            }
            catch (ParseCanceledException e)
            {
                return output.Concat(new []{ new ReturnInfo(e.Message, 1) }).ToArray();
            }
        }

        private void ImportCommands()
        {
            foreach (var cmd in Factory.GenerateAll().Where(cmd => !Cache.Contains(cmd.Name)))
            {
                Cache.Register(cmd);
            }
        }

        # endregion

    }
}

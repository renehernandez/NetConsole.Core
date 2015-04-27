using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NetConsole.Core.Commands;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class GrammarTest
    {

        private CommandExtractor _extractor;
        private CommandGrammarLexer _lexer;
        private CommonTokenStream _tokens;
        private CommandGrammarParser _parser;
        private CommandFactory _factory;

        [SetUp]
        public void Init()
        {
            _factory = new CommandFactory();
            _factory.Register(new EchoCommand());
            _factory.Register(new PromptCommand());
            _extractor = new CommandExtractor(_factory);
        }

        [Test]
        public void Test_DefaultActionEcho()
        {
            var outputs = Connect("echo \"Hello World\" ");

            Assert.AreEqual(1, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual(@"""Hello World""", outputs[0].Output);
        }

        [Test]
        public void Test_DefaultActionPrompt()
        {
            var outputs = Connect("prompt");
            
            Assert.AreEqual(1, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("$", outputs[0].Output);
        }

        [Test]
        public void Test_SetActionPrompt()
        {
            var outputs = Connect(@"prompt : set ""^"" ");

            Assert.AreEqual(1, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual(@"""^""", outputs[0].Output);
        }

        [Test]
        public void Test_EchoedActionEchoCommand()
        {
            var outputs = Connect("echo:echoed Hello my Dear");

            Assert.AreEqual(1, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("Hello my Dear", outputs[0].Output);
        }

        [Test]
        public void Test_AndOperator()
        {
            var outputs = Connect("echo Testing and operator && prompt");

            Assert.AreEqual(2, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("Testing and operator", outputs[0].Output);
            Assert.AreEqual("$", outputs[1].Output);
        }

        [Test]
        public void Test_AndOperatorFail()
        {
            var outputs = Connect("echo && prompt");

            Assert.AreEqual(1, outputs.Length);
            Assert.AreEqual(1, _extractor.LastOperationStatus);
            Assert.AreEqual("There is not any compatible action for this command.", outputs[0].Output);
        }

        [Test]
        public void Test_OrOperatorFailFirst()
        {
            var outputs = Connect("prompt : which abfia || echo \"Previous command was wrong!!\" ");

            Assert.AreEqual(2, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("There is not any compatible action for this command.", outputs[0].Output);
            Assert.AreEqual("\"Previous command was wrong!!\"", outputs[1].Output);
        }

        [Test]
        public void Test_PipeOperator()
        {
            var outputs = Connect("prompt : set amour | echo : echoed");

            Assert.AreEqual(2, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("amour", outputs[1].Output);
        }

        [Test]
        public void Test_DoublePipe()
        {
            var outputs = Connect("echo les amour | echo : echoed | prompt : set");

            Assert.AreEqual(3, outputs.Length);
            Assert.AreEqual(0, _extractor.LastOperationStatus);
            Assert.AreEqual("les amour", outputs[1].Output);
            Assert.AreEqual("les amour", outputs[2].Output);
        }

        private ReturnInfo[] Connect(string input)
        {
            _lexer = new CommandGrammarLexer(new AntlrInputStream(input));
            _tokens = new CommonTokenStream(_lexer);
            _parser = new CommandGrammarParser(_tokens);
            var tree = _parser.compile();
            return _extractor.Visit(tree);
        }

    }
}

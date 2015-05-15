using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;
using NetConsole.Core.Managers;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class CommandManagerTest
    {

        private CommandManager _manager;

        [SetUp]
        public void SetUp()
        {
            _manager = new CommandManager();
        }

        [Test]
        public void Test_CommandManagerNotImportNotRegistrable()
        {
            // Assert
            Assert.AreEqual(3, _manager.Factory.GetAll().Count());
        }

        [Test]
        public void Test_GetOutputFromStringSuccess()
        {
            // Act
            var output = _manager.GetOutputFromString("echo:echoed unique trial");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(0, output[0].Status);
            Assert.AreEqual("unique trial", output[0].Output);
        }

        [Test]
        public void Test_GetErrorOutputFromString()
        {
            // Act
            var output = _manager.GetOutputFromString("hello");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(1, output[0].Status);
            Assert.AreEqual("Command not present in factory.", output[0].Output);
        }

        [Test]
        public void Test_BailAtFirstErrorWithoutMatchingAnythingAfter()
        {
            // Act
            var output = _manager.GetOutputFromString("^^ echo blackd");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(1, output[0].Status);
        }

        [Test]
        public void Test_GetErrorOutputFromEmptyString()
        {
            // Act
            var output = _manager.GetOutputFromString("");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(1, output[0].Status);
        }

        [Test]
        public void Test_ParseNothingIfSyntacticError()
        {
            // Act
            var output = _manager.GetOutputFromString("echo hello world | prompt set ^^ echo again");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(1, output.Last().Status);
        }

        [Test]
        public void Test_ParseHelpOption()
        {
            // Act
            var output = _manager.GetOutputFromString("echo --help");

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(0, output[0].Status);
        }

        [Test]
        public void Test_GetOutputFromFile()
        {
            var output = _manager.GetOutputFromFile("../../script.txt");

            // Assert
            Assert.AreEqual(2, output.Length);
            Assert.AreEqual(0, output[1].Status);
        }

        [Test]
        public void Test_PipeListOption()
        {
            // Act
            var output = _manager.GetOutputFromString("prompt --list | echo");

            // Assert
            Assert.AreEqual(2, output.Length);
            Assert.AreEqual(0, output[1].Status);
        }

        [Test]
        public void Test_ThrowArgumentNullExceptionGetOutputFromString()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _manager.GetOutputFromString(null));
        }

        [Test]
        public void Test_ThrowArgumentNullExceptionGetOutputFromFile()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _manager.GetOutputFromFile(null));
        }
    }
}

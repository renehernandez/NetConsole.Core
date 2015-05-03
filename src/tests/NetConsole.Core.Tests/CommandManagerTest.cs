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
            Assert.AreEqual(2, _manager.Factory.GetAll().Count());
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
    }
}

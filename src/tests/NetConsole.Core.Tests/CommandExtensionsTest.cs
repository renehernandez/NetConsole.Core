using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Commands;
using NetConsole.Core.Extensions;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class CommandExtensionsTest
    {

        private ICommand command;

        [SetUp]
        public void SetUp()
        {
            command = new PromptCommand();
        }

        [Test]
        public void Test_FindActions()
        {
            // Act
            var actions = command.FindActions();

            // Assert
            Assert.AreEqual(2, actions.Length);
            Assert.AreEqual("Get", actions.OrderBy(m => m.Name).ToArray()[0].Name);
            Assert.AreEqual("Set", actions.OrderBy(m => m.Name).ToArray()[1].Name);
        }

        [Test]
        public void Test_FindActionByName()
        {
            // Act
            var action = command.FindAction("get");
            
            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("Get", action.Name);
        }

        [Test]
        public void Test_FindActionByNameAndParams()
        {
            // Act
            var action = command.FindAction("set", new[] {typeof (string).Name});

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("Set", action.Name);
        }

        [Test]
        public void Test_FindDefaultActionByParams()
        {
            // Act
            var action = command.FindAction(null, new string[0]);

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("Get", action.Name);
        }

        [Test]
        public void Test_GetActionForHelpOption()
        {
            // Act
            var action = command.GetActionForOption("help");

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("Help", action.Name);
        }

        [Test]
        public void Test_GetActionForListOption()
        {
            // Act
            var action = command.GetActionForOption("list");

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("List", action.Name);
        }

        [Test]
        public void Test_GetActionForListOptionOutput()
        {
            // Act
            var action = command.GetActionForOption("list");

            // Assert
            Assert.AreEqual("get\r\nset\r\n", action.Invoke(command, new []{Type.Missing}));
        }

        [Test]
        public void Test_Perform()
        {
            // Arrange
            var action = command.FindAction("set");

            // Act
            var output = command.Perform(action, new[] {new ParamInfo("hello"),});

            // Assert
            Assert.NotNull(output);
            Assert.AreEqual("hello", output);
        }

    }
}

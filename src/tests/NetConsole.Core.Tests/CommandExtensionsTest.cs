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

        private ICommand _command;

        [SetUp]
        public void SetUp()
        {
            _command = new PromptCommand();
        }

        [Test]
        public void Test_FindActions()
        {
            // Act
            var actions = _command.FindActions().ToArray();

            // Assert
            Assert.AreEqual(2, actions.Length);
            Assert.AreEqual("Get", actions.OrderBy(m => m.Name).ToArray()[0].Name);
            Assert.AreEqual("Set", actions.OrderBy(m => m.Name).ToArray()[1].Name);
        }

        [Test]
        public void Test_FindActionByName()
        {
            // Act
            var action = _command.FindAction("get");
            
            // Assert
            Assert.NotNull(action);
            Assert.AreEqual(ReflectorHelper.GetActionName((PromptCommand cmd) => cmd.Get()), action.Name);
        }

        [Test]
        public void Test_FindDefaultAction()
        {
            // Act
            var action = _command.FindDefaultAction();

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual(ReflectorHelper.GetActionName((PromptCommand cmd) => cmd.Get()), action.Name);
        }

        [Test]
        public void Test_FindActionByNameAndParams()
        {
            // Act
            var action = _command.FindAction("set", new object[] {typeof (string).Name});

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual(ReflectorHelper.GetActionName((PromptCommand cmd) => cmd.Set(null)), action.Name);
        }

        [Test]
        public void Test_FindDefaultActionByParams()
        {
            // Act
            var action = _command.FindAction(null, new string[0]);

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual(ReflectorHelper.GetActionName((PromptCommand cmd) => cmd.Get()), action.Name);
        }

        [Test]
        public void Test_GetActionForHelpOption()
        {
            // Act
            var action = _command.GetMethodForOption("help");

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("Help", action.Name);
        }

        [Test]
        public void Test_GetActionForListOption()
        {
            // Act
            var action = _command.GetMethodForOption("list");

            // Assert
            Assert.NotNull(action);
            Assert.AreEqual("List", action.Name);
        }

        [Test]
        public void Test_GetActionForListOptionOutput()
        {
            // Act
            var action = _command.GetMethodForOption("list");

            // Assert
            Assert.AreEqual("get\r\nset\r\n", action.Invoke(_command, new []{Type.Missing}));
        }

        [Test]
        public void Test_Perform()
        {
            // Arrange
            var action = _command.FindAction("set");

            // Act
            var output = _command.Perform(action, new object[] {"hello"});

            // Assert
            Assert.NotNull(output);
            Assert.AreEqual("hello", output);
        }

    }
}

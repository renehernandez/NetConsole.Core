using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetConsole.Core.Commands;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class OptionAccessorTest
    {

        private OptionAccessor accessor;

        [SetUp]
        public void SetUp()
        {
            var fakeCommand = MockRepository.GenerateMock<ICommand>();

            accessor = new OptionAccessor(fakeCommand);

            accessor.ExtendOptions(new Option
            {
                Name = "list",
                DeclarableOnly = true,
                OverrideExecution = true,
                Permanent = true
            }, new Option
            {
                Name = "test"
            });
            accessor.AddOptionValue("list", true);
        }

        [Test]
        public void Test_AddOptionValueThrowsUndefinedCommandOptionException()
        {
            // Assert
            Assert.Throws<UndefinedCommandOption>(() => accessor.AddOptionValue("help"));
        }

        [Test]
        public void Test_AddOptionValueThrowsDuplicatedCommandOptionValueException()
        {
            // Assert
            Assert.Throws<DuplicatedCommandOptionValueException>(() => accessor.AddOptionValue("list"));
        }

        [Test]
        public void Test_GetOptionThrowsUndefinedCommandOption()
        {
            // Assert
            Assert.Throws<UndefinedCommandOption>(() => accessor.GetOption("hello"));
        }

        [Test]
        public void Test_GetOptionThrowsOptionValueNotSet()
        {
            // Assert
            Assert.Throws<OptionValueNotSetException>(() => accessor.GetOption("test"));
        }

        [Test]
        public void Test_GetOptionDefinitionThrowsUndefinedCommandOption()
        {
            // Assert
            Assert.Throws<UndefinedCommandOption>(() => accessor.GetOptionDefinition("help"));
        }

        [Test]
        public void Test_HasOptionValueThrowsUndefinedCommandOption()
        {
            // Assert
            Assert.Throws<UndefinedCommandOption>(() => accessor.HasOptionValue("yummy"));
        }

    }
}

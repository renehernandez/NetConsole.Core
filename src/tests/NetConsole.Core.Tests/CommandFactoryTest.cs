using System.Linq;
using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Factories;
using NetConsole.Core.Interfaces;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class CommandFactoryTest
    {

        private ICommandFactory _factory;
        private ICommand _cmd;

        [SetUp]
        public void SetUp()
        {
            _factory = new CommandFactory();
            _cmd = new EchoCommand();
            _factory.Register(_cmd);
        }

        [Test]
        public void Test_Contains()
        {
            // Assert
            Assert.AreEqual(false, _factory.Contains("hello"));
            Assert.AreEqual(true, _factory.Contains(_cmd.Name));
        }

        [Test]
        public void Test_Register()
        {
            // Assert
            Assert.AreEqual(_cmd, _factory.GetInstance(_cmd.Name));
            Assert.AreEqual(1, _factory.GetAll().Count());
        }

        [Test]
        public void Test_RegisterNotRegistrable()
        {
            // Arrange
            _factory = new CommandFactory();
            var cmdNotRegistrable = new NotRegistrableCommand();

            // Act
            _factory.Register(cmdNotRegistrable);

            // Assert
            Assert.AreEqual(1, _factory.GetAll().Count());
            Assert.AreEqual(cmdNotRegistrable, _factory.GetInstance(cmdNotRegistrable.Name));
        }

        [Test]
        public void Test_RegisterAll()
        {
            // Arrange
            _factory = new CommandFactory();

            // Act
            _factory.RegisterAll();

            // Assert
            Assert.AreEqual(2, _factory.GetAll().Count());
        }

        [Test]
        public void Test_RegisterAllWithNotRegistrable()
        {
            // Arrange
            _factory = new CommandFactory();

            // Act
            _factory.RegisterAll(true);

            // Assert
            Assert.AreEqual(3, _factory.GetAll().Count());
        }

        [Test]
        public void Test_RegisterThrowsDuplicatedException()
        {
            // Assert
            Assert.Throws<DuplicatedCommandNameException>(() => _factory.Register(_cmd));
        }

        [Test]
        public void Test_RegisterThrowsNullCommandInstanceException()
        {
            // Arrange
            ICommand cmd = null;

            // Assert
            Assert.Throws<NullCommandInstanceException>(() => _factory.Register(cmd));
        }

        [Test]
        public void Test_GetInstanceThrowsException()
        {
            // Assert
            Assert.Throws<UnregisteredCommandException>(() => _factory.GetInstance("close"));
        }

        [Test]
        public void Test_UnregisterThrowsException()
        {
            // Assert
            Assert.Throws<FailedUnregisterOperationException>(() => _factory.Unregister("close"));
        }

        [Test]
        public void Test_UnregisterNotThrowsException()
        {
            // Assert
            Assert.DoesNotThrow(() => _factory.Unregister(_cmd.Name));
        }

        [Test]
        public void Test_UnregisterRemoveCommandFromCache()
        {
            // Act
            _factory.Unregister(_cmd.Name);

            // Assert
            Assert.AreEqual(0, _factory.GetAll().Count());
        }

        [Test]
        public void Test_UnregisterReturnsCommand()
        {
            // Assert
            Assert.AreEqual(_cmd, _factory.Unregister(_cmd.Name));
        }

    }
}
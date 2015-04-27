using System.Linq;
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
        public void Init()
        {
            _factory = new CommandFactory();
            _cmd = new EchoCommand();
            _factory.Register(_cmd);
        }

        [Test]
        public void Test_Contains()
        {
            Assert.AreEqual(false, _factory.Contains("hello"));
            Assert.AreEqual(true, _factory.Contains(_cmd.Name));
        }

        [Test]
        public void Test_Register()
        {
            Assert.AreEqual(_cmd, _factory.GetInstance(_cmd.Name));
            Assert.AreEqual(1, _factory.GetAll().Count());
        }

        [Test]
        public void Test_RegisterThrowsDuplicatedException()
        {
            Assert.Throws<DuplicatedCommandNameException>(() => _factory.Register(_cmd));
        }

        [Test]
        public void Test_GetInstanceThrowsException()
        {
            Assert.Throws<UnregisteredCommandException>(() => _factory.GetInstance("close"));
        }

        [Test]
        public void Test_UnregisterThrowsException()
        {
            Assert.Throws<FailedUnregisterOperationException>(() => _factory.Unregister("close"));
        }

        [Test]
        public void Test_UnregisterNotThrowsException()
        {
            Assert.DoesNotThrow(() => _factory.Unregister(_cmd.Name));
        }

        [Test]
        public void Test_UnregisterRemoveCommandFromCache()
        {
            Assert.AreEqual(1, _factory.GetAll().Count());
            _factory.Unregister(_cmd.Name);
            Assert.AreEqual(0, _factory.GetAll().Count());
        }

        [Test]
        public void Test_UnregisterReturnsCommand()
        {
            Assert.AreEqual(_cmd, _factory.Unregister(_cmd.Name));
        }

    }
}
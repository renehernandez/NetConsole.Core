using NetConsole.Core.Commands;
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
            _cmd = new CloseCommand();
        }

        [Test]
        public void Test_Contains()
        {
            _factory.Register(_cmd);

            Assert.AreEqual(false, _factory.Contains("hello"));
            Assert.AreEqual(true, _factory.Contains(_cmd.Name));
        }

        [Test]
        public void Test_Register()
        {
            _factory.Register(_cmd);

            Assert.AreEqual(_cmd, _factory.GetInstance(_cmd.Name));
        }

        [Test]
        public void Test_GetInstance()
        {
            _factory.Register(_cmd);

        }

    }
}
using System.Linq;
using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Caching;
using NetConsole.Core.Factories;
using NetConsole.Core.Interfaces;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class CommandCacheTest
    {

        private ICache<ICommand> _cache;
        private ICommand _cmd;

        [SetUp]
        public void SetUp()
        {
            _cache = CommandCache.GetEmptyCache();
            _cmd = new EchoCommand();
            _cache.Register(_cmd);
        }

        [Test]
        public void Test_Contains()
        {
            // Assert
            Assert.AreEqual(false, _cache.Contains("hello"));
            Assert.AreEqual(true, _cache.Contains(_cmd.Name));
        }

        [Test]
        public void Test_Register()
        {
            // Assert
            Assert.AreEqual(_cmd, _cache.GetInstance(_cmd.Name));
            Assert.AreEqual(1, _cache.GetAll().Count());
        }

        [Test]
        public void Test_RegisterNotRegistrable()
        {
            // Arrange
            _cache = CommandCache.GetEmptyCache();
            var cmdNotRegistrable = new NotRegistrableCommand();

            // Act
            _cache.Register(cmdNotRegistrable);

            // Assert
            Assert.AreEqual(1, _cache.GetAll().Count());
            Assert.AreEqual(cmdNotRegistrable, _cache.GetInstance(cmdNotRegistrable.Name));
        }

        [Test]
        public void Test_RegisterAll()
        {
            // Arrange
            _cache = CommandCache.GetEmptyCache();

            // Act
            _cache.RegisterAll(new CommandFactory());

            // Assert
            Assert.AreEqual(2, _cache.GetAll().Count());
        }

        [Test]
        public void Test_RegisterThrowsDuplicatedException()
        {
            // Assert
            Assert.Throws<DuplicatedNameException>(() => _cache.Register(_cmd));
        }

        [Test]
        public void Test_RegisterThrowsNullCommandInstanceException()
        {
            // Arrange
            ICommand cmd = null;

            // Assert
            Assert.Throws<NullInstanceException>(() => _cache.Register(cmd));
        }

        [Test]
        public void Test_GetInstanceThrowsException()
        {
            // Assert
            Assert.Throws<UnregisteredInstanceException>(() => _cache.GetInstance("close"));
        }

        [Test]
        public void Test_UnregisterThrowsException()
        {
            // Assert
            Assert.Throws<FailedUnregisterOperationException>(() => _cache.Unregister("close"));
        }

        [Test]
        public void Test_UnregisterNotThrowsException()
        {
            // Assert
            Assert.DoesNotThrow(() => _cache.Unregister(_cmd.Name));
        }

        [Test]
        public void Test_UnregisterRemoveCommandFromCache()
        {
            // Act
            _cache.Unregister(_cmd.Name);

            // Assert
            Assert.AreEqual(0, _cache.GetAll().Count());
        }

        [Test]
        public void Test_UnregisterReturnsCommand()
        {
            // Assert
            Assert.AreEqual(_cmd, _cache.Unregister(_cmd.Name));
        }

    }
}
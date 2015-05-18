using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Interfaces;
using NetConsole.Core.Managers;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class ScriptManagerTest
    {

        private IManager<IScript> _manager;
        
        [SetUp]
        public void SetUp()
        {
            _manager = new ScriptManager();
        }


        [Test]
        public void Test_ThrowsArgumentNullExceptionProcessInput()
        {
            Assert.Throws<ArgumentNullException>(() => _manager.ProcessInput(null));
        }

    }
}

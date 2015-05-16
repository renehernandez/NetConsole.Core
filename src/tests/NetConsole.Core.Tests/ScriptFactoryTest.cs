using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Factories;
using NetConsole.Core.Interfaces;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class ScriptFactoryTest
    {

        private IFactory<IScript> _factory;
        
        [SetUp]
        public void SetUp()
        {
            _factory = new ScriptFactory();
            _factory.RegisterAll();
        }


        [Test]
        public void Test_RegisterAll()
        {
            // Assert
            Assert.AreEqual(1, _factory.GetAll().Count());
        }

        [Test]
        public void Test_GetInstance()
        {
            // Act
            var script = _factory.GetInstance("simple");

            // Assert
            Assert.AreEqual("simple", script.Name);
        }

    }
}

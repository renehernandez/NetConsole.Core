using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Caching;
using NetConsole.Core.Factories;
using NetConsole.Core.Interfaces;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class ScriptCacheTest
    {

        private ICache<IScript> _cache;
        
        [SetUp]
        public void SetUp()
        {
            _cache = ScriptCache.GetEmptyCache();
            _cache.RegisterAll(new ScriptFactory());
        }


        [Test]
        public void Test_RegisterAll()
        {
            // Assert
            Assert.AreEqual(1, _cache.GetAll().Count());
        }

        [Test]
        public void Test_GetInstance()
        {
            // Act
            var script = _cache.GetInstance("simple");

            // Assert
            Assert.AreEqual("simple", script.Name);
        }

    }
}

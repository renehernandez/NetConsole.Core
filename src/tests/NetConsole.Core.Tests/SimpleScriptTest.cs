using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class SimpleScriptTest
    {
        
        [Test]
        public void Test_ScriptPerform()
        {
            // Arrange
            var script = new SimpleScript();

            // Act
            var output = script.Perform();

            // Assert
            Assert.AreEqual(2, output.Length);

        }

    }
}

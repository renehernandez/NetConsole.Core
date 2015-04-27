using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetConsole.Core.Managers;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class CommandImporterTest
    {

        private CommandImporter _importer;

        [SetUp]
        public void SetUp()
        {
            _importer = MockRepository.GenerateMock<CommandImporter>();
            _importer.ImportAllCommands();
        }

        [Test]
        public void Test_GetOutputFromString()
        {
            var output = _importer.GetOutputFromString("echo:echoed unique trial");

            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(0, output[0].Status);
            Assert.AreEqual("unique trial", output[0].Output);
        }
    }
}

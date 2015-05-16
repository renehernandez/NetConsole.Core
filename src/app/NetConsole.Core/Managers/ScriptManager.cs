using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class ScriptManager : IManager<IScript>
    {

        # region Public Properties

        public IFactory<IScript> Factory { get; private set; }

        # endregion

        # region Constructors

        public ScriptManager(IFactory<IScript> factory = null)
        {
            Factory = factory ?? new ScriptFactory();
            ImportScripts();
        }

        # endregion

        # region Public Methods

        public ReturnInfo[] ProcessInput(string input)
        {
            if(input == null)
                throw new ArgumentNullException("input");

            var output = new List<ReturnInfo>();
            try
            {
                var results = Factory.GetInstance(input).Perform();
                return output.Concat(results.Select(o => new ReturnInfo(o, 0))).ToArray();
            }
            catch (CoreException e)
            {
                return output.Concat(new[] {new ReturnInfo(e.Message, 1)}).ToArray();
            }
        }

        # endregion

        # region Private Methods

        private void ImportScripts()
        {
            Factory.RegisterAll();
        }

        # endregion

    }
}

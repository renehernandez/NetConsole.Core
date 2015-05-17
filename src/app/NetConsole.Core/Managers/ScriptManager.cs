using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Caching;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Managers
{
    public class ScriptManager : IManager<IScript>
    {

        # region Public Properties

        public IFactory<IScript> Factory { get; private set; }

        public ICache<IScript> Cache { get; private set; }

        # endregion

        # region Constructors

        public ScriptManager() : this(ScriptCache.GetCache(), new ScriptFactory())
        {
            
        }

        public ScriptManager(ICache<IScript> cache, IFactory<IScript> factory)
        {
            Cache = cache;
            Factory = factory;
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
                var results = Cache.GetInstance(input).Perform();
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
            //Cache.RegisterAll();
        }

        # endregion

    }
}

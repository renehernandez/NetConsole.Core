using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Caching
{
    public class ScriptCache : BaseCache<ScriptCache, IScript>
    {

        # region Private Fields

        protected static MemoryCache Cache;

        # endregion

        # region Constructors

        static ScriptCache()
        {
            Cache = new MemoryCache("Scripts");
        }

        public ScriptCache() : base(Cache)
        {

        }

        # endregion

    }
}

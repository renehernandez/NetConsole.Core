using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using NetConsole.Core.Attributes;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Caching
{
    public class CommandCache : BaseCache<CommandCache, ICommand>
    {

        # region Private Fields

        protected static MemoryCache Cache;

        # endregion

        # region Constructors

        static CommandCache()
        {
            Cache = new MemoryCache("Commands");
        }


        public CommandCache():base(Cache)
        {
        }

        # endregion

    }
}
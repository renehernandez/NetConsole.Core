using System.Collections;
using System.Collections.Generic;
using NetConsole.Core.Commands;

namespace NetConsole.Core.Interfaces
{
    public interface IOptionAccessor
    {

        void AddOptionValue(string name, object value=null);

        object GetOption(string name);

        Option GetOptionDefinition(string name);

        bool HasOptionValue(string name);

        void ResetAccesorData();

        bool HasOptionDefined(string name);

        void ExtendOptions(params Option[] moreOptions);

        void ExtendOptions(IEnumerable<Option> moreOptions);

    }
}
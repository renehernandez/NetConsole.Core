using System;
using System.Collections.Generic;
using System.Linq;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{
    public class OptionAccessor : IOptionAccessor
    {

        # region Private Fields

        private readonly Dictionary<string, object> _store;

        private readonly Dictionary<string, Option> _options;

        private ICommand _commandOwner;

        # endregion

        # region Constructors

        public OptionAccessor(ICommand cmd, IEnumerable<Option> options)
        {
            _options = new Dictionary<string, Option>(options.ToDictionary(o => o.Name));
            _store = new Dictionary<string, object>();
            _commandOwner = cmd;
        }

        public OptionAccessor(ICommand cmd, params Option[] options):this(cmd, options.AsEnumerable())
        {          
        }

        # endregion

        # region Public Methods

        public void AddOptionValue(string name, object value = null)
        {
            if (!HasOptionDefined(name))
                throw new UndefinedCommandOption(name);
            if (HasOptionValue(name))
                throw new DuplicatedCommandOptionValueException(name);

            //if (_options[name].DeclarableOnly && value != null)
            //{
            //    throw new DeclarableOnlyOptionException(name);
            //}

            _store[name] = value ?? _commandOwner.GetActionForOption(name);
        }

        public object GetOption(string name)
        {
            if(!HasOptionValue(name))
                throw new OptionValueNotSetException(name);

            return _store[name];
        }

        public Option GetOptionDefinition(string name)
        {
            if (!HasOptionDefined(name))
                throw new UndefinedCommandOption(name);

            return _options[name];
        }

        public bool HasOptionValue(string name)
        {
            if (!HasOptionDefined(name))
                throw new UndefinedCommandOption(name);

            return _store.ContainsKey(name);
        }

        public void ResetAccesorData()
        {
            var options = _options.Values.Where(o => !o.Permanent).Select(o => o.Name).ToList();

            foreach (var option in options)
            {
                _store.Remove(option);
            }
        }

        public bool HasOptionDefined(string name)
        {
            return _options.ContainsKey(name);
        }

        public void ExtendOptions(params Option[] moreOptions)
        {
            ExtendOptions(moreOptions.AsEnumerable());
        }

        public void ExtendOptions(IEnumerable<Option> moreOptions)
        {
            foreach(var option in moreOptions.Where(o => !HasOptionDefined(o.Name)))
            {
                _options[option.Name] = option;
            }
        }

        # endregion
    }
}
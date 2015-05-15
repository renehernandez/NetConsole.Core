using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Grammar
{
    public class ParamInfo : IParameterInfo
    {

        public string Type { get; private set; }
        
        public object Value { get; private set; }

        public ParamInfo(object value, Type type = null)
        {
            Value = value;
            Type = type != null ? type.Name : value.GetType().Name;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}

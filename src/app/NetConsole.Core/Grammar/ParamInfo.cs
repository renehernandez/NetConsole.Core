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

        public ParamInfo(object value)
        {
            Value = value;
            Type = value.GetType().Name;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}

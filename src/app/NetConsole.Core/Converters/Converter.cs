using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Converters
{
    public static class Converter
    {

        # region Public Methods

        public static Tuple<bool, object> TryString(object arg)
        {
            return  arg == null || arg is string ? new Tuple<bool, object>(true, arg) : new Tuple<bool, object>(false, null);
        }

        public static Tuple<bool, object> TryByte(object arg)
        {
            return MakeConvertion(typeof (byte), arg);
        }

        public static Tuple<bool, object> TrySbyte(object arg)
        {
            return MakeConvertion(typeof (sbyte), arg);
        }

        public static Tuple<bool, object> TryShort(object arg)
        {
            return MakeConvertion(typeof (short), arg);
        }

        public static Tuple<bool, object> TryUshort(object arg)
        {
            return MakeConvertion(typeof (ushort), arg);
        }

        public static Tuple<bool, object> TryInt(object arg)
        {
            return MakeConvertion(typeof (int), arg);
        }

        public static Tuple<bool, object> TryUint(object arg)
        {
            return MakeConvertion(typeof (uint), arg);
        }

        public static Tuple<bool, object> TryLong(object arg)
        {
            return MakeConvertion(typeof (long), arg);
        }

        public static Tuple<bool, object> TryUlong(object arg)
        {
            return MakeConvertion(typeof (ulong), arg);
        }

        public static Tuple<bool, object> TryChar(object arg)
        {
            return MakeConvertion(typeof (char), arg);
        }

        public static Tuple<bool, object> TryBool(object arg)
        {
            return MakeConvertion(typeof (bool), arg);
        }

        public static Tuple<bool, object> TryFloat(object arg)
        {
            return MakeConvertion(typeof (float), arg);
        }

        public static Tuple<bool, object> TryDouble(object arg)
        {
            return MakeConvertion(typeof (double), arg);
        }

        public static Tuple<bool, object> TryDecimal(object arg)
        {
            return MakeConvertion(typeof (decimal), arg);
        }

        public static Tuple<bool, object> TryDateTime(object arg)
        {
            return MakeConvertion(typeof (DateTime), arg);
        }

        public static Tuple<bool, object> TryObject(Type type, object arg)
        {
            return arg == null || type.IsInstanceOfType(arg) 
                ? new Tuple<bool, object>(true, arg)
                : new Tuple<bool, object>(false, null);
        }

        # endregion

        # region Private Methods

        private static readonly Func<Type, object, Tuple<bool, object>> MakeConvertion = (type, o) =>
        {
            if (type.IsInstanceOfType(o))
                return new Tuple<bool, object>(true, o);

            var parse = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });
            object[] args = { o.ToString(), Activator.CreateInstance(type) };

            if (o is string && (bool)parse.Invoke(null, args))
                return new Tuple<bool, object>(true, args[1]);

            return new Tuple<bool, object>(false, null);
        };

        # endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Converters;

namespace NetConsole.Core.Extensions
{
    public static class MethodInfoExtensions
    {
        # region Private Fields

        private static readonly Func<Type, object, Tuple<bool, object>>[] ConverterFuncs;

        # endregion

        # region Constructor

        static MethodInfoExtensions()
        {
            var nullOutput = new Tuple<bool, object>(false, null);

            ConverterFuncs = new Func<Type, object, Tuple<bool, object>>[]
            { 
                (t, o) => Type.GetTypeCode(t) == TypeCode.Byte ? Converter.TryByte(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.SByte ? Converter.TrySbyte(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Int16 ? Converter.TryShort(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.UInt16 ? Converter.TryUshort(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Int32 ? Converter.TryInt(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.UInt32 ? Converter.TryUint(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Int64 ? Converter.TryLong(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.UInt64 ? Converter.TryUlong(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Single ? Converter.TryFloat(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Double ? Converter.TryDouble(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Decimal ? Converter.TryDecimal(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Char ? Converter.TryChar(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.String ? Converter.TryString(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Boolean ? Converter.TryBool(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.DateTime ? Converter.TryDateTime(o) : nullOutput,
                (t, o) => Type.GetTypeCode(t) == TypeCode.Object ? Converter.TryObject(t, o) : nullOutput
            };
        }

        # endregion

        # region Public Extensions

        public static Tuple<bool, object[]> MatchMethodParameters(this MethodInfo info, object[] arguments)
        {
            var parameters = info.GetParameters();
            var nullOutput = new Tuple<bool, object[]>(false, null);

            if (parameters.Length == 0)
            {
                return arguments.Length == 0 ? new Tuple<bool, object[]>(true, arguments) : nullOutput;
            }

            var requiredParams = parameters.Where(p => !p.IsOptional);
            var optionalParams = parameters.Where(p => p.IsOptional);
            int requiredCount = requiredParams.Count();
            int optionalCount = optionalParams.Count();
            int providedCount = arguments.Count();

            bool hasParamArray = parameters.Last().GetCustomAttributes(false).OfType<ParamArrayAttribute>().Any();

            if (requiredCount > providedCount)
                return nullOutput;

            var matchedArgs = parameters.Select(param => param.DefaultValue).ToList();

            int length = Math.Min(parameters.Length, arguments.Length);
            Tuple<bool, object> coercedArg;

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1 && hasParamArray)
                {
                    var arrayParamType = parameters[i].ParameterType.GetElementType();
                    var arrayValues = Array.CreateInstance(arrayParamType, arguments.Length - i);
                    for (int j = i, arrayPos = 0; j < arguments.Length; j++, arrayPos++)
                    {
                        coercedArg = CoerceArgument(arrayParamType, arguments[j]);
                        if (!coercedArg.Item1)
                            return nullOutput;
                        
                        arrayValues.SetValue(coercedArg.Item2, arrayPos);
                    }

                    matchedArgs[i] = arrayValues;
                    break;
                }

                var actionParam = parameters[i];
                var requiredType = actionParam.ParameterType;
                coercedArg = CoerceArgument(requiredType, arguments[i]);

                if (!coercedArg.Item1)
                    return nullOutput;

                matchedArgs[i] = coercedArg.Item2;
            }

            return new Tuple<bool, object[]>(true, matchedArgs.ToArray());
        }

        public static string ActionName(this MethodInfo info)
        {
            return info.Name.ToLower();
        }

        # endregion

        # region Private Methods

        private static Tuple<bool, object> CoerceArgument(Type requiredType, object arg)
        {
            return (ConverterFuncs.Select(converter => converter(requiredType, arg))
                .Where(tup => tup.Item1)).FirstOrDefault();
        }

        # endregion
    }
}

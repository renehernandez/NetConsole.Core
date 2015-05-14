using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Extensions
{
    public static class CommandExtension
    {

        public static MethodInfo FindAction<T>(this T instance, string actionName) where T : ICommand
        {
            var type = instance.GetType();
            var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            return methodsInfo.SingleOrDefault(m => m.Name.ToLower() == actionName);   
        }

        public static MethodInfo[] FindActions<T>(this T instance) where T : ICommand
        {
            var type = instance.GetType();
            var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            return methodsInfo;
        }

        public static MethodInfo FindAction<T>(this T instance, string actionName = null, params string[] parametersTypes) where T : ICommand
        {
            var type = instance.GetType();
            var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            if (actionName != null)
            {
                var info = methodsInfo.SingleOrDefault(m => m.Name.ToLower() == actionName);
                return info != null && CheckMethodParameters(info, parametersTypes) ? info : null;
            }

            return
                methodsInfo.FirstOrDefault(
                    m =>
                        m.GetCustomAttributes(typeof (DefaultActionAttribute), true).Length != 0 &&
                        CheckMethodParameters(m, parametersTypes));
        }

        private static bool CheckMethodParameters(MethodInfo info, string[] parametersTypes)
        {
            var parameters = info.GetParameters();

            if (parameters.Length == 0)
            {
                return parametersTypes.Length == 0;
            }   

            bool hasParams = parameters[parameters.Length - 1].GetCustomAttributes(typeof (ParamArrayAttribute), false).Length > 0;
            int length = Math.Min(parameters.Length, parametersTypes.Length);
            for (int i = 0; i < length; i++)
            {
                if (i == parameters.Length - 1 && hasParams)
                {
                    string paramsType = parameters[parameters.Length - 1].ParameterType.GetElementType().Name;
                    for(int j = i; j < parametersTypes.Length; j++)
                        if (parametersTypes[j] != paramsType)
                            return false;

                    return true;
                }

                if (parameters[i].ParameterType.Name != parametersTypes[i])
                    break;

                if (i == parameters.Length - 1)
                    return true;
            }

            return false;
        }

        public static object Perform<T>(this T instance, MethodInfo info, IList<ParamInfo> paramsInfo) where T : ICommand
        {
            ParameterInfo[] parameters = info.GetParameters();
            bool hasParams = false;
            object[] inputs;
            if (parameters.Length > 0)
                hasParams = parameters[parameters.Length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;

            if (hasParams)
            {
                int lastParamPosition = parameters.Length - 1;
                inputs = new object[parameters.Length];
                for (int i = 0; i < lastParamPosition; i++)
                    inputs[i] = paramsInfo[i].Value;

                var paramsType = parameters[lastParamPosition].ParameterType.GetElementType();
                var extra = Array.CreateInstance(paramsType, paramsInfo.Count - lastParamPosition);
                for (int i = 0; i < extra.Length; i++)
                    extra.SetValue(paramsInfo[i + lastParamPosition].Value, i);

                inputs[lastParamPosition] = extra;
            }
            else
            {
                inputs = paramsInfo.Select(x => x.Value).ToArray();
            }

            return info.Invoke(instance, inputs);
        }

        public static MethodInfo GetActionForOption<T>(this T instance, string option) where T : ICommand
        {
            var type = instance.GetType();

            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | 
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Instance)
                .First(m =>
                    m.GetCustomAttributes(true).OfType<ActionForOptionAttribute>()
                        .Any(at => at.Name.Equals(option)));
        }
    }
}
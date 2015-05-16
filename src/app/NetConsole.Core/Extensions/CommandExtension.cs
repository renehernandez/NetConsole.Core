using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Antlr4.Runtime.Misc;
using NetConsole.Core.Attributes;
using NetConsole.Core.Converters;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Extensions
{
    public static class CommandExtension
    {
        # region Public Extensions

        public static MethodInfo FindAction<T>(this T instance, string actionName) where T : ICommand
        {
            return instance.FindActions().SingleOrDefault(m => m.ActionName() == actionName);   
        }

        public static IEnumerable<MethodInfo> FindActions<T>(this T instance) where T : ICommand
        {
            var type = instance.GetType();
            var actions = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            return actions.Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object));
        }

        public static MethodInfo FindDefaultAction<T>(this T instance) where T : ICommand
        {
            return instance.FindActions().FirstOrDefault(m => m.GetCustomAttributes(true).OfType<DefaultActionAttribute>().Any());
        }

        public static MethodInfo FindAction<T>(this T instance, string actionName = null, params object[] arguments) where T : ICommand
        {
            if (actionName != null)
            {
                var actions = instance.FindActions().Where(m => m.ActionName() == actionName);
                return actions.FirstOrDefault(info => info.MatchMethodParameters(arguments).Item1);
            }

            return
                instance.FindActions().FirstOrDefault(
                    m =>
                        m.GetCustomAttributes(true).OfType<DefaultActionAttribute>().Any() &&
                        m.MatchMethodParameters(arguments).Item1);
        }

        public static object Perform<T>(this T instance, MethodInfo info, object[] paramsInfo) where T : ICommand
        {
            //ParameterInfo[] parameters = info.GetParameters();
            //bool hasParams = false;
            //object[] inputs;
            //if (parameters.Length > 0)
            //    hasParams = parameters[parameters.Length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;

            //if (hasParams)
            //{
            //    int lastParamPosition = parameters.Length - 1;
            //    inputs = new object[parameters.Length];
            //    for (int i = 0; i < lastParamPosition; i++)
            //        inputs[i] = paramsInfo[i];

            //    var paramsType = parameters[lastParamPosition].ParameterType.GetElementType();
            //    var extra = Array.CreateInstance(paramsType, paramsInfo.Length - lastParamPosition);
            //    for (int i = 0; i < extra.Length; i++)
            //        extra.SetValue(paramsInfo[i + lastParamPosition], i);

            //    inputs[lastParamPosition] = extra;
            //}
            //else
            //{
            //    inputs = paramsInfo.ToArray();
            //}

            return info.Invoke(instance, paramsInfo);
        }

        public static MethodInfo GetMethodForOption<T>(this T instance, string option) where T : ICommand
        {
            var type = instance.GetType();

            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | 
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Instance)
                .FirstOrDefault(m =>
                    m.GetCustomAttributes(true).OfType<ActionForOptionAttribute>()
                        .Any(at => at.Name.Equals(option)));
        }

        # endregion

    }
}
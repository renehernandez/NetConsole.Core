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

        public static MethodInfo FindAction(this ICommand instance, string actionName)
        {
            return instance.FindActions().SingleOrDefault(m => m.ActionName() == actionName);   
        }

        public static IEnumerable<MethodInfo> FindActions(this ICommand instance)
        {
            var type = instance.GetType();
            var actions = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            return actions.Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object));
        }

        public static MethodInfo FindDefaultAction(this ICommand instance)
        {
            return instance.FindActions().FirstOrDefault(m => m.GetCustomAttributes(true).OfType<DefaultActionAttribute>().Any());
        }

        public static MethodInfo FindAction(this ICommand instance, string actionName = null, params object[] arguments)
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

        public static object Perform(this ICommand instance, MethodInfo info, object[] paramsInfo)
        {
            return info.Invoke(instance, paramsInfo);
        }

        public static MethodInfo GetMethodForOption<T>(this T instance, string option)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetConsole.Core.Attributes;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetConsole.Core.Extensions
{
    public static class CommandExtension
    {

        # region Jsonfy

        class MethodDef
        {
            public string Name { get; set; }

            public string ReturnType { get; set; }

            public Dictionary<string, string> Arguments { get; set; }

            public MethodDef()
            {
                Arguments = new Dictionary<string, string>();
            }
        }

        class PropertyDef
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string ReturnType { get; set; }
        }

        class JsonCommand
        {

            public string Name { get; set; }

            public Dictionary<string, MethodDef> Methods { get; set; }

            public Dictionary<string, PropertyDef> Properties { get; set; }

            public JsonCommand()
            {
                Methods = new Dictionary<string, MethodDef>();
                Properties = new Dictionary<string, PropertyDef>();
            }

        }

        # endregion

        public static string ToJson<T>(this T instance) where T : ICommand
        {
            var type = instance.GetType();
            var cmd = new JsonCommand
            {
                Name = instance.Name
            };

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                cmd.Properties.Add(prop.Name.ToLower(), new PropertyDef{ Name = prop.Name, Value = prop.GetValue(instance, null).ToString(), ReturnType = prop.PropertyType.Name});
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName))
            {
                cmd.Methods.Add(method.Name, new MethodDef {
                    Name = method.Name, 
                    ReturnType = method.ReturnType.Name, 
                    Arguments = method.GetParameters().ToDictionary(
                        p => p.Name,
                        p => p.ParameterType.ToString()
                    )
                    });
            }

            return JsonConvert.SerializeObject(cmd, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static MethodInfo HasMatch<T>(this T instance, string action = null, params string[] parametersTypes) where T : ICommand
        {
            var type = instance.GetType();
            var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            if (action != null)
            {
                var info = methodsInfo.SingleOrDefault(m => m.Name.ToLower() == action);
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

                Type paramsType = parameters[lastParamPosition].ParameterType.GetElementType();
                Array extra = Array.CreateInstance(paramsType, paramsInfo.Count - lastParamPosition);
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
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetConsole.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetConsole.Core.Extensions
{
    public static class CommandExtension
    {

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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetConsole.Core.Exceptions;

namespace NetConsole.Core.Extensions
{
    public static class TypeExtensions
    {

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) 
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }


        public static IEnumerable<Type> GetTypesWithInterface<T>(this IEnumerable<Type> iEnum) where T : class
        {
            var it = typeof (T);
            
            if (!it.IsInterface) 
                throw new NotInterfaceTypeException();

            return iEnum.Where(t => !t.IsInterface && it.IsAssignableFrom(t)).ToList();
        }  
    }
}
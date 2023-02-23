using System;
using System.Linq;

namespace EntityFramework
{
    public static class TypeTools
    {
        public static void IterateGenericInterfaces<TInterface>(Object toIterate, Action<Type> action)
        {
            foreach (var @interface in toIterate.GetType().GetInterfaces())
            {
                if (!@interface.IsGenericType || !@interface.GetInterfaces().Contains(typeof(TInterface))) continue;
                foreach (var generic in @interface.GenericTypeArguments)
                {
                    action.Invoke(generic);
                }
            }
        }
    }
}
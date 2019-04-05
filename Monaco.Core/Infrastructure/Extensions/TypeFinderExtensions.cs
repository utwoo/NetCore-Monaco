using System;

namespace Monaco.Core.Infrastructure.Extensions
{
    public static class TypeFinderExtensions
    {
        public static bool IsTypeImplementOpenGeneric(this Type type, Type openGeneric)
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
            foreach (var @interface in type.FindInterfaces((filter, criteria) => true, null))
            {
                if (!@interface.IsGenericType)
                    continue;

                return genericTypeDefinition.IsAssignableFrom(@interface.GetGenericTypeDefinition());
            }
            return false;
        }

    }
}
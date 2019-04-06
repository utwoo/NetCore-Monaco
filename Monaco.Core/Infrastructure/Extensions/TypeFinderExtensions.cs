using System;

namespace Monaco.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of Type Finder
    /// </summary>
    public static class TypeFinderExtensions
    {
        /// <summary>
        /// Check one type is implemented Open Generic interface
        /// </summary>
        /// <param name="type">Checked Type</param>
        /// <param name="openGeneric">Open Generic interface</param>
        /// <returns>Check Result</returns>
        public static bool IsTypeImplementOpenGeneric(this Type type, Type openGeneric)
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
            foreach (var implementInterface in type.FindInterfaces((filter, criteria) => true, null))
            {
                if (!implementInterface.IsGenericType)
                    continue;

                return genericTypeDefinition.IsAssignableFrom(implementInterface.GetGenericTypeDefinition());
            }
            return false;
        }

    }
}
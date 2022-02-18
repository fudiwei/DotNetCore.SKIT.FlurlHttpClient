using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    internal static class TypeExtensions
    {
        public static string GetNameWithoutGenerics(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.Name.Remove(type.Name.IndexOf('`'));
            }

            return type.Name;
        }
    }
}

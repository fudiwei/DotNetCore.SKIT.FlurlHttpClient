using System;
using System.Linq;

namespace SKIT.FlurlHttpClient.Converters.Internal
{
    internal static class TypeHelper
    {
        public static readonly Type[] NumberTypes = new Type[]
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(sbyte?),
            typeof(byte?),
            typeof(short?),
            typeof(ushort?),
            typeof(int?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?)
        };

        public static bool IsNumberType(Type type)
        {
            return NumberTypes.Contains(type);
        }
    }
}

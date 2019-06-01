using System.Collections.Generic;
using System.Reflection;

namespace Agony.Common.Reflection
{
    public static class CraftDataReflector
    {
        private static readonly FieldInfo techMappingFieldInfo = typeof(CraftData).GetField("techMapping", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly FieldInfo entClassTechTable = typeof(CraftData).GetField("entClassTechTable", BindingFlags.NonPublic | BindingFlags.Static);

        public static Dictionary<TechType, string> GetTechMapping()
        {
            return (Dictionary<TechType, string>)techMappingFieldInfo.GetValue(null);
        }

        public static Dictionary<string, TechType> GetEntClassTechTable()
        {
            return (Dictionary<string, TechType>)entClassTechTable.GetValue(null);
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Agony.Common.Reflection
{
    public static class AtlasReflector
    {
        private static readonly FieldInfo serialDataFieldInfo = typeof(Atlas).GetField("serialData", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo nameFieldInfo = typeof(Atlas).GetField("atlasName", BindingFlags.NonPublic | BindingFlags.Instance);

        public static string GetAtlasName(Atlas atlas)
        {
            if (atlas == null) throw new ArgumentNullException("atlas is null");
            return (string)nameFieldInfo.GetValue(atlas);
        }

        public static List<Atlas.SerialData> GetSerialData(Atlas atlas)
        {
            if (atlas == null) throw new ArgumentNullException("atlas is null");
            return (List<Atlas.SerialData>)serialDataFieldInfo.GetValue(atlas);
        }
    }  
}
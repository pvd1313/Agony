using System.Collections.Generic;
using Agony.Common.Reflection;
using Harmony;
using System;
using UWE;

namespace Agony.AssetTools.Wrappers
{
    public static class PrefabDatabaseWrapper
    {
        [HarmonyPatch(typeof(PrefabDatabase), "TryGetPrefabFilename")]
        private static class TryGetPrefabFilenamePatch
        {
            private static bool Prefix(ref bool __result, string classId, ref string filename)
            {
                if (string.IsNullOrEmpty(classId)) { return true; }
                if (prefabFiles.TryGetValue(classId, out filename))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        private static readonly Dictionary<string, string> prefabFiles = new Dictionary<string, string>();

        public static void SetPrefabFile(string classID, string fileName)
        {
            if (string.IsNullOrEmpty(classID)) throw new ArgumentException("classID is null or empty");
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("fileName is null or empty");

            prefabFiles[classID] = fileName;
        }

        public static void PreparePrefabDatabase()
        {
            var originPrefabFiles = PrefabDatabase.prefabFiles;
            if (originPrefabFiles.Count == 0) { PrefabDatabase.LoadPrefabDatabase(SNUtils.prefabDatabaseFilename); }
        }
    }
}
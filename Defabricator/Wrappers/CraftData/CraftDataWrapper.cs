using System.Collections.Generic;
using Agony.Common.Reflection;
using Agony.Common;
using HarmonyLib;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static partial class CraftDataWrapper
    {
        public static void SetTechPrefab(TechType techType, string classID)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");
            if (string.IsNullOrEmpty(classID)) throw new ArgumentException("classID is null or empty");
            CraftData.PreparePrefabIDCache();

            var techMapping = CraftDataReflector.GetTechMapping();
            var entClassTechTable = CraftDataReflector.GetEntClassTechTable();
            entClassTechTable[classID] = techType;
            techMapping[techType] = classID;
        }

        public static bool TryGetTechPrefab(TechType techType, out string classID)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");

            CraftData.PreparePrefabIDCache();
            var techMapping = CraftDataReflector.GetTechMapping();
            return techMapping.TryGetValue(techType, out classID);
        }
    }
}
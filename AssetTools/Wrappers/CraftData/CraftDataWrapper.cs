using System.Collections.Generic;
using Agony.Common.Reflection;
using Agony.Common;
using Harmony;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static partial class CraftDataWrapper
    {
        [HarmonyPatch(typeof(CraftData), "Get")]
        private static class TechDataGetPatch
        {
            private static bool Prefix(TechType techType, ref ITechData __result)
            {
                if (techData.TryGetValue(techType, out __result))
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(CraftData), "GetCraftTime")]
        private static class TechDataGetCraftTimePatch
        {
            private static bool Prefix(ref bool __result, TechType techType, ref float result)
            {
                if (craftingTimes.TryGetValue(techType, out result))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        private static readonly Dictionary<TechType, ITechData> techData = new Dictionary<TechType, ITechData>();
        private static readonly Dictionary<TechType, float> craftingTimes = new Dictionary<TechType, float>();
        
        public static void SetTechData(TechType techType, ITechData techData)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");
            CraftDataWrapper.techData[techType] = techData;
        }

        public static void SetCraftTime(TechType techType, float craftTime)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");
            if (craftTime <= 0) throw new ArgumentException("craftTime <= 0");
            craftingTimes[techType] = craftTime;
        }

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
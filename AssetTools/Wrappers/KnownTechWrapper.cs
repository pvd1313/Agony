using System.Collections.Generic;
using Harmony;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static class KnownTechWrapper
    {
        [HarmonyPatch(typeof(KnownTech), "Contains")]
        private static class KnownTechContainsPatch
        {
            private static bool Prefix(ref bool __result, TechType techType)
            {
                if (defaultTech.Contains(techType))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        private static HashSet<TechType> defaultTech = new HashSet<TechType>();

        public static void AddDefault(TechType techType)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");
            if (!defaultTech.Contains(techType)) { defaultTech.Add(techType); }
        }
    }
}
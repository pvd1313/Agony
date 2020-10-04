using System;
using HarmonyLib;
using SMLHelper.V2.Handlers;

namespace Agony.Defabricator
{
    internal static partial class Main
    {
        [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
        public static class Player_Awake_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                foreach (TechType techType in Enum.GetValues(typeof(TechType)))
                {
#if SUBNAUTICA
                    if (SMLHelper.V2.Handlers.CraftDataHandler.GetTechData(techType) != null)
#elif BELOWZERO
                    if (SMLHelper.V2.Handlers.CraftDataHandler.GetRecipeData(techType) != null)
#endif
                    {
                        RecyclingData.TryGet(techType, out TechType recyclingTech, true);
                    }
                }

#if BELOWZERO
                TechData.Cache();
#endif
                CraftData.RebuildDatabase();
                Language.main.LoadLanguageFile(Language.main.GetCurrentLanguage());
            }
        }
    }
}
using System;
using HarmonyLib;

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
                    if(SMLHelper.V2.Handlers.CraftDataHandler.GetRecipeData(techType) != null)
#endif
                    {
                        RecyclingData.TryGet(techType, out _, true);
                    }
                }

                CraftData.RebuildDatabase();
                Language.main.LoadLanguageFile(Language.main.GetCurrentLanguage());
            }
        }
    }
}
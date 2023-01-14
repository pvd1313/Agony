namespace Agony.Defabricator
{
    using System;
    using HarmonyLib;
    using SMLHelper.Handlers;

    [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
    public static class Player_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            foreach(TechType techType in Enum.GetValues(typeof(TechType)))
            {
#if SUBNAUTICA
                if(CraftDataHandler.GetTechData(techType) != null)
#elif BELOWZERO
                if (CraftDataHandler.GetRecipeData(techType) != null)
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
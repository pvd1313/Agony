using HarmonyLib;

namespace Agony.AssetTools.Wrappers
{
    partial class LanguageWrapper
    {
        [HarmonyPatch(typeof(Language), "TryGet")]
        private static class LanguageTryGetPatch
        {
            private static bool Prefix(ref bool __result, string key, ref string result)
            {
                if (majorVocabulary[CurrentLanguage].TryGetValue(key, out result))
                {
                    __result = true;
                    return false;
                }
                return true;
            }

            private static void Postfix(ref bool __result, string key, ref string result)
            {
                if (__result) return;
                __result = minorVocabulary.TryGetValue(key, out result);
            }
        }

        [HarmonyPatch(typeof(Language), "Contains")]
        private static class LanguageContainsPatch
        {
            private static bool Prefix(ref bool __result, string key)
            {
                if (majorVocabulary[CurrentLanguage].ContainsKey(key))
                {
                    __result = true;
                    return false;
                }
                return true;
            }

            private static void Postfix(ref bool __result, string key)
            {
                if (__result) return;
                __result = minorVocabulary.ContainsKey(key);
            }
        }
    }
}
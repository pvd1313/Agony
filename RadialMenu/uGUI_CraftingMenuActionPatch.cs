using UnityEngine;
using HarmonyLib;

namespace Agony.RadialTabs
{
    [HarmonyPatch(typeof(uGUI_CraftingMenu), "Action")]
    internal static class uGUI_CraftingMenuActionPatch
    {
        private static void Postfix(uGUI_CraftingMenu __instance, uGUI_CraftNode sender)
        {
            var client = __instance._client;
            var interactable = __instance.interactable;
            if (client == null || !interactable || !__instance.ActionAvailable(sender))
            {
                if (sender.icon == null) { return; }
                var duration = 1 + Random.Range(-0.2f, 0.2f);
                sender.icon.PunchScale(5, 0.5f, duration, 0);
            }
        }
    }   
}
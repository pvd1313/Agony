using Agony.Common.Reflection;
using UnityEngine;
using HarmonyLib;

namespace Agony.RadialTabs
{
    [HarmonyPatch(typeof(uGUI_CraftingMenu), "Action")]
    internal static class uGUI_CraftingMenuActionPatch
    {
        private static void Postfix(uGUI_CraftingMenu __instance, uGUI_CraftNode sender)
        {
            var client = uGUI_CraftingMenuReflector.GetClient(__instance);
            var interactable = uGUI_CraftingMenuReflector.GetInteractable(__instance);
            if (client == null || !interactable || !__instance.ActionAvailable(sender))
            {
                if (sender.icon == null) { return; }
                var duration = 1 + Random.Range(-0.2f, 0.2f);
                sender.icon.PunchScale(5, 0.5f, duration, 0);
            }
        }
    }   
}
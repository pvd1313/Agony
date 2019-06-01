using Agony.Common.Reflection;
using Harmony;

namespace Agony.Defabricator
{
    partial class Main
    {
        private static partial class GUIHandler
        {
            [HarmonyPatch(typeof(uGUI_CraftingMenu), "OnSelect")]
            private static class uGUI_CraftingMenuOnSelectPatch
            {
                private static void Postfix(uGUI_CraftingMenu __instance) { OnCraftingMenuSelected(__instance); }
            }

            [HarmonyPatch(typeof(uGUI_CraftingMenu), "OnDeselect")]
            private static class uGUI_CraftingMenuOnDeselectPatch
            {
                private static void Postfix(uGUI_CraftingMenu __instance) { OnCraftingMenuDeselected(__instance); }
            }

            [HarmonyPatch(typeof(uGUI_CraftingMenu), "ActionAvailable")]
            private static class uGUI_CraftingMenuActionAvailablePatch
            {
                private static void Postfix(uGUI_CraftingMenu __instance, ref bool __result, uGUI_CraftNode sender)
                {
                    if (!Active) return;
                    if (CurrentMenu != __instance) return;
                    if (sender.action != TreeAction.Craft) return;
                    __result &= !RecyclingData.IsBlackListed(sender.techType0);
                }
            }

            [HarmonyPatch(typeof(uGUI_CraftNode), "CreateIcon")]
            private static class CreateIconPatch
            {
                private static void Postfix(uGUI_CraftNode __instance)
                {
                    if (!Active) return;
                    if (uGUI_CraftNodeReflector.GetView(__instance) != CurrentMenu) return;

                    GUIFormatter.PaintNodeColor(__instance);
                }
            }

            [HarmonyPatch(typeof(uGUI_CraftNode), "UpdateIcon")]
            private static class uGUI_CraftNodeUpdateIconPatch
            {
                private static void Postfix(uGUI_CraftNode __instance, bool available)
                {
                    if (!Active) return;
                    if (uGUI_CraftNodeReflector.GetView(__instance) != CurrentMenu) return;

                    if (uGUI_CraftNodeReflector.GetVisible(__instance))
                    {
                        var enabled = available && !uGUI_CraftNodeReflector.IsLockedInHierarchy(__instance);
                        GUIFormatter.SetNodeChroma(__instance, enabled);
                    }
                }
            }

            public static uGUI_CraftingMenu CurrentMenu { get; private set; }

            private static void OnCraftingMenuSelected(uGUI_CraftingMenu sender)
            {
                Deactivate();
                CurrentMenu = sender;
            }

            private static void OnCraftingMenuDeselected(uGUI_CraftingMenu sender)
            {
                Deactivate();
                CurrentMenu = null;
            }
        }
    }
}
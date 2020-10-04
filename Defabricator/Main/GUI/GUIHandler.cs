using HarmonyLib;
#if BELOWZERO
using uGUI_CraftNode = uGUI_CraftingMenu.Node;
#endif

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
#if SUBNAUTICA
                    __result &= !RecyclingData.IsBlackListed(sender.techType0);
#elif BELOWZERO
                    __result &= !RecyclingData.IsBlackListed(sender.techType);
#endif
                }
            }
#if SUBNAUTICA

            [HarmonyPatch(typeof(uGUI_CraftNode), "CreateIcon")]
            private static class CreateIconPatch
            {
                private static void Postfix(uGUI_CraftNode __instance)
                {
                    if (!Active) return;
                    if (__instance.view != CurrentMenu) return;

                    GUIFormatter.PaintNodeColor(__instance);
                }
            }

            [HarmonyPatch(typeof(uGUI_CraftNode), "UpdateIcon")]
            private static class uGUI_CraftNodeUpdateIconPatch
            {
                private static void Postfix(uGUI_CraftNode __instance, bool available)
                {
                    if (!Active) return;
                    if (__instance.view != CurrentMenu) return;

                    if (__instance.visible)
                    {
                        var enabled = available && !__instance.IsLockedInHierarchy();
                        GUIFormatter.SetNodeChroma(__instance, enabled);
                    }
                }
            }
#elif BELOWZERO

            [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.CreateIcon))]
            private static class CreateIconPatch
            {
                private static void Postfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node)
                {
                    if (!Active)
                        return;
                    if (__instance != CurrentMenu)
                        return;
                    GUIFormatter.PaintNodeColor(node);
                }
            }

            [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.UpdateIcons))]
            private static class uGUI_CraftingMenuUpdateIconsPatch
            {
                private static void Postfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node)
                {
                    if (!Active)
                        return;
                    if (__instance != CurrentMenu)
                        return;
                    GUIFormatter.SetNodeChroma(node, true);
                }
            }
#endif
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
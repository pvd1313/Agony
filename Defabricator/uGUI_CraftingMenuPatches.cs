namespace Agony.Defabricator
{
    using HarmonyLib;

    [HarmonyPatch]
    public static partial class uGUI_CraftingMenuPatches
    {

        [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.OnDeselect))]
        [HarmonyPostfix]
        private static void OnDeselectPostfix(uGUI_CraftingMenu __instance) { OnCraftingMenuDeselected(__instance); }

        [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.ActionAvailable))]
        [HarmonyPostfix]
        private static void ActionAvailablePostfix(uGUI_CraftingMenu __instance, ref bool __result, uGUI_CraftingMenu.Node sender)
        {
            if(!Main.Active)
                return;
            if(CurrentMenu != __instance)
                return;
            if(sender.action != TreeAction.Craft)
                return;
            __result &= !RecyclingData.IsBlackListed(sender.techType);
        }

        [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.OnSelect))]
        [HarmonyPostfix]
        private static void OnSelectPostfix(uGUI_CraftingMenu __instance) { OnCraftingMenuSelected(__instance); }

        [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.UpdateIcons))]
        [HarmonyPostfix]
        private static void UpdateIconsPostfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node)
        {
            if(!Main.Active)
                return;
            if(__instance != CurrentMenu)
                return;
            GUIFormatter.SetNodeChroma(node, true);
        }

        [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.CreateIcon))]
        [HarmonyPostfix]
        private static void CreateIconPostfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node)
        {
            if(!Main.Active)
                return;
            if(__instance != CurrentMenu)
                return;
            GUIFormatter.PaintNodeColor(node);
        }

        public static uGUI_CraftingMenu CurrentMenu { get; private set; }

        private static void OnCraftingMenuSelected(uGUI_CraftingMenu sender)
        {
            Main.Deactivate();
            CurrentMenu = sender;
        }

        private static void OnCraftingMenuDeselected(uGUI_CraftingMenu sender)
        {
            Main.Deactivate();
            CurrentMenu = null;
        }
    }
}
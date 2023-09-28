namespace Agony.RadialTabs;

using HarmonyLib;
using UnityEngine;

[HarmonyPatch]
public static class uGUI_CraftingMenu_Patches
{
    [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.IsGrid))]
    [HarmonyPrefix]
    public static bool IsGridPrefix(ref bool __result)
    {
        __result = false;
        return false;
    }

    [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.CreateIcon))]
    [HarmonyPostfix]
    public static void CreateIconPostfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node)
    {
        RadialCell radialCell = RadialCell.Create(node);
        uGUI_ItemIcon icon = node.icon;
        Vector2 vector = new(radialCell.size, radialCell.size);
        icon.SetBackgroundSize(vector);
        icon.SetActiveSize(vector);
        float num = radialCell.size * (float)Config.IconForegroundSizeMult;
        icon.SetForegroundSize(num, num, true);
        icon.SetBackgroundRadius(radialCell.size / 2f);
        icon.rectTransform.SetParent(__instance.iconsCanvas);
        icon.SetPosition(radialCell.parent.Position);
        Vector2 position = !node.isRoot
            ? node.parent is not uGUI_CraftingMenu.Node node2 || !node2.expanded ? radialCell.parent.Position : radialCell.Position
            : radialCell.Position;

        float speed = (radialCell.radius + radialCell.size) * (float)Config.AnimationSpeedMult;
        float fadeDistance = radialCell.size * (float)Config.AnimationFadeDistanceMult;
        new IconMovingAnimation(speed, fadeDistance, position).Play(node.icon);
    }

    [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.GetIconMetrics))]
    [HarmonyPrefix]
    private static void Prefix(uGUI_CraftingMenu __instance, ref uGUI_CraftingMenu.Node node)
    {
        uGUI_ItemIcon icon = node.icon;
        if(icon != null)
        {
            __instance.icons.Remove(icon);
            Object.Destroy(icon.gameObject);
            node.icon = null;
        }
    }

    [HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.Action))]
    [HarmonyPostfix]
    public static void ActionPostfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node sender)
    {
        bool client = __instance._client != null;
        bool interactable = __instance.interactable;
        if(!client || !interactable || !__instance.ActionAvailable(sender))
        {
            if(sender.icon == null)
            {
                return;
            }
            float num = 1f + Random.Range(-0.2f, 0.2f);
            sender.icon.PunchScale(5f, 0.5f, num, 0f);
        }
    }
}
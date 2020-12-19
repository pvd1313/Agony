using UnityEngine;
using HarmonyLib;

namespace Agony.RadialTabs
{
    internal static class uGUI_CraftNodePatches
    {
        [HarmonyPatch(typeof(uGUI_CraftNode), "CreateIcon")]
        private static class CreateIconPatch
        {
            private static void Postfix(uGUI_CraftNode __instance)
            {
                var grid = RadialCell.Create(__instance);
                var icon = __instance.icon;
                var size = new Vector2(grid.size, grid.size);
                icon.SetBackgroundSize(size);
                icon.SetActiveSize(size);
                var foregroundSize = grid.size * (float)Config.IconForegroundSizeMult;
                icon.SetForegroundSize(foregroundSize, foregroundSize, true);
                icon.SetBackgroundRadius(grid.size / 2);
                icon.rectTransform.SetParent(__instance.view.iconsCanvas);
                icon.SetPosition(grid.parent.Position);
            }
        }

        [HarmonyPatch(typeof(uGUI_CraftNode), "SetVisible")]
        private static class SetVisiblePatch
        {
            private static void Postfix(uGUI_CraftNode __instance)
            {
                if (__instance.icon == null) return;
                var grid = RadialCell.Create(__instance);
                var pos = __instance.visible ? grid.Position : grid.parent.Position;
                var speed = (grid.radius + grid.size) * (float)Config.AnimationSpeedMult;
                var fadeDistance = grid.size * (float)Config.AnimationFadeDistanceMult;
                var anim = new IconMovingAnimation(speed, fadeDistance, pos);
                anim.Play(__instance.icon);
            }
        }
        
        [HarmonyPatch(typeof(uGUI_CraftNode), "Punch")]
        private static class PunchPatch
        {
            private static bool Prefix()
            {
                return false;
            }
        }
    }
}
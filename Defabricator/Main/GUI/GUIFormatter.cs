using UnityEngine;
#if BELOWZERO
using uGUI_CraftNode = uGUI_CraftingMenu.Node;
#endif

namespace Agony.Defabricator
{
    partial class Main
    {
        private static class GUIFormatter
        {
            private const float AnimationDuration = .3f;
            private static readonly Color backColor = new Color(1, 0, 0);
            private static readonly Color frontColor = new Color(1, .7f, .7f);

            public static void PaintNodeColor(uGUI_CraftNode node)
            {
#if SUBNAUTICA
                var frontColor = RecyclingData.IsBlackListed(node.techType0) ? Color.black : GUIFormatter.frontColor;
#elif BELOWZERO
                var frontColor = RecyclingData.IsBlackListed(node.techType) ? Color.black : GUIFormatter.frontColor;
#endif
                SetIconColors(node.icon, frontColor, backColor);
            }

            public static void RevertNodeColor(uGUI_CraftNode node)
            {
                SetIconColors(node.icon, Color.white, Color.white);
            }

            private static void SetIconColors(uGUI_ItemIcon icon, Color frontColor, Color backColor)
            {
                if (!icon) return;
                icon.SetForegroundColors(frontColor, frontColor, frontColor);
                icon.SetBackgroundColors(backColor, backColor, backColor);
            }

            public static void SetNodeChroma(uGUI_CraftNode node, bool enabled)
            {
                if (!node.icon) return;
                node.icon.SetForegroundChroma(enabled ? 1 : 0);
                node.icon.SetBackgroundChroma(enabled ? 1 : .2f);
            }

            public static void PaintNodeColorAnimated(uGUI_CraftNode node)
            {
#if SUBNAUTICA
                var frontColor = RecyclingData.IsBlackListed(node.techType0) ? Color.black : GUIFormatter.frontColor;
#elif BELOWZERO
                var frontColor = RecyclingData.IsBlackListed(node.techType) ? Color.black : GUIFormatter.frontColor;
#endif
                AnimateIconColor(node.icon, frontColor, backColor);
            }

            public static void RevertNodeColorAnimated(uGUI_CraftNode node)
            {
                AnimateIconColor(node.icon, Color.white, Color.white);
            }

            private static void AnimateIconColor(uGUI_ItemIcon icon, Color frontColor, Color backColor)
            {
                if (!icon) return;
                var anim = new IconColorAnimation(frontColor, backColor, AnimationDuration);
                anim.Play(icon);
            }
        }
    }
}
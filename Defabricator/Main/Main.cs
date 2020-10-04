using System.Collections.Generic;
using System;
using FMOD;
using UWE;
using System.Collections;
using UnityEngine;
#if BELOWZERO
using uGUI_CraftNode = uGUI_CraftingMenu.Node;
#endif

namespace Agony.Defabricator
{
    internal static partial class Main
    {
        public static bool Active { get; private set; }

        private static Dictionary<uGUI_CraftNode, TechType> replacedNodeTechs = new Dictionary<uGUI_CraftNode, TechType>();

        public static void Patch() 
        { 
            KeyInputHandler.Patch();
        }

        public static bool IsCurrentCrafter(Crafter crafter)
        {
            if (!Active) return false;
            if (!crafter) return false;
            return crafter as ITreeActionReceiver == GUIHandler.CurrentMenu.client;
        }

#if SUBNAUTICA
        private static void Activate()
        {
            if (Active) return;
            if (!GUIHandler.CurrentMenu) return;
            Active = true;

            int c = 0, n = 0;
            uGUI_CraftNode menuRoot = GUIHandler.CurrentMenu.icons;

            ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x));
            menuRoot?.UpdateRecursively(ref c, ref n);
            ForeachChildRecursively(menuRoot, x => GUIFormatter.PaintNodeColorAnimated(x));

        }

        private static void Deactivate()
        {
            if (!Active) return;
            Active = false;

            int c = 0, n = 0;
            uGUI_CraftNode menuRoot = GUIHandler.CurrentMenu.icons;

            ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x));
            menuRoot?.UpdateRecursively(ref c, ref n); 
            ForeachChildRecursively(menuRoot, x => GUIFormatter.RevertNodeColorAnimated(x));
        }

        private static void ForeachChildRecursively(uGUI_CraftNode node, Action<uGUI_CraftNode> action)
        {
            if (node == null) return;
            foreach (var child in node)
            {
                action(child);
                ForeachChildRecursively(child, action);
            }
        }

        private static void ReplaceNodeTech(uGUI_CraftNode node)
        {
            if (node.action != TreeAction.Craft) return;

            if (!node.techType0.ToString().StartsWith("Defabricated") && RecyclingData.TryGet(node.techType0, out TechType recyclingTech))
            {
                node.techType0 = recyclingTech;
            }
            else if (node.techType0.ToString().StartsWith("Defabricated"))
            {
                TechTypeExtensions.FromString(node.techType0.ToString().Replace("Defabricated", ""), out TechType original, true);
                node.techType0 = original;
            }
            else
            {
                ErrorMessage.AddMessage($"Failed to change {node.techType0}");
            }
        }
#elif BELOWZERO
        private static void Activate()
        {
            if (Active) return;
            if (!GUIHandler.CurrentMenu) return;
            Active = true;

            int c = 0, n = 0;
            var menuRoot = GUIHandler.CurrentMenu.tree;

            ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x, true));
            GUIHandler.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
            ForeachChildRecursively(menuRoot, x => GUIFormatter.PaintNodeColorAnimated(x));
        }

        private static void Deactivate()
        {
            if (!Active) return;
            Active = false;

            int c = 0, n = 0;
            var menuRoot = GUIHandler.CurrentMenu.tree;

            ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x, false));
            GUIHandler.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
            ForeachChildRecursively(menuRoot, x => GUIFormatter.RevertNodeColorAnimated(x));
        }

        private static void ForeachChildRecursively(uGUI_CraftNode node, Action<uGUI_CraftNode> action)
        {
            if (node == null) return;
            foreach (var child in node)
            {
                action(child);
                ForeachChildRecursively(child, action);
            }
        }

        private static void ReplaceNodeTech(uGUI_CraftNode node, bool activate)
        {
            if (node.action != TreeAction.Craft)
                return;

            if (!node.techType.ToString().StartsWith("Defabricated") && activate && RecyclingData.TryGet(node.techType, out TechType recyclingTech))
            {
                replacedNodeTechs[node] = node.techType;
                node.techType = recyclingTech;
            }
            else if (node.techType.ToString().StartsWith("Defabricated") && !activate)
            {
                string techString = node.techType.ToString().Replace("Defabricated", "");
                if(Enum.TryParse(techString, out TechType techType))
                {
                    replacedNodeTechs[node] = node.techType;
                    node.techType = techType;
                }
            }
        }

#endif
    }
}
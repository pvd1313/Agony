using System.Collections.Generic;
using Agony.Common.Reflection;
using System;
using FMOD;
using HarmonyLib;

namespace Agony.Defabricator
{
    internal static partial class Main
    {
        public static bool Active { get; private set; }

        private static Dictionary<uGUI_CraftNode, TechType> replacedNodeTechs = new Dictionary<uGUI_CraftNode, TechType>();

        public static void Patch() { KeyInputHandler.Patch(); }

        public static bool IsCurrentCrafter(Crafter crafter)
        {
            if (!Active) return false;
            if (!crafter) return false;
            return crafter as ITreeActionReceiver == GUIHandler.CurrentMenu.client;
        }

        private static void Activate()
        {
            if (Active) return;
            if (!GUIHandler.CurrentMenu) return;
            Active = true;

            int c = 0, n = 0;
            uGUI_CraftNode menuRoot = uGUI_CraftingMenuReflector.GetIcons(GUIHandler.CurrentMenu);

            if (!Language.main.TryGet("DefabricatedTitanium", out _))
            {
                foreach(TechType techType in Enum.GetValues(typeof(TechType)))
                {
                    if(SMLHelper.V2.Handlers.CraftDataHandler.GetTechData(techType) != null)
                    {
                        RecyclingData.TryGet(techType, out _, true);
                    }
                }

                Language.main.LoadLanguageFile(Language.main.GetCurrentLanguage());
                CraftData.RebuildDatabase();
            }

            ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x));
            menuRoot?.UpdateRecursively(ref c, ref n);
            ForeachChildRecursively(menuRoot, x => GUIFormatter.PaintNodeColorAnimated(x));

        }

        private static void Deactivate()
        {
            if (!Active) return;
            Active = false;

            int c = 0, n = 0;
            uGUI_CraftNode menuRoot = uGUI_CraftingMenuReflector.GetIcons(GUIHandler.CurrentMenu);

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
                TechTypeExtensions.FromString(node.techType0.ToString().Replace("Defabricated", ""), out node.techType0, true);
            }
        }
    }
}
namespace Agony.Defabricator
{
    using System.Reflection;
    using HarmonyLib;
    using System;
    using BepInEx;
    using BepInEx.Logging;
    using global::Common;
    using UnityEngine;

    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    public class Main: BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "Defabricator",
            AUTHOR = "PVD-MrPurple6411",
            GUID = "com.pvd.agony.defabricator",
            VERSION = "1.0.0.0";

        private const KeyCode ActivationKey = KeyCode.C;

        #endregion

        public void Awake()
        {
            Logging.Initialize(Logger);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.pvd.agony.defabricator");
            Logger.LogInfo("Patched");
        }

        public void Update()
        {
            if(Input.GetKeyDown(ActivationKey))
            {
                if(!Active)
                { Activate(); }
                else
                { Deactivate(); }
            }
        }

        public static bool Active { get; private set; }

        public static bool IsCurrentCrafter(Crafter crafter)
        {
            if(!Active)
                return false;
            if(!crafter)
                return false;
            return crafter as ITreeActionReceiver == uGUI_CraftingMenuPatches.CurrentMenu.client;
        }
        internal static void Activate()
        {
            if(Active)
                return;
            if(!uGUI_CraftingMenuPatches.CurrentMenu)
                return;
            Active = true;

            int c = 0, n = 0;
            var menuRoot = uGUI_CraftingMenuPatches.CurrentMenu.tree;

            if(menuRoot != null)
            {
                ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x));
                uGUI_CraftingMenuPatches.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
                ForeachChildRecursively(menuRoot, x => GUIFormatter.PaintNodeColorAnimated(x));
            }
        }

        internal static void Deactivate()
        {
            if(!Active)
                return;
            Active = false;

            int c = 0, n = 0;
            var menuRoot = uGUI_CraftingMenuPatches.CurrentMenu.tree;

            if(menuRoot != null)
            {
                ForeachChildRecursively(menuRoot, x => ReplaceNodeTech(x));
                uGUI_CraftingMenuPatches.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
                ForeachChildRecursively(menuRoot, x => GUIFormatter.RevertNodeColorAnimated(x));
            }
        }

        private static void ForeachChildRecursively(uGUI_CraftingMenu.Node node, Action<uGUI_CraftingMenu.Node> action)
        {
            if(node == null)
                return;
            foreach(var child in node)
            {
                action(child);
                ForeachChildRecursively(child, action);
            }
        }

        private static void ReplaceNodeTech(uGUI_CraftingMenu.Node node)
        {
            if(node.action != TreeAction.Craft)
                return;


            if(!node.techType.ToString().StartsWith("Defabricated") && RecyclingData.TryGet(node.techType, out TechType recyclingTech))
            {
                node.techType = recyclingTech;
            }
            else if(node.techType.ToString().StartsWith("Defabricated"))
            {
                TechTypeExtensions.FromString(node.techType.ToString().Replace("Defabricated", ""), out TechType original, true);
                node.techType = original;
            }
            else
            {
                ErrorMessage.AddMessage($"Failed to change {node.techType}");
            }
        }
    }
}
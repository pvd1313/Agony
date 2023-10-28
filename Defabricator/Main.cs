namespace Defabricator;

using System.Reflection;
using HarmonyLib;
using System;
using BepInEx;
using Common;
using UnityEngine;
using BepInEx.Configuration;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
public class Main: BaseUnityPlugin
{
    private KeyCode ActivationKey => ActivationKeyEntry.Value;

    public ConfigEntry<KeyCode> ActivationKeyEntry { get; init; }

    public Main()
    {
        ActivationKeyEntry = Config.Bind("Defabricator", "Activator", KeyCode.C, "The key to press in order to activate the Defabrication menu.");
    }

    public void Awake()
    {
        Logging.Initialize(Logger);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.pvd.agony.defabricator");
        Logger.LogInfo("Patched");
    }

    public void Update()
    {
        if (!uGUI_CraftingMenuPatches.CurrentMenu)
            return;

        if (!Input.GetKeyDown(ActivationKey))
            return;

        if (!Active)
            Activate();
        else
            Deactivate();
    }

    public static bool Active { get; private set; }

    public static bool IsCurrentCrafter(Crafter crafter)
    {
        return Active && crafter && (crafter as ITreeActionReceiver) == uGUI_CraftingMenuPatches.CurrentMenu.client;
    }

    internal static void Activate()
    {
        if(Active)
            return;
        Active = true;

        int c = 0, n = 0;
        var menuRoot = uGUI_CraftingMenuPatches.CurrentMenu.tree;

        if(menuRoot != null)
        {
            ForeachChildRecursively(menuRoot, ReplaceNodeTech);
            uGUI_CraftingMenuPatches.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
            ForeachChildRecursively(menuRoot, GUIFormatter.PaintNodeColorAnimated);
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
            ForeachChildRecursively(menuRoot, ReplaceNodeTech);
            uGUI_CraftingMenuPatches.CurrentMenu.UpdateNotifications(menuRoot, ref c, ref n);
            ForeachChildRecursively(menuRoot, GUIFormatter.RevertNodeColorAnimated);
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

        if(RecyclingData.TryGet(node.techType, out TechType alternateTech))
            node.techType = alternateTech;
    }
}
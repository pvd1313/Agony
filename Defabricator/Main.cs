namespace Defabricator;

using System.Reflection;
using HarmonyLib;
using System;
using BepInEx;
using Common;
using UnityEngine;
using BepInEx.Configuration;
using System.Collections;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID, Nautilus.PluginInfo.PLUGIN_VERSION)]
[BepInIncompatibility("com.ahk1221.smlhelper")]
[BepInDependency("com.mrpurple6411.CustomCraft3", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.mrpurple6411.AIOFabricator", BepInDependency.DependencyFlags.SoftDependency)]
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
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
        Logger.LogInfo("Patched");
    }

    internal static bool Initialized = false;

    public IEnumerator Start()
    {
        yield return new WaitUntil(() => CraftData.cacheInitialized && CraftTree.initialized);
        yield return new WaitForEndOfFrame();
        foreach (CraftTree.Type type in Enum.GetValues(typeof(CraftTree.Type)))
        {
            var tree = CraftTree.GetTree(type);
            if (tree == null)
                continue;

            foreach (var node in tree.nodes)
                yield return RecursivlyCreateDefabricatedTechTypes(node);
        }

        Initialized = true;
    }

    private IEnumerator RecursivlyCreateDefabricatedTechTypes(TreeNode node)
    {
        if (node is not CraftNode craftNode)
        {
            Logger.LogDebug($"{node.id} is not a CraftNode");
            yield break;
        }

        if (craftNode.action != TreeAction.Craft)
        {
            foreach (var child in craftNode.nodes)
                    yield return RecursivlyCreateDefabricatedTechTypes(child);
            yield break;
        }

        string techString = craftNode.techType0.AsString();
        if (craftNode.techType0 != TechType.None && (RecyclingData.TryGet(craftNode.techType0, out var alternateTech) || alternateTech != TechType.None))
            Logger.LogDebug($"Successfully setup alternate tech {Language.main.GetOrFallback(techString, techString)}");
        yield break;
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
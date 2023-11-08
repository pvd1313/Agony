namespace RadialTabs;

using System.Reflection;
using HarmonyLib;
using System;
using BepInEx;
using BepInEx.Logging;
using Common;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Main: BaseUnityPlugin
{
    public void Awake()
    {
        Logging.Initialize(Logger);
        try
        { 
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception e)
        {
            Logger.Log(LogLevel.Error, e);
        }
    }
}
using System.Reflection;
using HarmonyLib;
using System;
using QModManager.API.ModLoading;
using QModManager.Utility;

namespace Agony.RadialTabs
{
    [QModCore]
    internal static class QMod
    {
        [QModPatch]
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.pvd.agony.radialcraftingtabs");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Logger.Log(Logger.Level.Error, null, e);
            }
        }
    }
}
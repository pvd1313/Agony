using System.Reflection;
using HarmonyLib;
using System;
using QModManager.Utility;
using QModManager.API.ModLoading;

namespace Agony.Defabricator
{
    [QModCore]
    public static class QMod
    {
        [QModPatch]
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.pvd.agony.defabricator");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Main.Patch();
            }
            catch (Exception e)
            {
                Logger.Log(Logger.Level.Error, null, e);
            }
        }
    }
}
using System.Reflection;
using Agony.Common;
using Harmony;
using System;

namespace Agony.RadialTabs
{
    internal static class QMod
    {
        public static void Patch()
        {
            try
            {
                var harmony = HarmonyInstance.Create("com.pvd.agony.radialcraftingtabs");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }
    }
}
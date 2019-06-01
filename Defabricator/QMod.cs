using System.Reflection;
using Agony.Common;
using Harmony;
using System;

namespace Agony.Defabricator
{
    internal static class QMod
    {
        public static void Patch()
        {
            try
            {
                var harmony = HarmonyInstance.Create("com.pvd.agony.defabricator");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Main.Patch();
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }
    }
}
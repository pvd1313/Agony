using System.Reflection;
using Agony.Common;
using Harmony;
using System;

namespace Agony.WorkbenchFix
{
    internal static class QMod
    {
        public static void Patch()
        {
            try
            {
                var harmony = HarmonyInstance.Create("com.pvd.agony.workbenchfix");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                MaterialFix.Patch();
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }
    }
}

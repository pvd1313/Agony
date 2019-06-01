using System.Reflection;
using Agony.Common;
using Harmony;

namespace Agony.AssetTools
{
    internal static class QMod
    {
        public static void Patch()
        {
            try
            {
                var harmony = HarmonyInstance.Create("com.pvd.agony.assettools");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (System.Exception e)
            {
                Logger.Exception(e);
            }
        }
    }
}
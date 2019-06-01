using Agony.AssetTools.Wrappers;
using Agony.Common;
using UWE;

namespace Agony.Tests
{
    internal static class QMod
    {
        public static void Patch()
        {
            try
            {
                /*string classID;
                CraftDataWrapper.TryGetTechPrefab(TechType.Workbench, out classID);
                PrefabDatabaseWrapper.PreparePrefabDatabase();
                string name;
                PrefabDatabase.TryGetPrefabFilename(classID, out name);
                Logger.Message($"Prefab name = '{name}'");*/
                
            }
            catch (System.Exception e)
            {
                Logger.Exception(e);
            }
        }
    }
}
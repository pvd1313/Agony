namespace Agony.RadialTabs
{
    using System.Reflection;
    using HarmonyLib;
    using System;
    using BepInEx;
    using BepInEx.Logging;
    using global::Common;

    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main: BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "RadialTabs",
            AUTHOR = "PVD-MrPurple6411",
            GUID = "com.pvd.agony.radialcraftingtabs",
            VERSION = "1.0.0.0";

        #endregion


        public void Awake()
        {
            Logging.Initialize(Logger);
            try
            { 
                var harmony = new Harmony("com.pvd.agony.radialcraftingtabs");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, e);
            }
        }
    }
}
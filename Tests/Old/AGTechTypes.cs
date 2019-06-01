/*using SMLHelper.V2.Handlers;

namespace Agony
{
    public static class AGTechTypes
    {
        public static TechType SulfuricAcid { get; private set; }
        public static TechType DiamondDust { get; private set; }

        public static void AddNewItems()
        {
            AGLogger.LogMessage("Adding new items...");

            SulfuricAcid = AddItem("AGTechType_SulfuricAcid", "Sulfuric acid", "Mineral acid composed of sulfur. Used primarily as energy capacitor.");
            DiamondDust = AddItem("AGTechType_DiamondDust", "Diamond dust", "Crushed precious diamond. Used in advanced electronics.");
        }

        private static TechType AddItem(string id, string name, string tooltip = "")
        {
            return TechTypeHandler.AddTechType(id, name, tooltip, AGResources.LoadSprite(id), false);
        }
    }
}*/
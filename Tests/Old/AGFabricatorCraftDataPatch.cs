/*using SMLHelper.V2.Crafting;
using System.Collections.Generic;

namespace Agony
{
    namespace Patches
    {
        public static class AGFabricatorCraftDataPatch
        {
            private class AGFabricatorTab : AGCraftTreeTab
            {
                public AGFabricatorTab(string id, string displayName, AGCraftTreeTab parent = null)
                    : base(id, displayName, Fabricator, parent) { }
            }

            private const CraftTree.Type Fabricator = CraftTree.Type.Fabricator;

            private static readonly AGFabricatorTab ResourcesTab = new AGFabricatorTab("AGCraftTree_Fabricator_Resources", "Resources");
            private static readonly AGFabricatorTab MaterialsTab = new AGFabricatorTab("AGCraftTree_Fabricator_Materials", "Materials", ResourcesTab);
            private static readonly AGFabricatorTab ChemicalsTab = new AGFabricatorTab("AGCraftTree_Fabricator_Chemicals", "Chemicals", ResourcesTab);
            private static readonly AGFabricatorTab ElectronicsTab = new AGFabricatorTab("AGCraftTree_Fabricator_Electronics", "Electronics", ResourcesTab);

            private static readonly AGFabricatorTab SurvivalTab = new AGFabricatorTab("AGCraftTree_Fabricator_Survival", "Sustenance");

            private static readonly AGFabricatorTab PersonalTab = new AGFabricatorTab("AGCraftTree_Fabricator_Personal", "Personal");
            private static readonly AGFabricatorTab ToolsTab = new AGFabricatorTab("AGCraftTree_Fabricator_Tools", "Tools", PersonalTab);
            private static readonly AGFabricatorTab EquipmentTab = new AGFabricatorTab("AGCraftTree_Fabricator_Equipment", "Equipment", PersonalTab);
            private static readonly AGFabricatorTab DeployablesTab = new AGFabricatorTab("AGCraftTree_Fabricator_Deployables", "Deployables", PersonalTab);

            private static readonly AGFabricatorTab RecyclingTab = new AGFabricatorTab("AGCraftTree_Fabricator_Recycling", "Recycling");
            private static readonly AGFabricatorTab RecyclingMaterialsTab = new AGFabricatorTab("AGCraftTree_Fabricator_RecyclingMaterials", "Recycling materials", RecyclingTab);
            private static readonly AGFabricatorTab RecyclingElectronicsTab = new AGFabricatorTab("AGCraftTree_Fabricator_RecyclingElectronics", "Recycling electronics", RecyclingTab);
            private static readonly AGFabricatorTab RecyclingToolsTab = new AGFabricatorTab("AGCraftTree_Fabricator_RecyclingTools", "Recycling tools", RecyclingTab);
            private static readonly AGFabricatorTab RecyclingEquipmentTab = new AGFabricatorTab("AGCraftTree_Fabricator_RecyclingEquipment", "Recycling equipment", RecyclingTab);
            private static readonly AGFabricatorTab RecyclingDeployablesTab = new AGFabricatorTab("AGCraftTree_Fabricator_RecyclingDeployables", "Recycling deployables", RecyclingTab);

            public static void Apply()
            {
                PatchCraftTree();
                PatchRecipes();
            }

            private static void PatchCraftTree()
            {
                AGLogger.LogMessage("Patching fabricator craft tree...");
                AGCraftTreeTab.RemoveRootTabs(Fabricator, "Resources", "Survival", "Personal", "Machines");
                AGCraftTreeTab.AttachTabsToCraftingTree(ResourcesTab, MaterialsTab, ChemicalsTab, ElectronicsTab);
                AGCraftTreeTab.AttachTabsToCraftingTree(SurvivalTab);
                AGCraftTreeTab.AttachTabsToCraftingTree(PersonalTab, ToolsTab, EquipmentTab, DeployablesTab);
                AGCraftTreeTab.AttachTabsToCraftingTree(RecyclingTab, RecyclingMaterialsTab, RecyclingElectronicsTab, RecyclingToolsTab, RecyclingEquipmentTab, RecyclingDeployablesTab);
            }

            private static void PatchRecipes()
            {
                AGLogger.LogMessage("Patching fabricator recipes...");

                // Materials
                AGCraftDataHelper.SetItemCraft(TechType.TitaniumIngot, 1, MaterialsTab, new Ingredient(TechType.Titanium, 8), new Ingredient(TechType.Lead, 1));
                AGCraftDataHelper.SetItemCraft(TechType.Glass, 1, MaterialsTab, new Ingredient(TechType.Quartz, 8));
                AGCraftDataHelper.SetItemCraft(TechType.FiberMesh, 1, MaterialsTab, new Ingredient(TechType.CreepvinePiece, 1));
                AGCraftDataHelper.SetItemCraft(TechType.EnameledGlass, 1, MaterialsTab, new Ingredient(TechType.Glass, 2), new Ingredient(TechType.StalkerTooth, 3));
                AGCraftDataHelper.SetItemCraft(AGTechTypes.DiamondDust, 1, MaterialsTab, new Ingredient(TechType.Diamond, 3));
                AGCraftDataHelper.SetItemCraft(TechType.PlasteelIngot, 1, MaterialsTab, new Ingredient(TechType.TitaniumIngot, 1), new Ingredient(TechType.Lithium, 3));

                // Chemicals
                AGCraftDataHelper.SetItemCraft(TechType.Silicone, 1, ChemicalsTab, TechType.FiberMesh, TechType.Lubricant);
                AGCraftDataHelper.SetItemCraft(TechType.Bleach, 1, ChemicalsTab, new Ingredient(TechType.Salt, 3), new Ingredient(TechType.CoralChunk, 1));
                AGCraftDataHelper.SetItemCraft(TechType.Lubricant, 1, ChemicalsTab, TechType.CreepvineSeedCluster, TechType.Bleach);
                AGCraftDataHelper.SetItemCraft(AGTechTypes.SulfuricAcid, 1, ChemicalsTab, new Ingredient(TechType.AcidMushroom, 1), new Ingredient(TechType.CrashPowder, 3));

                // Electronics
                AGCraftDataHelper.SetItemCraft(TechType.CopperWire, 1, ElectronicsTab, new Ingredient(TechType.Copper, 4));
                AGCraftDataHelper.SetItemCraft(TechType.WiringKit, 1, ElectronicsTab, new Ingredient(TechType.CopperWire, 1), new Ingredient(TechType.Silver, 3));
                AGCraftDataHelper.SetItemCraft(TechType.ComputerChip, 1, ElectronicsTab, new Ingredient(TechType.JeweledDiskPiece, 1), new Ingredient(TechType.Gold, 3));
                AGCraftDataHelper.SetItemCraft(TechType.AdvancedWiringKit, 1, ElectronicsTab, TechType.WiringKit, TechType.ComputerChip, AGTechTypes.DiamondDust);
                AGCraftDataHelper.SetItemCraft(TechType.Battery, 1, ElectronicsTab, TechType.TitaniumIngot, AGTechTypes.SulfuricAcid, TechType.CopperWire);

                // Tools
                AGCraftDataHelper.SetItemCraft(TechType.Knife, 1, ToolsTab, TechType.TitaniumIngot);
                AGCraftDataHelper.SetItemCraft(TechType.Flashlight, 1, ToolsTab, TechType.TitaniumIngot, TechType.Glass, TechType.Silicone, TechType.Battery, TechType.CopperWire);
                AGCraftDataHelper.SetItemCraft(TechType.Scanner, 1, ToolsTab, TechType.TitaniumIngot, TechType.Glass, TechType.Silicone, TechType.Battery, TechType.WiringKit, TechType.ComputerChip);
                AGCraftDataHelper.SetItemCraft(TechType.Welder, 1, ToolsTab, TechType.TitaniumIngot, TechType.Glass, TechType.Silicone, TechType.Battery, TechType.WiringKit);
                AGCraftDataHelper.SetItemCraft(TechType.DiveReel, 1, ToolsTab, TechType.TitaniumIngot, TechType.Glass, TechType.Silicone, TechType.Battery, TechType.WiringKit, TechType.ComputerChip);
                AGCraftDataHelper.SetItemCraft(TechType.Builder, 1, ToolsTab, TechType.PlasteelIngot, TechType.EnameledGlass, TechType.Silicone, TechType.Battery, TechType.AdvancedWiringKit);
                AGCraftDataHelper.SetItemCraft(TechType.AirBladder, 1, ToolsTab, new Ingredient(TechType.Bladderfish, 1), new Ingredient(TechType.Silicone, 2));
                AGCraftDataHelper.SetItemCraft(TechType.Flare, 5, ToolsTab, TechType.TitaniumIngot, TechType.Silicone, TechType.CrashPowder);
                AGCraftDataHelper.SetItemCraft(TechType.FireExtinguisher, 1, ToolsTab, new Ingredient(TechType.FireExtinguisher, 1), new Ingredient(TechType.DisinfectedWater, 2));

                // Equipment
                AGCraftDataHelper.SetItemCraft(TechType.Tank, 1, EquipmentTab, new Ingredient(TechType.TitaniumIngot, 2), new Ingredient(TechType.Glass, 1), new Ingredient(TechType.Silicone, 1));
                AGCraftDataHelper.SetItemCraft(TechType.DoubleTank, 1, EquipmentTab, new Ingredient(TechType.Tank, 1), new Ingredient(TechType.TitaniumIngot, 4), new Ingredient(TechType.WiringKit, 1));
                AGCraftDataHelper.SetItemCraft(TechType.Fins, 1, EquipmentTab, new Ingredient(TechType.Silicone, 3));
                AGCraftDataHelper.SetItemCraft(TechType.RadiationSuit, 1, EquipmentTab, new List<TechType>() { TechType.RadiationHelmet, TechType.RadiationGloves },
                              new Ingredient(TechType.Silicone, 8), new Ingredient(TechType.Lead, 4), new Ingredient(TechType.Glass, 1));

                // Deployables
                AGCraftDataHelper.SetItemCraft(TechType.Beacon, 1, DeployablesTab, TechType.TitaniumIngot, TechType.AirBladder, TechType.Battery, TechType.WiringKit, TechType.ComputerChip);
                AGCraftDataHelper.SetItemCraft(TechType.SmallStorage, 1, DeployablesTab, TechType.TitaniumIngot, TechType.AirBladder);
                AGCraftDataHelper.SetItemCraft(TechType.Gravsphere, 1, DeployablesTab, TechType.TitaniumIngot, TechType.CopperWire, TechType.Battery);
                AGCraftDataHelper.SetItemCraft(TechType.PipeSurfaceFloater, 1, DeployablesTab, TechType.TitaniumIngot, TechType.AirBladder);
                AGCraftDataHelper.SetItemCraft(TechType.Pipe, 5, DeployablesTab, TechType.TitaniumIngot, TechType.Silicone);
                AGCraftDataHelper.SetItemCraft(TechType.Seaglide, 1, DeployablesTab, new Ingredient(TechType.TitaniumIngot, 3), new Ingredient(TechType.Silicone, 3), new Ingredient(TechType.Glass, 1),
                              new Ingredient(TechType.Lubricant, 2), new Ingredient(TechType.Flashlight, 1), new Ingredient(TechType.Battery, 1),
                              new Ingredient(TechType.WiringKit, 1), new Ingredient(TechType.ComputerChip, 1));

                // Survival
                AGCraftDataHelper.SetItemCraft(TechType.FilteredWater, 1, SurvivalTab, TechType.Bladderfish, TechType.FiberMesh);
                AGCraftDataHelper.SetItemCraft(TechType.DisinfectedWater, 2, SurvivalTab, TechType.Bleach);
                AGCraftDataHelper.SetItemCraft(TechType.CookedGarryFish, 1, SurvivalTab, TechType.GarryFish, TechType.CreepvinePiece);
                AGCraftDataHelper.SetItemCraft(TechType.FirstAidKit, 1, SurvivalTab, TechType.FiberMesh, TechType.Bleach);

                // Recycling
                AGCraftDataHelper.SetItemRecycle(TechType.ScrapMetal, 1, RecyclingMaterialsTab, TechType.Titanium);
                AGCraftDataHelper.SetItemRecycle(TechType.Battery, 1, RecyclingElectronicsTab, new Ingredient(TechType.Titanium, 6), new Ingredient(TechType.Copper, 3), new Ingredient(TechType.CrashPowder, 2));
                AGCraftDataHelper.SetItemRecycle(TechType.Flare, 1, RecyclingEquipmentTab, TechType.Titanium);
                AGCraftDataHelper.SetItemRecycle(TechType.PipeSurfaceFloater, 1, RecyclingDeployablesTab, new Ingredient(TechType.Titanium, 6), new Ingredient(TechType.Silicone, 1));
                AGCraftDataHelper.SetItemRecycle(TechType.Pipe, 1, RecyclingDeployablesTab, TechType.Titanium);
            }
        }
    }
}*/
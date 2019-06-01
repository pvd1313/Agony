/*using System.Collections.Generic;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Crafting;

namespace Agony
{
    public static class AGCraftDataHelper
    {
        public static void SetItemCraft(TechType item, int craftAmount, AGCraftTreeTab tab, params TechType[] ingredients)
        {
            var list = new List<Ingredient>();
            foreach (var tech in ingredients)
                list.Add(new Ingredient(tech, 1));
            SetItemCraft(item, craftAmount, tab, list.ToArray());
        }

        public static void SetItemCraft(TechType item, int craftAmount, AGCraftTreeTab tab, params Ingredient[] ingredients)
        {
            SetItemCraft(item, craftAmount, tab, new List<TechType>() { }, ingredients);
        }

        public static void SetItemCraft(TechType item, int craftAmount, AGCraftTreeTab tab, List<TechType> linkedItems, params Ingredient[] ingredients)
        {
            var techData = new TechData(ingredients);
            techData.craftAmount = craftAmount;
            techData.LinkedItems = linkedItems;
            CraftDataHandler.SetTechData(item, techData);
            tab.AddItem(item);
        }

        public static void SetItemRecycle(TechType item, int recycleAmount, AGCraftTreeTab tab, params TechType[] components)
        {
            var list = new List<Ingredient>();
            foreach (var component in components)
            {
                var ingredient = list.Find(x => x.techType == component);
                if (ingredient == null)
                {
                    list.Add(new Ingredient(component, 1));
                }
                else
                {
                    ingredient.amount++;
                }
            }
            SetItemRecycle(item, recycleAmount, tab, list.ToArray());
        }

        public static void SetItemRecycle(TechType item, int recycleAmount, AGCraftTreeTab tab, params Ingredient[] components)
        {
            var techID = "AGRecycling_" + item.AsString();
            var techName = "Recycle " + item;
            var techTooltip = CreateRecycleTooltip(components);
            var techSprite = SpriteManager.Get(item);
            var tech = TechTypeHandler.AddTechType(techID, techName, techTooltip, techSprite, false);

            var techData = new TechData(new Ingredient(item, recycleAmount));
            var linkedItems = new List<TechType>();
            foreach (var component in components)
                for (var i = 0; i < component.amount; i++)
                    linkedItems.Add(component.techType);
            techData.LinkedItems = linkedItems;

            CraftDataHandler.SetTechData(tech, techData);
            tab.AddItem(tech);
        }

        public static string CreateRecycleTooltip(Ingredient[] components)
        {
            switch (components.Length)
            {
                case 1: return "Scrap for " + components[0].techType + ".";
                case 2: return "Scrap for " + components[0].techType + " and " + components[1].techType + ".";
                case 3: return "Scrap for " + components[0].techType + ", " + components[1].techType + ", " + components[2].techType + ".";
                default: return "Scrap for materials.";
            }
        }
    }
}*/

namespace Agony.Defabricator
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Agony.Common;
    using HarmonyLib;
    using SMLHelper.V2.Crafting;
    using SMLHelper.V2.Handlers;
    using UWE;
    using UnityEngine;
#if SUBNAUTICA
    using RecipeData = SMLHelper.V2.Crafting.TechData;
    using BepInEx.Logging;
    using global::Common;
#endif

        internal static class RecyclingData
        {
            private static readonly HashSet<TechType> blacklist = new HashSet<TechType>(); 
            private static readonly Dictionary<TechType, TechType> cache = new Dictionary<TechType, TechType>();
            private static readonly string nonRecyclableText = "<color=#FF3030FF>Non-recyclable</color>";
            private static readonly string nonRecyclableTooltip = "Unfortunately there are no techniques that could be used in order to recycle this item.";
            private static readonly string recycleText = "<color=#00FA00FF>Recycle: </color> {0}";
            private static readonly string recycleTooltip = "Scrap for {0}.";

            static RecyclingData()
            {
            }

            public static bool IsBlackListed(TechType recyclingTech) => blacklist.Contains(recyclingTech);

            public static bool TryGet(TechType originTech, out TechType recyclingTech, bool initialRun = false)
            {
                recyclingTech = TechType.None;
                if (originTech == TechType.None) { return false; }
                if (cache.TryGetValue(originTech, out recyclingTech)) { return true; }
#if SUBNAUTICA
                RecipeData originData = CraftDataHandler.GetTechData(originTech);
#elif BELOWZERO
                RecipeData originData = CraftDataHandler.GetRecipeData(originTech);
#endif
                if (originData == null)
                {
                    if(!initialRun)
                        Logging.Logger.Log(LogLevel.Error, $"Failed to load RecipeData for TechType '{originTech}'.");
                    return false;
                }

                if (Config.IsBlacklisted(originTech))
                { blacklist.Add(originTech); }
                recyclingTech = CreateRecyclingData(originTech, originData);
                cache[originTech] = recyclingTech;
                
                return true;
            }

            

            private static TechType CreateRecyclingData(TechType originTech, RecipeData originData)
            {
                if (IsBlackListed(originTech))
                {
                    TechType blackListedTech = TechTypeHandler.AddTechType($"Defabricated{originTech}", LoadRecyclingText(originTech), LoadRecyclingTooltip(originTech, null), SpriteManager.Get(originTech), true);
                    CraftDataHandler.SetTechData(blackListedTech, new RecipeData(new List<Ingredient>()));
                    blacklist.Add(blackListedTech);
                    return blackListedTech;
                }

                var ingredients = new Dictionary<TechType, int>();
                if (originData.craftAmount > 0) { ingredients[originTech] = originData.craftAmount; }
                for (var i = 0; i < originData.linkedItemCount; i++)
                {
                    var item = originData.LinkedItems[i];
                    ingredients[item] = ingredients.ContainsKey(item) ? (ingredients[item] + 1) : 1;
                }
                var resIngs = new List<Ingredient>();
                ingredients.ForEach(x => resIngs.Add(new Ingredient(x.Key, x.Value)));

                var linkedItems = new List<TechType>();
                for(var i = 0; i < originData.ingredientCount; i++)
                {
                    var ing = originData.Ingredients[i];
                    var amount = Mathf.FloorToInt(ing.amount * Config.GetYield(ing.techType));
                    for(var j = 0; j < amount; j++) { linkedItems.Add(ing.techType); }
                }
                RecipeData Data = new RecipeData() { craftAmount = 0, Ingredients = resIngs, LinkedItems = linkedItems };
                DefabricatedPrefab defabricatedPrefab = new DefabricatedPrefab($"Defabricated{originTech}", LoadRecyclingText(originTech), LoadRecyclingTooltip(originTech, Data), originTech, Data);
                defabricatedPrefab.Patch();
                KnownTech.Add(defabricatedPrefab.TechType);

#if SUBNAUTICA
                Dictionary<string, Atlas> nameToAtlas = AccessTools.Field(typeof(Atlas), "nameToAtlas").GetValue(null) as Dictionary<string, Atlas>;

                foreach (Atlas atlas in nameToAtlas.Values)
                {
                    Atlas.Sprite sprite = atlas.GetSprite(originTech.AsString());
                    if(sprite != null)
                    {
                        atlas.nameToSprite[defabricatedPrefab.TechType.AsString()] = sprite;
                        break;
                    }
                }
				
#elif BELOWZERO
                Dictionary<string, Dictionary<string, Sprite>> atlases = AccessTools.Field(typeof(SpriteManager), "atlases").GetValue(null) as Dictionary<string, Dictionary<string, Sprite>>;

                foreach (Dictionary<string, Sprite> atlas in atlases.Values)
                {
                    if (atlas.TryGetValue(originTech.AsString(), out Sprite sprite))
                    {
                        atlas[defabricatedPrefab.TechType.AsString()] = sprite;
                        break;
                    }
                }
#endif
                if (IsBlackListed(originTech))
                {
                    blacklist.Add(defabricatedPrefab.TechType);
                }

                return defabricatedPrefab.TechType;
            }

            private static string LoadRecyclingText(TechType originTech)
            {
                var techName = Language.main.Get(originTech.AsString());
                var formated = FormatWithFallback(recycleText, techName, techName);

                if (IsBlackListed(originTech))
                {
                    formated = nonRecyclableText;
                }

                return formated;
            }

            private static string LoadRecyclingTooltip(TechType originTech, RecipeData data)
            {
                if (IsBlackListed(originTech))
                {
                    return nonRecyclableTooltip;
                }

                if (data == null)
                    return nonRecyclableTooltip;
                var ings = new Dictionary<TechType, int>();
                for(var i = 0; i < data.linkedItemCount; i++)
                {
                    var item = data.GetLinkedItem(i);
                    ings[item] = ings.ContainsKey(item) ? (ings[item] + 1) : 1;
                }

                var builder = new System.Text.StringBuilder();
                foreach(var ing in ings)
                {
                    builder.Append(Language.main.Get(ing.Key.AsString()));
                    if (ing.Value > 1)
                    {
                        builder.Append(" (x");
                        builder.Append(ing.Value);
                        builder.Append(')');
                    }
                    builder.Append(", ");
                }
                if (builder.Length >= 2) { builder.Length -= 2; }
                var ingList = builder.ToString();

                return FormatWithFallback(recycleTooltip, "{0}", ingList);
            }

            public static string FormatWithFallback(string unmanaged, string fallback, params object[] args)
            {
                if (fallback == null)
                    throw new ArgumentNullException("fallback is null");

                try
                {
                    return string.Format(unmanaged, args);
                }
                catch (FormatException) { }
                return string.Format(fallback, args);
            }
        }
}
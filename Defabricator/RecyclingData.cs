
namespace Defabricator;

using System;
using System.Collections.Generic;
using UnityEngine;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
#if SUBNAUTICA
using static CraftData;
using System.Linq;
#endif

public static class RecyclingData
{
    private static readonly HashSet<TechType> blacklist = new();
    private static readonly Dictionary<TechType, TechType> cache = new();
    internal static readonly Dictionary<TechType, TechType> reverseCache = new();
    private static readonly string nonRecyclableText = "<color=#FF3030FF>Non-recyclable</color>";
    private static readonly string nonRecyclableTooltip = "Unfortunately there are no techniques that could be used in order to recycle this item.";
    private static readonly string recycleText = "<color=#00FA00FF>Recycle: </color> {0}";
    private static readonly string recycleTooltip = "Scrap for {0}.";

    static RecyclingData()
    {
    }

    public static bool IsBlackListed(TechType recyclingTech) => blacklist.Contains(recyclingTech);

    public static bool TryGet(TechType originTech, out TechType alternateTech)
    {
        alternateTech = TechType.None;
        if (originTech == TechType.None) { return false; }
        if (Main.Initialized && !CrafterLogic.IsCraftRecipeUnlocked(originTech)) 
        {
                return false; 
        }

        if (cache.TryGetValue(originTech, out alternateTech))
        {
            if (!Main.Initialized)
                alternateTech = TechType.None;
            return Main.Active;
        }

        if (reverseCache.TryGetValue(originTech, out alternateTech)) 
        {
            if (!Main.Initialized)
                alternateTech = TechType.None;
            return !Main.Active; 
        }

        RecipeData originData = CraftDataHandler.GetRecipeData(originTech);
        if (originData == null)
            return false;

        alternateTech = CreateRecyclingData(originTech, originData);
        cache[originTech] = alternateTech;
        reverseCache[alternateTech] = originTech;
        return Main.Active;
    }

    private static TechType CreateRecyclingData(TechType originTech, RecipeData originData)
    {
        if (Config.IsBlacklisted(originTech))
        {
            blacklist.Add(originTech);
            TechType blackListedTech = EnumHandler.AddEntry<TechType>($"Defabricated{originTech}")
            .WithPdaInfo(LoadRecyclingText(originTech), LoadRecyclingTooltip(originTech, null))
            .WithIcon(SpriteManager.Get(originTech));
            CraftDataHandler.SetRecipeData(blackListedTech, new RecipeData(new List<Ingredient>()));
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
        for (var i = 0; i < originData.ingredientCount; i++)
        {
            var ing = originData.Ingredients[i];
            var amount = Mathf.FloorToInt(ing.amount * Config.GetYield(ing.techType));
            for (var j = 0; j < amount; j++) { linkedItems.Add(ing.techType); }
        }

        RecipeData Data = new() { craftAmount = 0, Ingredients = resIngs, LinkedItems = linkedItems };
        CustomPrefab defabricatedPrefab = new($"Defabricated{originTech}", LoadRecyclingText(originTech), LoadRecyclingTooltip(originTech, Data), SpriteManager.Get(originTech));
        CraftDataHandler.SetRecipeData(defabricatedPrefab.Info.TechType, Data);
        defabricatedPrefab.SetGameObject(new CloneTemplate(defabricatedPrefab.Info, originTech)
        {
            ModifyPrefab = (go) => go.SetActive(false)
        });
        defabricatedPrefab.Register();

        return defabricatedPrefab.Info.TechType;
    }

    private static string LoadRecyclingText(TechType originTech)
    {
        var techName = Language.main.Get(originTech.AsString());
        var formated = FormatWithFallback(recycleText, techName, techName);

        if (IsBlackListed(originTech))
            formated = nonRecyclableText;

        return formated;
    }

    private static string LoadRecyclingTooltip(TechType originTech, RecipeData data)
    {
        if (IsBlackListed(originTech) || data == null)
            return nonRecyclableTooltip;

        var ings = new Dictionary<TechType, int>();
        for (var i = 0; i < data.linkedItemCount; i++)
        {
            var item = data.GetLinkedItem(i);
            ings[item] = ings.ContainsKey(item) ? (ings[item] + 1) : 1;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var ing in ings)
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
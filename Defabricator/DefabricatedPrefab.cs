namespace Agony.Defabricator;

using Nautilus.Assets;
using Nautilus.Crafting;
using System.Diagnostics.CodeAnalysis;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets.Gadgets;

public class DefabricatedPrefab : CustomPrefab
{
    [SetsRequiredMembers]
    public DefabricatedPrefab(string classId, string friendlyName, string description, TechType original, RecipeData data) : 
        base(classId, friendlyName, description, SpriteManager.Get(original))
    {
        this.SetRecipe(data);
        SetGameObject(new CloneTemplate(Info, original));
        Register();
        if (!KnownTech.Contains(original))
            this.SetUnlock(original);
        else
            KnownTech.Add(Info.TechType, true);
    }
}

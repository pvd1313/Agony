namespace Agony.Defabricator
{
    using SMLHelper.Assets;
    using SMLHelper.Crafting;
    using System.Collections;
    using UnityEngine;
#if SUBNAUTICA
    using Sprite = Atlas.Sprite;
    using RecipeData = SMLHelper.Crafting.TechData;
#endif

    public class DefabricatedPrefab : Craftable
    {
        TechType Original;
        RecipeData recipeData;

        public DefabricatedPrefab(string classId, string friendlyName, string description, TechType original, RecipeData data) : base(classId, friendlyName, description)
        {
            Original = original;
            recipeData = data;
        }

        public override TechType RequiredForUnlock => Original;

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(Original, false);
            yield return task;
            GameObject prefab = task.GetResult();

            gameObject.Set(Object.Instantiate(prefab));
            yield break;
        }

        protected override RecipeData GetBlueprintRecipe()
        {
            return recipeData;
        }

        protected override Sprite GetItemSprite()
        {
            return SpriteManager.Get(Original);
        }
    }
}

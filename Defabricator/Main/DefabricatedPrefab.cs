using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
#if SUBNAUTICA
using Sprite = Atlas.Sprite;
using RecipeData = SMLHelper.V2.Crafting.TechData;
#endif

namespace Agony.Main
{
    internal class DefabricatedPrefab : Craftable
    {
        TechType Original;
        RecipeData Data;

        public DefabricatedPrefab(string classId, string friendlyName, string description, TechType original, RecipeData data) : base(classId, friendlyName, description)
        {
            Original = original;
            Data = data;
        }

        public override TechType RequiredForUnlock => Original;

        public override GameObject GetGameObject()
        {
            return CraftData.GetPrefabForTechType(Original);
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(Original, false);
            yield return task;

            gameObject.Set(task.GetResult());
            yield break;
        }

        protected override RecipeData GetBlueprintRecipe()
        {
            return Data;
        }

        protected override Sprite GetItemSprite()
        {
            return SpriteManager.Get(Original);
        }
    }
}

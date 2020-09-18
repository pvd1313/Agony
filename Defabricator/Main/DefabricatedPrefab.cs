using SMLHelper.V2.Assets;
using System.Collections;
using UnityEngine;
#if SUBNAUTICA
using Data = SMLHelper.V2.Crafting.TechData;
#elif BELOWZERO
using Data = SMLHelper.V2.Crafting.RecipeData;
#endif

namespace Agony.Main
{
    internal class DefabricatedPrefab : Craftable
    {
        TechType Original;
        Data Data;

        public DefabricatedPrefab(string classId, string friendlyName, string description, TechType original, Data data) : base(classId, friendlyName, description)
        {
            Original = original;
            Data = data;
        }
#if BELOWZERO
        public override GameObject GetGameObject()
        {
            return CraftData.GetPrefabForTechType(Original);
        }
#endif
        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(Original, false);
            yield return task;

            gameObject.Set(task.GetResult());
            yield break;
        }

        protected override Data GetBlueprintRecipe()
        {
            return Data;
        }

    }
}

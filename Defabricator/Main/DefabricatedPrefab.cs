using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Agony.Main
{
    internal class DefabricatedPrefab : Craftable
    {
        TechType Original;
        TechData Data;

        public DefabricatedPrefab(string classId, string friendlyName, string description, TechType original, TechData data) : base(classId, friendlyName, description)
        {
            Original = original;
            Data = data;
        }

        public override GameObject GetGameObject()
        {
            return CraftData.GetPrefabForTechType(Original);
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            gameObject.Set(GetGameObject());
            yield break;
        }

        protected override TechData GetBlueprintRecipe()
        {
            return Data;
        }

    }
}

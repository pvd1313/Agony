namespace Defabricator;

using Common.Animation;
using UnityEngine;
using UWE;
using System.Collections;

partial class CrafterFX
{
    partial class WorkbenchFX
    {
        public static class BeamMaterial
        {
            internal static Material original;
            internal static Material custom;

            static BeamMaterial()
            {
                CoroutineHost.StartCoroutine(GetMaterials());
            }

            private static IEnumerator GetMaterials()
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.Fabricator, false);
                yield return task;

                GameObject prefab = task.GetResult();
                original = prefab.GetComponent<Fabricator>().leftBeam.GetComponent<Renderer>().sharedMaterial;
                custom = new Material(original);

                var func = AnimationFuncs.SinusoidalColor(Config.BeamColor, Config.BeamAlphaColor, Config.BeamFrequency);
                var anim = new ShaderColorPropertyAnimation(ShaderPropertyID._TintColor, func);
                anim.Play(custom);
            }
        }
    }
}
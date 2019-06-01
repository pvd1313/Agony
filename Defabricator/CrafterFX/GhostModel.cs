using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Harmony;

namespace Agony.Defabricator
{
    partial class CrafterFX
    {
        private static class GhostModel
        {
            [HarmonyPatch(typeof(CrafterGhostModel), "UpdateProgress")]
            private static class CrafterGhostModelUpdateProgressPatch
            {
                private static void Prefix(CrafterGhostModel __instance, ref float progress)
                {
                    var crafter = __instance.GetComponent<Crafter>();
                    if (!Handler.IsModified(crafter)) return;
                    progress = 1 - progress;
                }
            }

            [HarmonyPatch(typeof(CrafterGhostModel), "UpdateModel")]
            private static class CrafterGhostModelUpdateModelPatch
            {
                private static readonly FieldInfo ghostMaterialsFieldInfo = typeof(CrafterGhostModel).GetField("ghostMaterials", BindingFlags.NonPublic | BindingFlags.Instance);

                private static void Postfix(CrafterGhostModel __instance)
                {
                    var crafter = __instance.GetComponent<Crafter>();
                    if (!Handler.IsModified(crafter)) return;

                    foreach (var mat in (List<Material>)ghostMaterialsFieldInfo.GetValue(__instance))
                    {
                        if (mat.name != "DontRender")
                        {
                            mat.SetColor(ShaderPropertyID._BorderColor, Config.GhostColor);
                        }
                    }
                }
            }
        }
    }  
}
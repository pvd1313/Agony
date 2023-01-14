namespace Agony.Defabricator
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using HarmonyLib;

    [HarmonyPatch]
    partial class CrafterFX
    {
        [HarmonyPatch]
        public static class GhostModel
        {
            [HarmonyPatch(typeof(CrafterGhostModel), nameof(CrafterGhostModel.UpdateProgress))]
            public static class CrafterGhostModelUpdateProgressPatch
            {
                private static void Prefix(CrafterGhostModel __instance, ref float progress)
                {
                    var crafter = __instance.GetComponent<Crafter>();
                    if (!Handler.IsModified(crafter)) return;
                    progress = 1 - progress;
                }
            }

            [HarmonyPatch(typeof(CrafterGhostModel), nameof(CrafterGhostModel.UpdateModel))]
            public static class CrafterGhostModelUpdateModelPatch
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
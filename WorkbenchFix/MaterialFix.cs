using Agony.Common.Animation;
using UnityEngine;
using System;

using AGLogger = Agony.Common.Logger;

namespace Agony.WorkbenchFix
{
    internal static class MaterialFix
    {
        private static readonly Color min = new Color(0.62f, 0.96f, 1.00f);
        private static readonly Color max = new Color(0.25f, 0.72f, 1.00f);
        private static readonly float frequency = 60;

        static MaterialFix()
        {
            try
            {
                //var prefab = CraftData.GetPrefabForTechType(TechType.Workbench, true);
                var workbench = Resources.Load<GameObject>("Submarine/Build/Workbench").GetComponent<Workbench>();
                var material = workbench.fxLaserBeam[0].GetComponent<Renderer>().sharedMaterial;
                var func = AnimationFuncs.SinusoidalColor(min, max, frequency);
                var anim = new ShaderColorPropertyAnimation(ShaderPropertyID._Color, func);
                anim.Play(material);
            }
            catch(Exception e) { AGLogger.Exception(e); }
        }

        public static void Patch() { }
    }
}
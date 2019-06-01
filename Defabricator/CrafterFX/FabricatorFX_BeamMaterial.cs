using Agony.Common.Animation;
using UnityEngine;
using System;

using AGLogger = Agony.Common.Logger;

namespace Agony.Defabricator
{
    partial class CrafterFX
    {
        partial class FabricatorFX
        {
            private static class BeamMaterial
            {
                public static readonly Material original;
                public static readonly Material custom;

                static BeamMaterial()
                {
                    try
                    {
                        var prefab = CraftData.GetPrefabForTechType(TechType.Fabricator);
                        original = prefab.GetComponent<Fabricator>().leftBeam.GetComponent<Renderer>().sharedMaterial;
                        custom = new Material(original);

                        var func = AnimationFuncs.SinusoidalColor(Config.BeamColor, Config.BeamAlphaColor, Config.BeamFrequency);
                        var anim = new ShaderColorPropertyAnimation(ShaderPropertyID._TintColor, func);
                        anim.Play(custom);
                    }
                    catch (Exception e) { AGLogger.Exception(e); }
                }
            }
        }
    }
}
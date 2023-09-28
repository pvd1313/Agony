namespace Defabricator;

using System.Reflection;
using UnityEngine;
using System;
using BepInEx.Logging;
using Common;

partial class CrafterFX
{
    private static partial class FabricatorFX
    {
        private sealed class ParticleSystemParameters
        {
            public ParticleSystem.MinMaxGradient startColor;

            public void Apply(ParticleSystem particleSystem)
            {
                if (!particleSystem) return;
                var main = particleSystem.main;
                main.startColor = startColor;
            }
        }

        private static readonly FieldInfo sparksLFieldInfo = typeof(Fabricator).GetField("sparksL", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo sparksRFieldInfo = typeof(Fabricator).GetField("sparksR", BindingFlags.NonPublic | BindingFlags.Instance);

        private static ParticleSystemParameters originParticlesParams;
        private static ParticleSystemParameters modParticlesParams;
        private static Color? originLightColor;
        private static Color? modLightColor;

        public static void Modify(Fabricator fabricator)
        {
            if (BeamMaterial.custom) { SetFabricatorBeamsMaterial(fabricator, BeamMaterial.custom); }
            if (TryCreateModParticleParams(fabricator)) { SetFabricatorParticlesParams(fabricator, modParticlesParams); }
            if (TryCreateModLightColor(fabricator)) { SetFabricatorLightColor(fabricator, modLightColor.Value); }
        }

        public static void Revert(Fabricator fabricator)
        {
            if (BeamMaterial.original) { SetFabricatorBeamsMaterial(fabricator, BeamMaterial.original); }
            if (originParticlesParams != null) { SetFabricatorParticlesParams(fabricator, originParticlesParams); }
            if (originLightColor != null) { SetFabricatorLightColor(fabricator, originLightColor.Value); }
        }

        private static bool TryCreateModLightColor(Fabricator fabricator)
        {
            if (originLightColor != null) return true;
            try
            {
                originLightColor = fabricator.fabLight.color;
                modLightColor = Config.BeamColor;
                return true;
            }
            catch (Exception e)
            {
                Logging.Logger.Log(LogLevel.Error, e);
                return false;
            }
        }

        private static void SetFabricatorLightColor(Fabricator fabricator, Color color)
        {
            try
            {
                fabricator.fabLight.color = color;
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static void SetFabricatorBeamsMaterial(Fabricator fabricator, Material material)
        {
            try
            {
                fabricator.leftBeam.GetComponent<Renderer>().material = material;
                fabricator.rightBeam.GetComponent<Renderer>().material = material;
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static void SetFabricatorParticlesParams(Fabricator fabricator, ParticleSystemParameters @params)
        {
            try
            {
                var sparksL = ((GameObject)sparksLFieldInfo.GetValue(fabricator)).GetComponent<ParticleSystem>();
                var sparksR = ((GameObject)sparksRFieldInfo.GetValue(fabricator)).GetComponent<ParticleSystem>();
                @params.Apply(sparksL);
                @params.Apply(sparksR);
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static bool TryCreateModParticleParams(Fabricator fabricator)
        {
            if (modParticlesParams != null) return true;
            try
            {
                var sparksL = ((GameObject)sparksLFieldInfo.GetValue(fabricator)).GetComponent<ParticleSystem>();
                originParticlesParams = new ParticleSystemParameters()
                {
                    startColor = sparksL.main.startColor
                };
                modParticlesParams = new ParticleSystemParameters()
                {
                    startColor = Config.BeamAlphaColor
                };
                return true;
            }
            catch (Exception e)
            {
                Logging.Logger.Log(LogLevel.Error, e);
                return false;
            }
        }
    }
}  
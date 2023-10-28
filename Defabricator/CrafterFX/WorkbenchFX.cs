namespace Defabricator;

using System.Reflection;
using UnityEngine;
using System;
using BepInEx.Logging;
using Common;

partial class CrafterFX
{
    private static partial class WorkbenchFX
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

        private static readonly FieldInfo sparksField = typeof(Workbench).GetField("fxSparksInstances", BindingFlags.NonPublic | BindingFlags.Instance);

        private static ParticleSystemParameters originParticlesParams;
        private static ParticleSystemParameters modParticlesParams;
        private static Color? originLightColor;
        private static Color? modLightColor;

        public static void Modify(Workbench workbench)
        {
            if (BeamMaterial.custom) { SetWorkbenchBeamsMaterial(workbench, BeamMaterial.custom); }
            if (TryCreateModParticleParams(workbench)) { SetWorkbenchParticlesParams(workbench, modParticlesParams); }
            if (TryCreateModLightColor(workbench)) { SetWorkbenchLightColor(workbench, modLightColor.Value); }
        }

        public static void Revert(Workbench workbench)
        {
            if (BeamMaterial.original) { SetWorkbenchBeamsMaterial(workbench, BeamMaterial.original); }
            if (originParticlesParams != null) { SetWorkbenchParticlesParams(workbench, originParticlesParams); }
            if (originLightColor != null) { SetWorkbenchLightColor(workbench, originLightColor.Value); }
        }

        private static bool TryCreateModLightColor(Workbench workbench)
        {
            if (originLightColor != null) return true;
            try
            {
                originLightColor = workbench.workingLight.GetComponent<Light>().color;
                modLightColor = Config.BeamColor;
                return true;
            }
            catch (Exception e)
            {
                Logging.Logger.Log(LogLevel.Error, e);
                return false;
            }
        }

        private static void SetWorkbenchLightColor(Workbench workbench, Color color)
        {
            try
            {
                workbench.workingLight.GetComponent<Light>().color = color;
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static void SetWorkbenchBeamsMaterial(Workbench workbench, Material material)
        {
            try
            {
                foreach (var beam in workbench.fxLaserBeam)
                    beam.GetComponent<Renderer>().material = material;
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static void SetWorkbenchParticlesParams(Workbench workbench, ParticleSystemParameters @params)
        {
            try
            {
                var sparks = (GameObject[])sparksField.GetValue(workbench);
                foreach (var spark in sparks)
                    @params.Apply(spark.GetComponent<ParticleSystem>());
            }
            catch (Exception e) { Logging.Logger.Log(LogLevel.Error, e); }
        }

        private static bool TryCreateModParticleParams(Workbench workbench)
        {
            if (modParticlesParams != null) return true;
            try
            {
                var sparks = ((GameObject[])sparksField.GetValue(workbench))[0].GetComponent<ParticleSystem>();
                originParticlesParams = new ParticleSystemParameters()
                {
                    startColor = sparks.main.startColor
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
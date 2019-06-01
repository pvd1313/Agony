using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using Harmony;
using System;

using AGLogger = Agony.Common.Logger;

namespace Agony.WorkbenchFix
{
    internal static class AnimationFix
    {
        [HarmonyPatch(typeof(Workbench), "LateUpdate")]
        private static class LateUpdatePatch
        {
            private static readonly FieldInfo sparksField = typeof(Workbench).GetField("fxSparksInstances", BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly FieldInfo spawnAnimationDelayField = typeof(GhostCrafter).GetField("spawnAnimationDelay", BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly Action<Workbench> baseLateUpdate;

            static LateUpdatePatch()
            {
                var lateUpdate = typeof(GhostCrafter).GetMethod("LateUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
                var dyn = new DynamicMethod("", null, new Type[] { typeof(Workbench) }, typeof(Workbench));
                var gen = dyn.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call, lateUpdate);
                gen.Emit(OpCodes.Ret);

                baseLateUpdate = (Action<Workbench>)dyn.CreateDelegate(typeof(Action<Workbench>));
            }

            private static bool Prefix(Workbench __instance)
            {
                var workbench = __instance;
                baseLateUpdate(workbench);

                var data = workbench.GetData();
                data.beamTimer += Time.deltaTime;
                var animationDelay = (float)spawnAnimationDelayField.GetValue(workbench);
                
                if (workbench.logic)
                {
                    var targetState = workbench.logic.inProgress && (data.beamTimer > animationDelay);
                    if (targetState != data.animatingBeams) { ToggleBeamAnimation(workbench, targetState); }
                }
                if (data.animatingBeams){ UpdateAnimation(workbench); }

                return false;
            }

            private static void ToggleBeamAnimation(Workbench workbench, bool active)
            {
                workbench.GetData().animatingBeams = active;
                try
                {
                    workbench.workingLight.SetActive(active);
                    workbench.animator.SetBool(AnimatorHashID.working, active);
                    var sparks = (GameObject[])sparksField.GetValue(workbench);
                    var beams = workbench.fxLaserBeam;
                    for(var i = 0; i < beams.Length; i++)
                    {
                        beams[i].SetActive(active);
                        var ps = sparks[i].GetComponent<ParticleSystem>();
                        if (active) ps.Play();
                        else ps.Stop();
                    }
                }
                catch (Exception e) { AGLogger.ExceptionOnce(e); }
            }

            private static void UpdateAnimation(Workbench workbench)
            {
                try
                {
                    var light = workbench.workingLight.GetComponent<Light>();
                    light.intensity *= 2;
                    var sparks = (GameObject[])sparksField.GetValue(workbench);
                    var beams = workbench.fxLaserBeam;
                    var spawnPoint = workbench.ghost.itemSpawnPoint;
                    for (var i = 0; i < sparks.Length; i++)
                    {
                        var laser = beams[i].transform;
                        sparks[i].transform.position = GetBeamEnd(laser.position, laser.forward, spawnPoint.position, spawnPoint.up);
                    }
                }
                catch (Exception e) { AGLogger.ExceptionOnce(e); }
            }

            private static Vector3 GetBeamEnd(Vector3 beamPos, Vector3 beamRot, Vector3 basePos, Vector3 baseRot)
            {
                return beamPos + Vector3.Normalize(beamRot) * (Vector3.Dot(basePos - beamPos, baseRot) / Vector3.Dot(beamRot, baseRot));
            }
        }

        [HarmonyPatch(typeof(Workbench), "OnStateChanged")]
        private static class OnStateChangedPatch
        {
            private static bool Prefix(Workbench __instance, bool crafting)
            {
                var workbench = __instance;
                var data = workbench.GetData();

                data.beamTimer = 0;
                var fabSound = workbench.fabricateSound;
                if (fabSound.IsPlaying() != crafting)
                {
                    if (crafting) { fabSound.Play(); }
                    else { fabSound.Stop(); }
                }

                return false;
            }
        }
    }
}
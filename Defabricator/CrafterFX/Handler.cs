namespace Agony.Defabricator;

using System.Collections.Generic;
using System.Reflection;
using Agony.Common;
using HarmonyLib;

[HarmonyPatch]
internal static partial class CrafterFX
{
    [HarmonyPatch]
    public static class Handler
    {
        [HarmonyPatch(typeof(Crafter), nameof(Crafter.OnCraftingBegin))]
        public static class CrafterCraftPatch
        {
            private static void Prefix(Crafter __instance)
            {
                if (!Main.IsCurrentCrafter(__instance)) return;

                if (!modifiedCrafters.Contains(__instance))
                {
                    modifiedCrafters.Add(__instance);
                    if (__instance is Fabricator) { FabricatorFX.Modify(__instance as Fabricator); }
                    else if (__instance is Workbench) { WorkbenchFX.Modify(__instance as Workbench); }
                }
            }
        }

        [HarmonyPatch(typeof(Crafter), nameof(Crafter.CrafterOnDone))]
        public static class CrafterOnDone
        {
            private static void Postfix(Crafter __instance)
            {
                if (modifiedCrafters.Contains(__instance))
                {
                    modifiedCrafters.Remove(__instance);
                    if (__instance is Fabricator) { FabricatorFX.Revert(__instance as Fabricator); }
                    else if (__instance is Workbench) { WorkbenchFX.Revert(__instance as Workbench); }
                }
            }
        }

        private static readonly HashSet<Crafter> modifiedCrafters = new();

        public static bool IsModified(Crafter crafter) => modifiedCrafters.Contains(crafter);
    }
}
/*using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace Agony.Core
{
    public static class AGTechData
    {
        [HarmonyPatch(typeof(KnownTech), "Initialize")]
        private static class KnownTechPatch
        {
            private static void Prefix()
            {
                var pdaDatas = Resources.FindObjectsOfTypeAll<PDAData>();

                foreach (var pda in pdaDatas)
                {
                    pda.defaultTech.Clear();
                    pda.analysisTech.Clear();
                    Analizes.ForEach(t => t.AddToPDA(pda));
                }
            }
        }

        private struct TechAnalisys
        {
            public TechType TechToUnlock;
            public TechType AnalizedTech;

            public void AddToPDA(PDAData pda)
            {
                var analizedTech = AnalizedTech;
                var analisys = pda.analysisTech.Find(x => x.techType == analizedTech);
                if (analisys == null)
                {
                    analisys = new KnownTech.AnalysisTech()
                    {
                        techType = analizedTech,
                        unlockTechTypes = new List<TechType>(),
                    };
                    pda.analysisTech.Add(analisys);
                }
                analisys.unlockTechTypes.Add(TechToUnlock);
            }
        }

        private static readonly List<TechAnalisys> Analizes = new List<TechAnalisys>();

        public static void AddAnalisys(TechType techToUnlock, params TechType[] analizedTech)
        {
            foreach(var aTech in analizedTech)
            {
                Analizes.Add(new TechAnalisys()
                {
                    TechToUnlock = techToUnlock,
                    AnalizedTech = aTech
                });
            }
        }
    }
}*/
using System.Collections.Generic;

namespace Agony.WorkbenchFix
{
    internal static class Extensions
    {
        internal sealed class Data
        {
            public bool animatingBeams;
            public float beamTimer;
        }

        private static readonly Dictionary<Workbench, Data> storage = new Dictionary<Workbench, Data>();

        public static Data GetData(this Workbench workbench)
        {
            Data data;
            if (storage.TryGetValue(workbench, out data)) { return data; }
            data = new Data();
            storage[workbench] = data;
            return data;
        }
    }

}
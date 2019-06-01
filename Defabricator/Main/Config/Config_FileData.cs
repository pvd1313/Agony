using System.Collections.Generic;
using UnityEngine;

namespace Agony.Defabricator
{
    partial class Main
    {
        partial class Config
        {
            private sealed class FileData
            {
                public List<string> Blacklist { get; private set; } = new List<string>();
                public Dictionary<string, double> Yields { get; private set; } = new Dictionary<string, double>();

                public void Apply()
                {
                    Validate();
                    Blacklist.ForEach(x => blacklist.Add(x));
                    Yields.ForEach(x => yields[x.Key] = x.Value);                   
                }

                private void Validate()
                {
                    Blacklist.RemoveAll(x => x == null);
                    var keys = new List<string>(Yields.Keys);
                    keys.ForEach(x => Yields[x] = Mathf.Clamp01((float)Yields[x]));
                }
            }
        }
    }
}

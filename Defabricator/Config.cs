namespace Agony.Defabricator
{
    using System.Collections.Generic;
    using Agony.Common;
    using System.IO;
    using System;
    using System.Reflection;
    using UnityEngine;

    public class Config
    {
        private sealed class AllItems
        {
            public Dictionary<string, string> Items { get; private set; } = new Dictionary<string, string>();

            public AllItems()
            {
                var lang = Language.main;
                if(lang == null)
                    return;

                var techs = Enum.GetValues(typeof(TechType));
                foreach(var tech in techs)
                {
                    var techID = tech.ToString();
                    var text = lang.Get(techID);
                    var tooltip = lang.Get("Tooltip_" + techID);
                    if(text == "" || tooltip == "")
                        continue;
                    Items[techID] = text;
                }
            }
        }

        private const string fileDataDirectory = "Items/";
        private const string fileDataSearchPattern = "*.json";
        private const string allItemsFile = "Items.txt";

        internal static HashSet<string> blacklist = new HashSet<string>();
        internal static Dictionary<string, double> yields = new Dictionary<string, double>();

        static Config()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, fileDataDirectory);
            Directory.CreateDirectory(path);
            var files = Directory.GetFiles(path, fileDataSearchPattern);

            foreach(var file in files)
            {
                FileData data;
                if(JsonUtil.TryFileToObject(file, out data))
                { data.Apply(); }
            }

            path = Path.Combine(path, allItemsFile);
            JsonUtil.TryObjectToFile(new AllItems(), path);
        }

        public static bool IsBlacklisted(TechType techType) => blacklist.Contains(techType.AsString());

        public static float GetYield(TechType techType)
        {
            double yeild;
            if(yields.TryGetValue(techType.AsString(), out yeild))
            {
                return (float)yeild;
            }
            return 1;
        }
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
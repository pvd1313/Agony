using System.Collections.Generic;
using Agony.Common;
using System.IO;
using System;
using System.Reflection;

namespace Agony.Defabricator
{
    partial class Main
    {
        private partial class Config
        {
            private sealed class AllItems
            {
                public Dictionary<string, string> Items { get; private set; } = new Dictionary<string, string>();

                public AllItems()
                {
                    var lang = Language.main;
                    if (lang == null) return;

                    var techs = (TechType[]) Enum.GetValues(typeof(TechType));
                    foreach(var tech in techs)
                    {
                        var techID = tech.AsString();
                        var text = lang.Get(techID);
                        var tooltip = lang.Get("Tooltip_" + techID);
                        if (text == "" || tooltip == "") continue;
                        Items[techID] = text;
                    }
                }
            }

            private const string fileDataDirectory = "Items/";
            private const string fileDataSearchPattern = "*.json";
            private const string allItemsFile = "Items.txt";

            private static HashSet<string> blacklist = new HashSet<string>();
            private static Dictionary<string, double> yields = new Dictionary<string, double>();
            
            static Config()
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                path = Path.Combine(path, fileDataDirectory);
                Directory.CreateDirectory(path);
                var files = Directory.GetFiles(path, fileDataSearchPattern);

                foreach(var file in files)
                {
                    FileData data;
                    if (JsonUtil.TryFileToObject(file, out data)) { data.Apply(); }
                }

                path = Path.Combine(path, allItemsFile);
                JsonUtil.TryObjectToFile(new AllItems(), path);
            }

            public static bool IsBlacklisted(TechType techType) => blacklist.Contains(techType.AsString());

            public static float GetYield(TechType techType)
            {
                double yeild;
                if (yields.TryGetValue(techType.AsString(), out yeild))
                {
                    return (float)yeild;
                }
                return 1;
            }
        }
    }
}
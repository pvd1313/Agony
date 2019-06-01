using System.Collections.Generic;
using Agony.AssetTools.Wrappers;
using Agony.Common;
using System.Linq;
using System;

namespace Agony.LanguagePatcher
{
    partial class LanguagePatcher
    {
        private sealed class PatchData
        {
            public const string ModTag = "<color=#00FF00FF>[MOD]</color> ";

            private static readonly HashSet<string> languages = new HashSet<string>(LanguageWrapper.Languages);
            private static readonly HashSet<string> assemblies = new HashSet<string>(from x in AppDomain.CurrentDomain.GetAssemblies() select x.GetName().Name);

            public string TargetLanguage { get; set; }
            public string TargetAssembly { get; set; }
            public string TargetAssemblyWeb { get; set; }
            public string Author { get; set; }
            public string AuthorWeb { get; set; }
            public string AuthorNotes { get; set; }
            public string Path { get; set; }
            public bool Enable { get; set; } = true;
            public Dictionary<string, string> Strings { get; set; } = new Dictionary<string, string>();

            public bool TryApply()
            {
                if (!Enable) return false;
                if (string.IsNullOrEmpty(TargetLanguage) || !languages.Contains(TargetLanguage))
                {
                    Logger.Error($"Invalid language '{TargetLanguage}' in '{Path}'.");
                    return false;
                }
                if (string.IsNullOrEmpty(TargetAssembly) || assemblies.Contains(TargetAssembly))
                {
                    SetStrings();
                    return true;
                }
                return false;
            }

            private void SetStrings()
            {
                if (Strings == null) return;
                foreach (var pair in Strings)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        Logger.Warning($"Invalid text at line '{pair.Key}' in {Path}.");
                        continue;
                    }
                    var text = Config.Debug ? (ModTag + pair.Value) : pair.Value;
                    LanguageWrapper.Set(TargetLanguage, pair.Key, text);
                }
            }
        }
    }
}
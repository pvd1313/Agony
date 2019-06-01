using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Agony.AssetTools.Wrappers;
using UnityEngine;
using System;

using AGLogger = Agony.Common.Logger;

namespace Agony.LanguagePatcher
{
    partial class LanguagePatcher
    {
        private sealed class Commands : MonoBehaviour
        {
            private static Commands main;
            private static readonly Regex onlyASCIIRegex = new Regex(@"^[ -~]+$");
            //private static readonly Regex containsEnglishWordRegex = new Regex(@"[a-zA-Z]{2,}");

            static Commands()
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            public static void Patch() { }

            private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
            {
                if (main == null)
                {
                    var gameObject = new GameObject(nameof(LanguagePatcher) + (nameof(Commands)));
                    main = gameObject.AddComponent<Commands>();

                    DevConsole.RegisterConsoleCommand(main, "lgfind", false, false);
                    DevConsole.RegisterConsoleCommand(main, "lgpatch", false, false);
                }
            }

            public void OnConsoleCommand_lgpatch(NotificationCenter.Notification note)
            {
                LanguageWrapper.Clear();
                LanguagePatcher.Patch();
                ErrorMessage.AddMessage("Language patches reloaded.");
            }

            public void OnConsoleCommand_lgfind(NotificationCenter.Notification note)
            {
                if (note == null || note.data == null || note.data.Count == 0) return;
                var pattern = (string)note.data[0];
                if (string.IsNullOrEmpty(pattern)) return;
                pattern = pattern.Replace('_', ' ');
                int maxTextSize = 128;
                if (note.data.Count > 1) { int.TryParse((string)note.data[1], out maxTextSize); }

                ErrorMessage.AddMessage($"Searching for '{pattern}'...");
                var regex = pattern == "-eng" ? onlyASCIIRegex : new Regex($"{Regex.Escape(pattern)}", RegexOptions.IgnoreCase);
                var searchResult = LanguageWrapper.FindAll(x => regex.IsMatch(x.Value));

                if (searchResult.Count > 0)
                {
                    ErrorMessage.AddMessage($"Found <color=#00FF00FF>{searchResult.Count}</color> strings. Check log for details.");
                    AGLogger.Message($"Searched '{pattern}'. Found {searchResult.Count} strings:");
                    ReportSearch(searchResult, maxTextSize);
                }
                else
                {
                    ErrorMessage.AddMessage($"Nothing found.");
                    AGLogger.Message($"Searched '{pattern}'. Nothing found.");
                }         
            }

            private static void ReportSearch(List<KeyValuePair<string, string>> searchResult, int maxTextSize)
            {
                var maxKeySize = 0;
                searchResult.ForEach(x => maxKeySize = (x.Key.Length > maxKeySize) ? (x.Key.Length) : (maxKeySize));

                var usedKeys = new HashSet<string>();
                foreach(var pair in searchResult)
                {
                    if (usedKeys.Contains(pair.Key)) continue;
                    usedKeys.Add(pair.Key);
                    var text = FormatText(pair.Value, maxTextSize);
                    Console.WriteLine("  \"{0,-" + (maxKeySize + 1) + "} : \"{1}\",", pair.Key + '"', text);
                }
            }

            private static string FormatText(string text, int maxTextSize)
            {
                var modded = text.StartsWith(PatchData.ModTag);
                if (maxTextSize > 0)
                {
                    if (modded) { maxTextSize += PatchData.ModTag.Length; }
                    if (text.Length > maxTextSize) { text = text.Substring(0, maxTextSize) + "..."; }
                }
                if (modded) { text = "[MOD]" + text.Substring(PatchData.ModTag.Length, text.Length - PatchData.ModTag.Length); }
                return text.Replace("\n", string.Empty);
            }
        }
    }
}
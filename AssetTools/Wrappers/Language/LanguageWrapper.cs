using System.Collections.Generic;
using Agony.Common.Reflection;
using Agony.Common;
using UnityEngine;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static partial class LanguageWrapper
    {
        private sealed class LanguageDummy : Language
        {
            private void Update() { }
            private void Awake() { }
            private void OnDestroy() { }
        }

        private const string UnknownLanguage = "_unknown";

        public static string[] Languages => languages.ToArray();       
        private static readonly List<string> languages;
        private static readonly Dictionary<string, Dictionary<string, string>> majorVocabulary = new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, string> minorVocabulary = new Dictionary<string, string>();
        private static string CurrentLanguage
        {
            get
            {
                if (Language.main == null) { return UnknownLanguage; }
                return Language.main.GetCurrentLanguage();
            }
        }

        static LanguageWrapper()
        {
            var gameObject = new GameObject();
            var dummy = gameObject.AddComponent<LanguageDummy>();
            languages = new List<string>(dummy.GetLanguages());
            GameObject.Destroy(gameObject);

            foreach(var lang in languages) { majorVocabulary[lang] = new Dictionary<string, string>(); }
            majorVocabulary[UnknownLanguage] = new Dictionary<string, string>();
        }

        public static void Set(string language, string key, string text)
        {
            if (string.IsNullOrEmpty(language)) throw new ArgumentException("language is null or empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key is null or empty");
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text is null or empty");
            if (!majorVocabulary.ContainsKey(language)) throw new ArgumentException("language is invalid");

            majorVocabulary[language][key] = text;
            LanguageCache.OnLanguageChanged();
        }

        public static void SetDefault(string key, string text)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key is null or empty");
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text is null or empty");

            minorVocabulary[key] = text;
        }

        public static void Clear()
        {
            majorVocabulary.ForEach(x => x.Value.Clear());
            LanguageCache.OnLanguageChanged();
        }

        public static List<KeyValuePair<string, string>> FindAll(Predicate<KeyValuePair<string, string>> match)
        {
            if (match == null) throw new ArgumentNullException("match is null");

            var result = new List<KeyValuePair<string, string>>();
            result.AddRange(majorVocabulary[CurrentLanguage].FindAll(match));
            if (Language.main != null) { result.AddRange(LanguageReflector.GetStrings(Language.main).FindAll(match)); }
            result.AddRange(minorVocabulary.FindAll(match));
            return result;
        }
    }
}
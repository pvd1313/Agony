using System.Collections.Generic;
using System.Reflection;
using Agony.Common;
using System;

namespace Agony.AssetTools.Wrappers
{
    public static class TechTypeExtensionsWrapper
    {
        private const BindingFlags PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static;

        private static readonly Type extType = typeof(TechTypeExtensions);
        private static readonly Dictionary<TechType, string> stringsNormal = (Dictionary<TechType, string>)extType.GetField("stringsNormal", PrivateStatic).GetValue(null);
        private static readonly Dictionary<TechType, string> stringsLowercase = (Dictionary<TechType, string>)extType.GetField("stringsLowercase", PrivateStatic).GetValue(null);
        private static readonly Dictionary<string, TechType> techTypesNormal = (Dictionary<string, TechType>)extType.GetField("techTypesNormal", PrivateStatic).GetValue(null);
        private static readonly Dictionary<string, TechType> techTypesIgnoreCase = (Dictionary<string, TechType>)extType.GetField("techTypesIgnoreCase", PrivateStatic).GetValue(null);
        private static readonly Dictionary<TechType, string> techTypeKeys = (Dictionary<TechType, string>)extType.GetField("techTypeKeys", PrivateStatic).GetValue(null);
        private static readonly Dictionary<string, TechType> keyTechTypes = (Dictionary<string, TechType>)extType.GetField("keyTechTypes", PrivateStatic).GetValue(null);

        public static void Link(TechType techType, string key)
        {
            if (techType == TechType.None) throw new ArgumentException("techType == None");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key is null or empty");
            if (stringsNormal.ContainsKey(techType) || stringsLowercase.ContainsKey(techType) || techTypeKeys.ContainsKey(techType))
            {
                Logger.Exception(new ArgumentException($"techType '{techType}' already has a key."));
            }
            var keyLower = key.ToLower();
            var intKey = ((int)techType).ToString();
            if (techTypesNormal.ContainsKey(key) || techTypesIgnoreCase.ContainsKey(keyLower) || keyTechTypes.ContainsKey(intKey))
            {
                Logger.Exception(new ArgumentException($"key '{key}' already has a TechType."));
            }

            techTypesNormal[key] = techType;
            techTypesIgnoreCase[keyLower] = techType;
            keyTechTypes[intKey] = techType;

            stringsNormal[techType] = key;
            stringsLowercase[techType] = keyLower;
            techTypeKeys[techType] = intKey;
        }
    }
}
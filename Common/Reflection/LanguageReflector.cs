using System.Collections.Generic;
using System.Reflection;
using System;

namespace Agony.Common.Reflection
{
    public static class LanguageReflector
    {
        private static readonly FieldInfo stringsFieldInfo = typeof(Language).GetField("strings", BindingFlags.NonPublic | BindingFlags.Instance);

        public static Dictionary<string, string> GetStrings(Language language)
        {
            if (language == null) throw new ArgumentNullException("language is null");
            return (Dictionary<string, string>)stringsFieldInfo.GetValue(language);
        }
    }
}
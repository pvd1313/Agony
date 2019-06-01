using System.Collections.Generic;
using System;

namespace Agony.Common
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> FindAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> match)
        {
            var result = new Dictionary<TKey, TValue>();
            dictionary.ForEach(x =>
            {
                if (match(x)) { result.Add(x.Key, x.Value); }
            });
            return result;
        }
    }
}
using System;

namespace Agony.Common
{
    public static class StringUtil
    {
        public static string FormatWithFallback(string unmanaged, string fallback, params object[] args)
        {
            if (fallback == null) throw new ArgumentNullException("fallback is null");

            try
            {
                return string.Format(unmanaged, args);
            }
            catch (FormatException) { }
            return string.Format(fallback, args);
        }
    }
}
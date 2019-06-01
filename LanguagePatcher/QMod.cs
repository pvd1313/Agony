using Agony.Common;

namespace Agony.LanguagePatcher
{
    internal static class QMod
    {
        public static void Patch()
        {
            LanguagePatcher.Patch();
        }
    }
}
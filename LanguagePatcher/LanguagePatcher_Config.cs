using Agony.Common;

namespace Agony.LanguagePatcher
{
    partial class LanguagePatcher
    {
        private sealed class Config
        {
            public static bool Debug { get; private set; } = true;

            static Config()
            {
                var config = new Config();
                ConfigUtil.Read(config);
                ConfigUtil.Write(config);
            }
        }
    }
}
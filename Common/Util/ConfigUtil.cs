using System.IO;

namespace Agony.Common
{
    public class ConfigUtil
    {
        public static void Read<T>(T config)
        {
            var file = GetConfigPath(config);
            JsonUtil.TryFileToObject(file, out config);
        }

        public static void Write<T>(T config)
        {
            var file = GetConfigPath(config);
            JsonUtil.TryObjectToFile(config, file);
        }

        private static string GetConfigPath<T>(T config)
        {
            var type = config.GetType();
            var path = PathUtil.GetAssemblyPath(type);
            return Path.Combine(path, $@"Config/{type.FullName}.json");
        }
    }
}
namespace Common;

using System.IO;
using System.Reflection;

public class ConfigUtil
{
    public static void Read<T>(ref T config)
    {
        var file = GetConfigPath(config);
        if(File.Exists(file))
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
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return Path.Combine(path, $@"Config\{type.FullName}.json");
    }
}
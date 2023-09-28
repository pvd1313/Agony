namespace Common;

using System.IO;
using System;
using BepInEx.Logging;
using Newtonsoft.Json;

public static class JsonUtil
{
    private static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

    public static bool TryFileToObject<T>(string fileName, out T obj)
    {
        obj = default;
        try
        {
            using (var file = new StreamReader(fileName))
            {
                obj = JsonConvert.DeserializeObject<T>(file.ReadToEnd(), SerializerSettings);
                return true;
            }
        }
        catch(Exception e)
        {
            Logging.Logger.Log(LogLevel.Error, $"File:'{fileName}'.\n"+ e);
            return false;
        }
    }

    public static bool TryObjectToFile<T>(T obj, string fileName)
    {
        try
        {
            var directory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(directory);
            using (var file = new StreamWriter(fileName))
            {
                file.Write(JsonConvert.SerializeObject(obj, SerializerSettings));
                return true;
            }
        }
        catch (Exception e)
        {
            Logging.Logger.Log(LogLevel.Error, $"File:'{fileName}'.\n"+ e);
            return false;
        }
    }
}
using System.IO;
using LitJson;
using System;

namespace Agony.Common
{
    public static class JsonUtil
    {
        public static bool TryFileToObject<T>(string fileName, out T obj)
        {
            obj = default(T);
            try
            {
                using (var file = new StreamReader(fileName))
                {
                    obj = JsonMapper.ToObject<T>(file);
                    return true;
                }
            }
            catch(Exception e)
            {
                Logger.Exception(e, $"File:'{fileName}'.");
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
                    var writer = new JsonWriter(file);                  
                    writer.PrettyPrint = true;
                    JsonMapper.ToJson(obj, writer);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e, $"File:'{fileName}'.");
                return false;
            }
        }
    }
}
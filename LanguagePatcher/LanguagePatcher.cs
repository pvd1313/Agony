using System.Collections.Generic;
using Agony.Common;
using System.IO;

namespace Agony.LanguagePatcher
{
    internal static partial class LanguagePatcher
    {
        private const string MainDirectory = "Languages/";
        private const string SearchPattern = "*.json";

        public static void Patch()
        {
            var filePaths = GetFilePaths();
            var patches = ReadPatches(filePaths);

            var patchCount = 0;
            patches.ForEach(x => patchCount += x.TryApply() ? 1 : 0);
            if (patchCount > 0) Logger.Message($"Applied {patchCount} patches.");

            Commands.Patch();
        }

        private static string[] GetFilePaths()
        {
            var path = PathUtil.GetAssemblyPath(typeof(LanguagePatcher));
            path = Path.Combine(path, MainDirectory);
            Directory.CreateDirectory(path);
            return Directory.GetFiles(path, SearchPattern);
        }

        private static List<PatchData> ReadPatches(string[] paths)
        {
            var langList = new List<PatchData>();
            foreach(var filePath in paths)
            {
                PatchData patch;
                if (JsonUtil.TryFileToObject(filePath, out patch))
                {
                    patch.Path = filePath;
                    langList.Add(patch);
                }
                else
                {
                    Logger.Error($"Failed to parse file '{filePath}'.");
                }
            }
            return langList;
        }
    }
}
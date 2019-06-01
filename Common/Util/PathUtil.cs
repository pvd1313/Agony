using System.Reflection;
using System.IO;
using System;

namespace Agony.Common
{
    public static class PathUtil
    {
        public static string GetAssemblyPath(Type type)
        {
            var location = Assembly.GetAssembly(type).Location;
            return Path.GetDirectoryName(location);
        }
    }
}
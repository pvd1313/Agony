using AGLogger = Agony.Common.Logger;
using Agony.Common;
using UnityEngine;
using System.IO;

namespace Agony.Tests
{
    public static class AGTestResourceLoader
    {
        private static AssetBundle Assets;

        private static Sprite Default;

        static AGTestResourceLoader()
        {
            var defaultTexture = SpriteManager.defaultSprite.texture;
            Default = Sprite.Create(defaultTexture, new Rect(0, 0, defaultTexture.width, defaultTexture.height), new Vector2(.5f, .5f));
        }

        public static void Load()
        {
            AGLogger.Message("Loading resources...");
            var path = PathUtil.GetAssemblyPath(typeof(AGTestResourceLoader));
            Assets = AssetBundle.LoadFromFile(Path.Combine(path, "resources.assets"));
            if (Assets == null) AGLogger.Error("Unable to load resources.");
        }

        public static Sprite LoadSprite(string name)
        {
            if (Assets == null) return Default;
            var sprite = Assets.LoadAsset<Sprite>(name);
            if (sprite == null)
            {
                AGLogger.Error($"Unable to load sprite '{name}'.");
                return Default;
            }
            return sprite;
        }
    }
}
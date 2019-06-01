using UnityEngine;
using System;

namespace Agony.Common
{
    public static class TextureUtil
    {
        public static Texture2D Blit(Texture texture)
        {
            if (!texture) throw new ArgumentNullException("texture is null or destroyed");

            var tempRender = RenderTexture.GetTemporary(texture.width, texture.height);
            Graphics.Blit(texture, tempRender);

            var gameRender = RenderTexture.active;
            RenderTexture.active = tempRender;
            var result = new Texture2D(texture.width, texture.height);
            result.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            result.Apply();

            RenderTexture.active = gameRender;
            RenderTexture.ReleaseTemporary(tempRender);
            return result;
        }
    }
}
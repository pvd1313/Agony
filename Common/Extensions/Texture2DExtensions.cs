using System;
using UnityEngine;

namespace Agony.Common
{
    public static class Texture2DExtensions
    {
        public static void ConvertPixels(this Texture2D texture, Converter<Color, Color> converter)
        {
            var pixels = texture.GetPixels();
            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = converter(pixels[i]);
            texture.SetPixels(pixels);
        }
    }
}
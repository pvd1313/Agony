namespace Common.Animation;

using UnityEngine;
using System;

public static class AnimationFuncs
{
    public static Func<Color, Color> SinusoidalColor(Color min, Color max, float frequency)
    {
        return new Func<Color, Color>(x =>
        {
            var lerp = MathUtil.Sin01(Time.time * frequency);
            return Color.Lerp(min, max, lerp);
        });
    }
}
namespace Common;

using UnityEngine;

public static class MathUtil
{
    public static float Sin01(float x)
    {
        return (Mathf.Sin(x) + 1) / 2;
    }
}
using UnityEngine;

namespace Agony.Common
{
    public static class MathUtil
    {
        public static float Sin01(float x)
        {
            return (Mathf.Sin(x) + 1) / 2;
        }
    }
}
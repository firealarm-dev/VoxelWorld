using UnityEngine;

namespace Terrain.Extensions
{
    internal static class NumberExtensions
    {
        public static float Map(this float value, float destMin, float destMax, float sourceMin, float sourceMax)
        {
            return Mathf.Lerp(destMin, destMax, Mathf.InverseLerp(sourceMin, sourceMax, value));
        }
    }
}

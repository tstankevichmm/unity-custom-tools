using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class FloatExtensions
    {
        public static bool FloatCompare(this float valueA, float valueB, float tolerance = 0.001f)
        {
            return Mathf.Abs(valueA - valueB) < tolerance;
        }
    }
}
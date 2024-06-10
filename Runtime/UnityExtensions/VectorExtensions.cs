using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class VectorExtensions
    {
        public static Vector3 DirectionTo(this Vector3 current, Vector3 target)
        {
            return target - current;
        }

        public static Vector3 DirectionToNormalized(this Vector3 current, Vector3 target)
        {
            return DirectionTo(current, target).normalized;
        }

        public static float DistanceAbs(this Vector3 original, Vector3 target)
        {
            return Mathf.Abs(Vector3.Distance(original, target));
        }

        public static Vector3 Set(this Vector3 origVector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? origVector.x, y ?? origVector.y, z ?? origVector.z);
        }

        public static Vector3 Set(this Vector3 origVector, Vector3 newVector, bool ignoreX = false, bool ignoreY = false, bool ignoreZ = false)
        {
            return new Vector3(
                ignoreX ? origVector.x : newVector.x,
                ignoreY ? origVector.y : newVector.y,
                ignoreZ ? origVector.z : newVector.z
            );
        }
        
        public static Vector3 GetOffset(Vector3 forward, Vector3 up, Vector3 right, Vector3 offset)
        {
            return (right * offset.x) + (up * offset.y) + (forward * offset.z);
        }
    }
}
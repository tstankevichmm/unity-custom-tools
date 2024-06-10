using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class QuaternionExtensions
    {
        public static Vector3 GetOffset(this Quaternion rotation, Vector3 offset)
        {
            Vector3 forward = rotation * Vector3.forward;
            Vector3 up = rotation * Vector3.up;
            Vector3 right = rotation * Vector3.right;
            
            return VectorExtensions.GetOffset(forward, up, right, offset);
        }
    }
}
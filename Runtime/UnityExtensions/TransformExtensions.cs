using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class TransformExtensions
    {
        public static Transform FindChildByName(this Transform transform, string name)
        {
            if (transform.name == name)
                return transform;

            foreach (Transform child in transform)
            {
                var returnTrans = child.FindChildByName(name);

                if (returnTrans)
                    return returnTrans;
            }

            return null;
        }

        public static Vector3 DirectionTo(this Transform current, Vector3 target)
        {
            return current.position.DirectionTo(target);
        }

        public static Vector3 DirectionTo(this Transform current, Transform target)
        {
            return current.DirectionTo(target.position);
        }

        public static float DistanceAbs(this Transform current, Vector3 target)
        {
            return current.position.DistanceAbs(target);
        }
        
        public static float DistanceToAbs(this Transform current, Transform target)
        {
            return current.DistanceAbs(target.position);
        }
        
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static Vector3 GetOffset(this Transform transform, Vector3 offset)
        {
            return VectorExtensions.GetOffset(transform.forward, transform.up, transform.right, offset);
        }

        // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
        public static Vector3 GetDirectionToMouse(this Transform transform, float maxDistance = 1000, bool normalized = true)
        {
            Vector3 mousePos = Input.mousePosition;

            Ray ray = Camera.main!.ScreenPointToRay(mousePos);
            Vector3 endLocation = transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                endLocation = hit.point;
            }

            if (normalized)
                return transform.position.DirectionToNormalized(endLocation);

            return transform.position.DirectionTo(endLocation);
        }
    }
}
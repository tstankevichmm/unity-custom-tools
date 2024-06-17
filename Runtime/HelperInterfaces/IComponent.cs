using UnityEngine;

namespace unity_custom_tools.Runtime.HelperInterfaces
{
    public interface IComponent
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public T GetComponent<T>();
    }
}
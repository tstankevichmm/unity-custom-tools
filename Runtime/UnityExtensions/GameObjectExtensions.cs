using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class GameObjectExtensions
    {
        public static T GetComponentInChildrenOrParent<T>(this GameObject gameObject, bool includeInactive = false)
        {
            T component = gameObject.GetComponentInChildren<T>(includeInactive);

            if (component != null)
                return component;
            
            component = gameObject.GetComponentInParent<T>(includeInactive);

            return component;
        }

        public static void SetActive(this GameObject[] gameObjectArray, bool isActive)
        {
            if (isActive)
            {
                gameObjectArray.Show();
            }
            else
            {
                gameObjectArray.Hide();
            }
        }

        public static void Hide(this GameObject[] gameObjectArray)
        {
            foreach (GameObject gameObject in gameObjectArray)
            {
                gameObject.SetActive(false);
            }
        }
        
        public static void Show(this GameObject[] gameObjectArray)
        {
            foreach (GameObject gameObject in gameObjectArray)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
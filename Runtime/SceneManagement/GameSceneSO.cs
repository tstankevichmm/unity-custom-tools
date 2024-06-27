using UnityEngine;

namespace CustomTools.SceneManagement
{ 
    [CreateAssetMenu(menuName = "Custom Tools/Scene Management/Game Scene SO")]
    public class GameSceneSO : ScriptableObject
    {
        [Header("Information")]
        public string sceneName;
    }
}
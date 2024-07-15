#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CustomTools.SceneManagement
{
    [CustomEditor(typeof(GameSceneSO))]
    public class GameSceneSOEditor : Editor
    {
        private const string NoSceneWarning = "There is no Scene associated to this location yet. " +
                                                 "Add a new scene with the dropdown below";

        private string[] _scenes;
        private GameSceneSO _gameSceneInspector;

        private void OnEnable()
        {
            _gameSceneInspector = target as GameSceneSO;
            PopulateScenePicker();
        }
        public override void OnInspectorGUI()
        {
            DrawScenePicker();
        }

        /// <summary>
        /// Creates a dropdown box to select a scene
        /// </summary>
        private void DrawScenePicker()
        {
            string sceneName = _gameSceneInspector.sceneName;
            EditorGUI.BeginChangeCheck();
            int selectedScene = _scenes.ToList().IndexOf(sceneName);

            if (selectedScene < 0)
            {
                EditorGUILayout.HelpBox(NoSceneWarning, MessageType.Warning);
            }

            selectedScene = EditorGUILayout.Popup("Scene", selectedScene, _scenes);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed selected scene");
                _gameSceneInspector.sceneName = _scenes[selectedScene];
                MarkAllDirty();
            }
        }


        /// <summary>
        /// Populates the Scene picker with Scenes included in the game's build index
        /// </summary>
        private void PopulateScenePicker()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            _scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                _scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
        }

        /// <summary>
        /// Marks scenes as dirty so data can be saved
        /// </summary>
        private void MarkAllDirty()
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}

#endif
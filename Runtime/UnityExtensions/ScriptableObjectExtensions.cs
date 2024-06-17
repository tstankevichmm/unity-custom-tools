using System;
using UnityEditor;
using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class ScriptableObjectExtensions
    {
        public static T LoadScriptableObject<T>(string soName) where T : ScriptableObject
        {
            string scriptableObjectName = soName;
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(T)} {scriptableObjectName}");

            if (guids.Length == 0)
                throw new Exception($"No {nameof(T)} found named {scriptableObjectName}");

            if (guids.Length > 0)
                Debug.LogWarning(
                    $"More than one {nameof(T)} found named {scriptableObjectName}, taking first one");

            return (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]),
                typeof(T));
        }
    }
}
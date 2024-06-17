using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class ScriptableObjectExtensions
    {
        public static T LoadScriptableObject<T>(string searchFilter) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets(searchFilter);
            
            if (guids.Length == 0)
                throw new Exception($"No {nameof(T)} found with filter: {searchFilter}");

            if (guids.Length > 0)
            {
                string allFound = guids.ToList().JoinToString();
                Debug.LogWarning(
                    $"More than one {nameof(T)} found with filter: {searchFilter}. \n{allFound}\n taking first one");
            }

            return (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]),
                typeof(T));
        }
    }
}
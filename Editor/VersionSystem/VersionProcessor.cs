using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CustomTools.VersionSystem
{
    public class VersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string _initialVersion = "0.0.0.100";
        
        public void OnPreprocessBuild(BuildReport report)
        {
            string currentVersion = FindCurrentVersion();
            
            if (!IsValidVersion(currentVersion))
            {
                throw new Exception($"Invalid version format: {currentVersion}");
            }
            
            int major = GetMajor(currentVersion);
            int minor = GetMinor(currentVersion);
            int patch = GetPatch(currentVersion);
            int internalBuild = GetInternal(currentVersion);
            
            string newVersion = $"{major}.{minor}.{patch}.{internalBuild + 1}";
            
            UpdateVersion(newVersion);
        }

        private string FindCurrentVersion()
        {
            string[] currentVersion = PlayerSettings.bundleVersion.Split(".");

            if (currentVersion.Length <= 1)
                return _initialVersion;
            
            return PlayerSettings.bundleVersion;
        }

        private void UpdateVersion(string version)
        {
            Debug.Log($"Updating Version To {version}");
            PlayerSettings.bundleVersion = version;
        }

        private bool IsValidVersion(string version)
        {
            string[] versionArray = version.Split(".");
            string[] initialArray = _initialVersion.Split(".");
            return versionArray.Length == initialArray.Length;
        }

        private int GetMajor(string version)
        {
            string[] versionArray = version.Split(".");
            return int.Parse(versionArray[0]);
        }

        private int GetPatch(string currentVersion)
        {
            string[] versionArray = currentVersion.Split(".");
            return int.Parse(versionArray[2]);
        }

        private int GetInternal(string currentVersion)
        {
            string[] versionArray = currentVersion.Split(".");
            return int.Parse(versionArray[3]);
        }

        private int GetMinor(string currentVersion)
        {
            string[] versionArray = currentVersion.Split(".");
            return int.Parse(versionArray[1]);
        }
    }
}
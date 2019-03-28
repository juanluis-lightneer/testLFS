//-----------------------------------------------------------------------------
//
// Lightneer Inc Confidential
//
//
// 2015 - 2019 (C) Lightneer Incorporated
// All Rights Reserved.
//
// NOTICE:  All information contained herein is, and remains
// the property of Lightneer Incorporated and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to Lightneer Incorporated
// and its suppliers and may be covered by U.S. and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from Lightneer Incorporated.
//
using UnityEngine;
using UnityEditor;

namespace Lightneer.AdsEditor
{
    public class AnalyticsEditorWindow : EditorWindow
    {
        public const string ADS_ANALITYCS_PREFAB_NAME = "AdsAnalyticsPrefab";
        public const string PLUGINS_BUNDLE_PATH = "Assets/PluginsBundle";
        public const string PLAYFAB_SHARED_SETTINGS = "PlayFabSharedSettings";

        string playfabTitleId;
        string appsFlyerAndroidId;
        string appsFlyerIosId;
        string appsFlyerDevKey;

        [MenuItem("Lightneer/Analytics/Config")]
        static void Init()
        {
            var window = (AnalyticsEditorWindow)EditorWindow.GetWindow(typeof(AnalyticsEditorWindow));
            window.Show();
        }

        void OnGUI()
        {
            showPlayFab();
            showAppsFlyer();

            if (GUILayout.Button("Open Facebook settings"))
            {
                Facebook.Unity.Editor.FacebookSettingsEditor.Edit();
            }

            if (GUILayout.Button("Open GameAnalytics settings"))
            {
                EditorApplication.ExecuteMenuItem("Window/GameAnalytics/Select Settings");
            }
        }

        private void showPlayFab()
        {
            GUILayout.Label("PlayFab", EditorStyles.boldLabel);
            playfabTitleId = EditorGUILayout.TextField("Title id:", playfabTitleId);
            if (GUILayout.Button("Update PlayFab"))
            {
                updatePlayFab();
            }
        }

        private void showAppsFlyer()
        {
            GUILayout.Label("AppsFlyer", EditorStyles.boldLabel);
            appsFlyerAndroidId = EditorGUILayout.TextField("Android id:", appsFlyerAndroidId);
            appsFlyerIosId = EditorGUILayout.TextField("iOS id:", appsFlyerIosId);
            appsFlyerDevKey = EditorGUILayout.TextField("iOS id:", appsFlyerDevKey);
            if (GUILayout.Button("Update AppsFlyer"))
            {
                updateAppsFlyerPrefab();
            }
        }

        private void updateAppsFlyerPrefab()
        {
            string[] guids = AssetDatabase.FindAssets(ADS_ANALITYCS_PREFAB_NAME, new[] { PLUGINS_BUNDLE_PATH });
            if (guids.Length > 0)
            {
                var pluginsPrefabRoot = PrefabUtility.LoadPrefabContents(AssetDatabase.GUIDToAssetPath(guids[0]));
                updateAppsFlyer(pluginsPrefabRoot);                
                PrefabUtility.SaveAsPrefabAsset(pluginsPrefabRoot, AssetDatabase.GUIDToAssetPath(guids[0]));
                PrefabUtility.UnloadPrefabContents(pluginsPrefabRoot);
            }
        }

        private void updateAppsFlyer(GameObject pluginsPrefabRoot)
        {
            var appsFlyer = pluginsPrefabRoot.GetComponentInChildren<AppsflyerIntegration>(true);
            if (appsFlyer != null)
            {
                appsFlyer.androidAppId = appsFlyerDevKey;
                appsFlyer.iOSAppId = appsFlyerIosId;
                appsFlyer.devKey = appsFlyerDevKey;
                Debug.Log("Updated AppsFlyer config");
            }
            else
            {
                Debug.Log("Failed updating AppsFlyer config");
            }
        }

        private void updatePlayFab()
        {
            string[] guids = AssetDatabase.FindAssets(PLAYFAB_SHARED_SETTINGS, new[] { PLUGINS_BUNDLE_PATH });            
            if (guids.Length > 0)
            {
                string pathToSharedSettings = null;
                foreach (var g in guids)
                {
                    var tmpPath = AssetDatabase.GUIDToAssetPath(g);
                    if (tmpPath.EndsWith(".asset"))
                    {
                        pathToSharedSettings = tmpPath;
                        break;
                    }
                }
                if (pathToSharedSettings != null)
                {
                    var playFabSharedSettings = AssetDatabase.LoadAssetAtPath<PlayFabSharedSettings>(pathToSharedSettings);
                    if (playFabSharedSettings != null)
                    {
                        playFabSharedSettings.TitleId = playfabTitleId;
                        AssetDatabase.SaveAssets();
                        Debug.Log("PlayFabSharedSettings updated");
                    }
                    else
                    {
                        Debug.Log("PlayFabSharedSettings not found");
                    }
                }
            }
            else
            {
                Debug.Log("PlayFabSharedSettings not found");
            }
        }
    }
}

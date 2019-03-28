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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lightneer.Ads;

namespace Lightneer.AdsEditor
{
    public class AdsEditorWindow : EditorWindow
    {
        public const string ADMOB_XML = "<meta-data android:name=\"com.google.android.gms.ads.APPLICATION_ID\"" +
                                        " android:value=\"{0}\"/>";
        public const string MANIFEST_PATH = "Plugins/Android/AndroidManifest.xml";
        public const string ADS_ANALITYCS_PREFAB_NAME = "AdsAnalyticsPrefab";
        public const string PLUGINS_BUNDLE_PATH = "Assets/PluginsBundle";

        string androidKey = "87995e15";
        string iosKey = "8799979d";
        string adMobAppID = "";
        bool groupEnabled;

        private string manifestEditorPath
        {
            get { return "Assets/" + MANIFEST_PATH; }
        }

        private string manifestAbsolutePath
        {
            get { return Application.dataPath + "/" + MANIFEST_PATH; }
        }

        private string adMobString
        {
            get { return string.Format(ADMOB_XML, adMobAppID); }
        }

        [MenuItem("Lightneer/Ads/Config")]
        static void Init()
        {
            var window = (AdsEditorWindow)EditorWindow.GetWindow(typeof(AdsEditorWindow));
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Iron Source keys", EditorStyles.boldLabel);
            androidKey = EditorGUILayout.TextField("Android key:", androidKey);
            iosKey = EditorGUILayout.TextField("iOS key:", iosKey);

            var adMobContent = new GUIContent("Android adMob AppID[?]:", "Leave it empty if you don't have it");
            adMobAppID = EditorGUILayout.TextField(adMobContent, adMobAppID);
            if (!string.IsNullOrEmpty(adMobAppID))
            {
                EditorGUILayout.LabelField("Update will overwrite your AndroidManifest.xml");
            }

            if (GUILayout.Button("Update"))
            {
                updatePrefab();
                updateAndroidXML();
            }
        }

        private void updateAndroidXML()
        {
            if (string.IsNullOrEmpty(adMobAppID))
                return;
            var manifestEditor = new ManifestEditor(manifestEditorPath);
            if (manifestEditor.isDocumentOpen)
            {
                manifestEditor.addChildToUniqueTag("application", string.Format(ADMOB_XML, adMobAppID));
                manifestEditor.save(manifestAbsolutePath);
                Debug.Log("Updated android manifest");
            }
        }

        private void updatePrefab()
        {
            string[] guids = AssetDatabase.FindAssets(ADS_ANALITYCS_PREFAB_NAME, new[] { PLUGINS_BUNDLE_PATH });
            if (guids.Length > 0)
            {
                var pluginsPrefabRoot = PrefabUtility.LoadPrefabContents(AssetDatabase.GUIDToAssetPath(guids[0]));
                var ironSourceIntegration = pluginsPrefabRoot.GetComponentInChildren<IronSrcIntegration>(true);                
                if (ironSourceIntegration != null)
                {
                    ironSourceIntegration.androidKey = androidKey;
                    ironSourceIntegration.iosKey = iosKey;
                    PrefabUtility.SaveAsPrefabAsset(pluginsPrefabRoot, AssetDatabase.GUIDToAssetPath(guids[0]));
                    PrefabUtility.UnloadPrefabContents(pluginsPrefabRoot);
                    Debug.Log("Updated iron source keys");
                }
            }
        }
    }
}

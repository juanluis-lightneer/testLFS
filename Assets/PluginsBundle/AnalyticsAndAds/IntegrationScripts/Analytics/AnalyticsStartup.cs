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
using GameEventSystem;
using System;
using UniQue;
using UnityEngine;

namespace Lightneer.Analytics
{
    public class AnalyticsStartup : SingletonMonoBehaviour<AnalyticsStartup>
    {
        public const string GAMEANALYTICS_PREFAB_NAME = "GameAnalytics";

        [System.Serializable]
        public class AnalyticsProviderConfig
        {
            public bool isEnabled;
            public AnalyticsProvider provider;
        }

        [Serializable]
        public struct References
        {
            public AnalyticsProviderConfig[] analyticsProviderConfigs;
        }
        public References refs;

        public bool enableDebug;

        public override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            refs.analyticsProviderConfigs = new AnalyticsProviderConfig[1];
            refs.analyticsProviderConfigs[0] = new AnalyticsProviderConfig()
            {
                isEnabled = true,
                provider = gameObject.AddComponent<EditorDummyProvider>(),
            };
            var gameanalytics = GameObject.Find(GAMEANALYTICS_PREFAB_NAME);
            if (gameanalytics != null)
            {
                DestroyImmediate(gameanalytics);
            }
#endif
        }

        private void Start()
        {
            foreach (var cfg in refs.analyticsProviderConfigs)
            {
                if (cfg.isEnabled)
                    cfg.provider.init();
            } 
        }        
 
        private void showDebug(string message)
        {
            if (enableDebug)
            {
                Debug.Log("[UNIFIED ANALYTICS] " + message);
            }
        }
    }
}

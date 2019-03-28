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
using UnityEngine;

namespace Lightneer.Analytics
{
    public class EditorDummyProvider : AnalyticsProvider
    {
        public override void init()
        {
            EventManager.Connect<OnAnalyticsGameOver>(onAnalyticsGameOver);
            EventManager.Connect<OnAnalyticsGameStart>(onAnalyticsGameStart);
            EventManager.Connect<OnAnalyticsAdStart>(onAnalyticsAdStart);
            EventManager.Connect<OnAnalyticsAdComplete>(onAnalyticsAdComplete);
            EventManager.Connect<OnAnalyticsSessionStart>(onAnalyticsSessionStart);
            EventManager.Connect<OnAnalyticsSessionComplete>(onAnalyticsSessionComplete);
            EventManager.Connect<OnAnalyticsCustomEvent>(onAnalyticsCustomEvent);
        }

        private void onAnalyticsCustomEvent(OnAnalyticsCustomEvent evt)
        {
            showLog("OnAnalyticsCustomEvent");
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete evt)
        {
            showLog("OnAnalyticsSessionComplete");
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart evt)
        {
            showLog("OnAnalyticsSessionStart");
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete evt)
        {
            showLog("OnAnalyticsAdComplete");
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart evt)
        {
            showLog("OnAnalyticsAdStart");
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver evt)
        {
            showLog("OnAnalyticsGameOver");
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart evt)
        {
            showLog("OnAnalyticsGameStart");
        }

        private void showLog(string log)
        {
            Debug.Log("[EditorDummyAnalytics] " + log);
        }
    }
}

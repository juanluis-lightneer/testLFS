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
using System;
using GameEventSystem;
using UnityEngine;
using UnityEngine.Analytics;

namespace Lightneer.Analytics
{
    public class UnityAnalytics : AnalyticsProvider
    {
        public bool enableDebug;
        public string lastSceenKey = "lastScreen";
        public string sessionTimeKey = "sessionTime";
        public string scoreValueKey = "value";

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

        private static void sendUnityCustomEvent(IAnalyticsEvent analyticsEvent)
        {
            AnalyticsEvent.Custom(analyticsEvent.eventName, analyticsEvent.dataAsDictionary);
        }

        private void onAnalyticsCustomEvent(OnAnalyticsCustomEvent evt)
        {
            sendUnityCustomEvent(evt);
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete evt)
        {
            sendUnityCustomEvent(evt);
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart evt)
        {
            sendUnityCustomEvent(evt);
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete evt)
        {
            var iAnalyticsEvent = (IAnalyticsEvent)evt;
            AnalyticsEvent.AdComplete(evt.isRewarded(), AdvertisingNetwork.None, 
                eventData: ((IAnalyticsEvent)evt).dataAsDictionary);
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart evt)
        {
            AnalyticsEvent.AdStart(evt.isRewarded(), AdvertisingNetwork.None, 
                eventData: ((IAnalyticsEvent)evt).dataAsDictionary);
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver evt)
        {
            AnalyticsEvent.GameOver(eventData: ((IAnalyticsEvent)evt).dataAsDictionary);
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart evt)
        {
            AnalyticsEvent.GameStart(eventData: ((IAnalyticsEvent)evt).dataAsDictionary);
        }

        private void showDebug(string debugString)
        {
            if (enableDebug)
            {
                Debug.Log("[UnityAnalyticsProvider] " + debugString);
            }
        }
    }
}

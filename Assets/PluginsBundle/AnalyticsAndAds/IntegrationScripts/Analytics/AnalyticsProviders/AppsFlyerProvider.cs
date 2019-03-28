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
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;

namespace Lightneer.Analytics
{
    public class AppsFlyerProvider : AnalyticsProvider
    {
        public bool enableDebug;

        public override void init()
        {
            if (enableDebug)
                Debug.Log("[AppsFlyerProvider] Initializing");
            EventManager.Connect<OnAnalyticsGameOver>(onAnalyticsGameOver);
            EventManager.Connect<OnAnalyticsGameStart>(onAnalyticsGameStart);
            EventManager.Connect<OnAnalyticsAdStart>(onAnalyticsAdStart);
            EventManager.Connect<OnAnalyticsAdComplete>(onAnalyticsAdComplete);
            EventManager.Connect<OnAnalyticsSessionStart>(onAnalyticsSessionStart);
            EventManager.Connect<OnAnalyticsSessionComplete>(onAnalyticsSessionComplete);
            EventManager.Connect<OnAnalyticsCustomEvent>(onAnalyticsCustomEvent);
        }

        private static void sendAppsFlyerEvent(IAnalyticsEvent analyticsEvent)
        {
            var dictionary = analyticsEvent.dataAsDictionary.toStringValuedDictionary();
            AppsFlyer.trackRichEvent(analyticsEvent.eventName, dictionary);
        }

        private void onAnalyticsCustomEvent(OnAnalyticsCustomEvent data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart data)
        {
            sendAppsFlyerEvent(data);
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver data)
        {
            sendAppsFlyerEvent(data);
        }
    }
}

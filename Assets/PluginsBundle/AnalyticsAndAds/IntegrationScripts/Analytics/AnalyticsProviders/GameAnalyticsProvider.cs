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
using GameAnalyticsSDK;
using GameEventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lightneer.Analytics
{
    public class GameAnalyticsProvider : AnalyticsProvider
    {
        public bool enableDebug;

        public override void init()
        {
            showDebug("Initializing");
            GameAnalytics.Initialize();
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
            sendCustomEventData(evt);
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete evt)
        {
            sendCustomEventData(evt);
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart evt)
        {
            sendCustomEventData(evt);
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete evt)
        {
            sendCustomEventData(evt);
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart evt)
        {
            sendCustomEventData(evt);
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver evt)
        {
            var status = GAProgressionStatus.Complete;
            if (evt.reason == EventStrings.MATCH_END_LOSE)
            {
                status = GAProgressionStatus.Fail;
            }
            else if (evt.reason == EventStrings.MATCH_END_QUIT)
            {
                status = GAProgressionStatus.Undefined;
            }
            GameAnalytics.NewProgressionEvent(status, evt.level.ToString(), evt.score);
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart evt)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, evt.level.ToString());
        }

        private void sendCustomEventData(IAnalyticsEvent evt)
        {
            foreach (var p in evt.dataAsDictionary)
            {
                processDesignEventData(evt.eventName, p);
            }
        }

        private float processDesignEventData(string customType, KeyValuePair<string, object> dataEntry)
        {
            float numericValue = 0.0f;
            string stringValue = string.Empty;
            if (dataEntry.Value.tryGetFloatValue(out numericValue))
            {
                sendNumericValue(customType, dataEntry, numericValue);
            }
            else if (dataEntry.Value.tryGetStringValue(out stringValue))
            {
                sendStringValue(customType, dataEntry, stringValue);
            }
            return numericValue;
        }

        private void sendStringValue(string customType, KeyValuePair<string, object> dataEntry, string stringValue)
        {
            string designEventString = customType + ":" + dataEntry.Key + ":" + stringValue;
            showDebug("Sending " + designEventString + " with value " + stringValue);
            GameAnalytics.NewDesignEvent(designEventString);
        }

        private void sendNumericValue(string customType, KeyValuePair<string, object> dataEntry, float numericValue)
        {
            string designEventString = customType + ":" + dataEntry.Key;
            showDebug("Sending " + designEventString + " with value " + numericValue);
            GameAnalytics.NewDesignEvent(designEventString, numericValue);
        }

        private void showDebug(string logString)
        {
            if (enableDebug)
                Debug.Log("[GameAnalyticsProvider] " + logString);
        }
    }
}

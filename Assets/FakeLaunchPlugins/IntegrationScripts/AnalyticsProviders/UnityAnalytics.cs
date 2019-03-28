using GameEventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Analytics
{
    public class UnityAnalytics : AnalyticsProvider
    {
        public bool enableDebug;
        public string lastSceenKey = "lastScreen";
        public string sessionTimeKey = "sessionTime";
        public string scoreValueKey = "value";

        public override void init()
        {

        }

        public override void sendEvent(OnAnalyticsEvent evt, float sessionTime)
        {
            var eventString = Enum.GetName(typeof(AnalyticsEventType), evt.type);
            var customData = new Dictionary<string, object>();
            customData.Add(sessionTimeKey, sessionTime);
            switch (evt.type)
            {
                case AnalyticsEventType.ADStart:
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] ADStart " + customData);
                    AnalyticsEvent.AdStart(false, AdvertisingNetwork.None, eventData: customData);
                    break;
                case AnalyticsEventType.ADEnd:
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] ADEnd " + customData);
                    AnalyticsEvent.AdComplete(false, AdvertisingNetwork.None, eventData: customData);
                    break;
                case AnalyticsEventType.MatchStart:
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] MatchStart " + customData);
                    AnalyticsEvent.GameStart(eventData: customData);
                    break;
                case AnalyticsEventType.MatchEnd:
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] MatchEnd " + customData);
                    AnalyticsEvent.GameOver(eventData: customData);
                    break;
                case AnalyticsEventType.SessionTime:
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] SessionTime " + eventString + " " + customData);
                    AnalyticsEvent.Custom(eventString, eventData: customData);
                    break;
                case AnalyticsEventType.HighScore:
                case AnalyticsEventType.Score:
                    handleScoreEvent(eventString, evt.data, ref customData);
                    break;
                case AnalyticsEventType.LastScreen:
                    customData.Add(lastSceenKey, evt.data);
                    if (enableDebug)
                        Debug.Log("[UnityAnalyticsProvider] LastScreen " + lastSceenKey + " " + evt.data);
                    AnalyticsEvent.Custom(eventString, eventData: customData);
                    break;
                case AnalyticsEventType.Custom:
                    handleCustomEvent(evt, ref customData);
                    break;
            }
        }

        private void handleCustomEvent(OnAnalyticsEvent evt, ref Dictionary<string, object> customData)
        {
            foreach (var p in evt.getCustomData())
            {
                customData.Add(p.Key, p.Value);
            }
            if (enableDebug)
                Debug.Log("[UnityAnalyticsProvider] Custom event " + evt.customEventTypeString);
            AnalyticsEvent.Custom(evt.customEventTypeString, eventData: customData);
        }

        private void handleScoreEvent(string eventString, object data, ref Dictionary<string, object> customData)
        {
            float numericValue = 0.0f;
            if (data.tryGetFloatValue(out numericValue))
            {
                customData.Add(scoreValueKey, (int)numericValue);
                if (enableDebug)
                    Debug.Log("[UnityAnalyticsProvider] Custom event (score) " + eventString + " = " + numericValue);
                AnalyticsEvent.Custom(eventString, eventData: customData);
            }
        }
    }
}

using GameAnalyticsSDK;
using GameEventSystem;
using System;
using UnityEngine;

namespace Analytics
{
    public class GameAnalyticsProvider : AnalyticsProvider
    {
        public bool enableDebug;

        public override void init()
        {
            if (enableDebug)
                Debug.Log("[GameAnalyticsProvider] Initializing");
            GameAnalytics.Initialize();
        }

        public override void sendEvent(OnAnalyticsEvent evt, float sessionTime)
        {
            var eventString = Enum.GetName(typeof(AnalyticsEventType), evt.type);
            switch (evt.type)
            {
                case AnalyticsEventType.HighScore:
                case AnalyticsEventType.Score:
                    handleScoreEvent(evt, eventString);
                    break;
                case AnalyticsEventType.LastScreen:
                    if (enableDebug)
                        Debug.Log("[GameAnalyticsProvider] Sending LastScreen " + (string)evt.data + " time: " + (int)sessionTime);
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, eventString, (string)evt.data, (int)sessionTime);
                    break;
                case AnalyticsEventType.Custom:
                    handleCustomEvent(evt, sessionTime);
                    break;
                default:
                    if (enableDebug)
                        Debug.Log("[GameAnalyticsProvider] Sending design event " + eventString + " time: " + sessionTime);
                    GameAnalytics.NewDesignEvent(eventString, sessionTime);
                    break;
            }
        }

        private void handleCustomEvent(OnAnalyticsEvent evt, float sessionTime)
        {
            var customType = evt.customEventTypeString;
            float numericValue = 0.0f;
            foreach (var p in evt.getCustomData())
            {
                if (p.Value.tryGetFloatValue(out numericValue))
                {
                    string designEventString = customType + ":" + p.Key;
                    if (enableDebug)
                        Debug.Log("[GameAnalyticsProvider] Sending " + designEventString + " with value " + numericValue);
                    GameAnalytics.NewDesignEvent(designEventString, numericValue);
                }
            }
        }

        private void handleScoreEvent(OnAnalyticsEvent evt, string eventString)
        {
            int numericValue = 0;
            if (evt.data.tryGetIntValue(out numericValue))
            {
                if (enableDebug)
                    Debug.Log("[GameAnalyticsProvider] Sending " + eventString + " with value " + numericValue);
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, eventString, numericValue);
            }
        }
    }
}

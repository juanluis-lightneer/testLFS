using GameAnalyticsSDK;
using GameEventSystem;
using System;
using UnityEngine;

namespace Analytics
{
    public class AppsFlyerProvider : AnalyticsProvider
    {
        public bool enableDebug;

        public override void init()
        {
            if (enableDebug)
                Debug.Log("[AppsFlyerProvider] Initializing");
        }

        public override void sendEvent(OnAnalyticsEvent evt, float sessionTime)
        {
            string eventName = Enum.GetName(typeof(AnalyticsEventType), evt.type);
            
            if(evt.type == AnalyticsEventType.Custom)
                eventName = evt.customEventTypeString;
            
            AppsFlyer.trackRichEvent(eventName, evt.getCustomDataAsString());
        }
    }
}

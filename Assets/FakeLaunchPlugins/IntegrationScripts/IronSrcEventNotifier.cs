using GameEventSystem;
using System;
using UnityEngine;

namespace PluginsIntegration
{
    public class IronSrcEventNotifier : MonoBehaviour
    {
        [Serializable]
        public struct IronSrcEventConfig
        {
            public IronSrcEventType type;
            public AnalyticsEventType analyticsEventType;
            public bool includeData;
            public bool enabled;
        }
        public IronSrcEventConfig[] ironSourceEventsConfig;

        public bool enableDebug;

        public void Start()
        {
            EventManager.Connect<OnIronSrcEvent>(onIronSrcEvent);
        }

        private void onIronSrcEvent(OnIronSrcEvent evt)
        {
            if (enableDebug)
            {
                Debug.Log("[IRONSRCEVENTNOTIFIER] " + evt.type + " " + evt.content);
            }
            for (int i = 0; i < ironSourceEventsConfig.Length; ++i)
            {
                if (ironSourceEventsConfig[i].enabled && evt.type == ironSourceEventsConfig[i].type)
                {
                    var analyticsEventType = ironSourceEventsConfig[i].analyticsEventType;
                    Debug.Log("[IRONSRCEVENTNOTIFIER] Found match on type: " + analyticsEventType);
                    if (ironSourceEventsConfig[i].includeData)
                    {
                        var data = evt.content;
                        EventManager.Send(new OnAnalyticsEvent(analyticsEventType, data));
                    }
                    else
                    {
                        EventManager.Send(new OnAnalyticsEvent(analyticsEventType, null));
                    }
                }
            }
        }
    }
}

using GameEventSystem;
using UnityEngine;

namespace Analytics
{
    public class EditorDummyProvider : AnalyticsProvider
    {
        public override void init()
        {
            
        }

        public override void sendEvent(OnAnalyticsEvent evt, float sessionTime)
        {
            if (evt.type == AnalyticsEventType.Custom)
            {
                Debug.Log("Custom event: " + evt.customEventTypeString + " | Time:" + sessionTime);
                foreach (var p in evt.getCustomData())
                {
                    Debug.Log(p.Key + " => " + p.Value);
                }
                Debug.Log("/Custom event");
                return;
            }
            Debug.Log("Event: " + evt.type + " | Data: " + evt.data + " | Time:" + sessionTime);
        }
    }
}

using GameEventSystem;
using System;
using UnityEngine;

namespace Analytics
{
    public class UnifiedAnalytics : MonoBehaviour
    {
        private static UnifiedAnalytics sInstance;
        public UnifiedAnalytics instance
        {
            get
            {
                if (sInstance == null)
                {
                    var go = new GameObject();
                    sInstance = go.AddComponent<UnifiedAnalytics>();
                }
                return sInstance;
            }
        }

        [Serializable]
        public struct References
        {
            public AnalyticsProvider[] analyticsProviders;
        }
        public References refs;

        public bool enableDebug;

#if UNITY_EDITOR
        private void Awake()
        {
            refs.analyticsProviders = new AnalyticsProvider[1];
            refs.analyticsProviders[0] = gameObject.AddComponent<EditorDummyProvider>();
        }
#endif

        private void Start()
        {
            sInstance = this;
            foreach (var provider in refs.analyticsProviders)
            {
                provider.init();
            }
            EventManager.Connect<OnAnalyticsEvent>(onAnalyticsEvent);
        }

        private void onAnalyticsEvent(OnAnalyticsEvent evt)
        {
            sendEvent(evt);
        }

        private float getTime()
        {
            return Time.realtimeSinceStartup;
        }
        
        public void sendEvent(OnAnalyticsEvent analyticsEvent)
        {
            var time = getTime();
            if (enableDebug)
            {
                Debug.Log("[ANALYTICS EVENT] " + analyticsEvent.type + " " + analyticsEvent.customEventTypeString);
            }
            foreach (var provider in refs.analyticsProviders)
            {
                provider.sendEvent(analyticsEvent, time);
            }
        }
    }
}

using GameEventSystem;
using System;
using UnityEngine;

namespace Analytics
{
    public class LastScreenEventSender : MonoBehaviour
    {
        [Serializable]
        public struct References
        {
            public AbstractLastScreenTracker lastScreenConcreteTracker;
        }
        public References refs;
        
        private void OnApplicationPause(bool pause)
        {
            if (pause && refs.lastScreenConcreteTracker != null)
            {
                EventManager.Send(new OnAnalyticsEvent(AnalyticsEventType.LastScreen, refs.lastScreenConcreteTracker.lastScreen));
            }
        }
    }
}

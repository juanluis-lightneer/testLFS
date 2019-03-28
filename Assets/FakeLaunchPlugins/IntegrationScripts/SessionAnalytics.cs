using GameEventSystem;
using UnityEngine;

namespace Analytics
{
    public class SessionAnalytics : MonoBehaviour
    {
        public bool isSessionTimeEnabled = true;

        private void OnApplicationPause(bool pause)
        {
            if (isSessionTimeEnabled && pause)
            {
                EventManager.Send(new OnAnalyticsEvent(AnalyticsEventType.SessionTime, null));                
            }
        }
    }
}

using GameEventSystem;
using UnityEngine;

namespace Analytics
{
    public abstract class AnalyticsProvider : MonoBehaviour
    {
        public abstract void init();
        public abstract void sendEvent(OnAnalyticsEvent evt, float sessionTime);
    }
}

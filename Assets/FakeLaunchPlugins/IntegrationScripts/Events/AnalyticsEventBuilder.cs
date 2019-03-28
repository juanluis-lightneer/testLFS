using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameEventSystem
{
    public class AnalyticsEventBuilder
    {
        private OnAnalyticsEvent mAnalyticsEvent;
        private string mEventType;
        private float mData;

        public AnalyticsEventBuilder()
        {
            mAnalyticsEvent = new OnAnalyticsEvent();
        }
        
        public void setTypeString(string type)
        {
            mAnalyticsEvent.setCustomEventTypeString(type);
        }

        public void addData(string dataName, object data)
        {
            mAnalyticsEvent.addCustomData(new KeyValuePair<string, object>(dataName, data));
        }

        public OnAnalyticsEvent build()
        {
            return mAnalyticsEvent;
        }
    }
}

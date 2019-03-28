using System;
using System.Collections.Generic;

namespace GameEventSystem
{
    public enum AnalyticsEventType
    {
        ADStart,
        ADEnd,
        MatchStart,
        MatchEnd,
        Score,
        HighScore,
        SessionTime,
        LastScreen,
        Custom
    }

    /// <summary>
    /// TODO: THIS NEEDS REFACTORING. NO TIME AT THE MOMENT :_(
    /// </summary>
    public class OnAnalyticsEvent : EventManager.Event
    {
        public const char DATA_SEPARATOR = ';';
        public const string DATA_KEY_STRING = "data";
        public const string CUSTOM_EVENT_NAME_KEY_STRING = "customEventType";

        public AnalyticsEventType type { get; private set; }
        public object data
        {
            get
            {
                if (type == AnalyticsEventType.Custom)
                    return "";
                return mData[DATA_KEY_STRING];
            }
            set { mData[DATA_KEY_STRING] = value; }
        }
        private Dictionary<string, object> mData;

        public OnAnalyticsEvent(AnalyticsEventType type, object data)
        {
            mData = new Dictionary<string, object>();
            this.type = type;
            this.data = data;
        }

        ///
        /// CUSTOM EVENT HANDLING
        ///
        
        public string customEventTypeString
        {
            get
            {
                if (type != AnalyticsEventType.Custom)
                    return Enum.GetName(typeof(AnalyticsEventType), type);
                return (string)mData[CUSTOM_EVENT_NAME_KEY_STRING];
            }
        }

        public OnAnalyticsEvent()
        {
            type = AnalyticsEventType.Custom;
            mData = new Dictionary<string, object>();
        }

        public void setCustomEventTypeString(string type)
        {
            mData.Add(CUSTOM_EVENT_NAME_KEY_STRING, type);
        }

        public void addCustomData(KeyValuePair<string, object> data)
        {
            mData.Add(data.Key, data.Value);
        }

        public IEnumerable<KeyValuePair<string, object>> getCustomData()
        {
            foreach (var k in mData.Keys)
            {
                yield return new KeyValuePair<string, object>(k, mData[k]);
            }
        }
        
        public Dictionary<string, string> getCustomDataAsString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in mData.Keys)
            {
                dict.Add(k, mData[k].ToString());
            }

            return dict;
        }

        public Dictionary<string, object> getCustomDataDictionary()
        {
            return mData;
        }
    }
}

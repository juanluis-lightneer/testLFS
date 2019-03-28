//-----------------------------------------------------------------------------
//
// Lightneer Inc Confidential
//
//
// 2015 - 2019 (C) Lightneer Incorporated
// All Rights Reserved.
//
// NOTICE:  All information contained herein is, and remains
// the property of Lightneer Incorporated and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to Lightneer Incorporated
// and its suppliers and may be covered by U.S. and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from Lightneer Incorporated.
//
using Lightneer.Analytics;
using System.Collections.Generic;

namespace GameEventSystem
{
    public struct OnAnalyticsCustomEvent : EventManager.Event, IAnalyticsEvent
    {
        private string mEventName;
        public string eventName { get { return mEventName; } }

        private Dictionary<string, object> mEventData;
        public Dictionary<string, object> dataAsDictionary { get { return mEventData; } }
 
        public OnAnalyticsCustomEvent(string eventName, Dictionary<string,object> eventData)
        {
            mEventName = eventName;
            mEventData = eventData;
            eventData.Add(EventStrings.ATTR_SESSION_TIME, SessionTimeTracker.instance.sessionTime);
        }
    }
}

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
    public struct OnAnalyticsGameStart : EventManager.Event, IAnalyticsEvent
    {
        public string reason;
        public float sessionTime;
        public int level;
        public string origin;
        public int softCurrency;

        public OnAnalyticsGameStart(string reason, int level, string origin, int softCurrency)
        {
            this.reason = reason;
            this.level = level;
            this.origin = origin;
            this.softCurrency = softCurrency;
            sessionTime = SessionTimeTracker.instance.sessionTime;
        }

        public string eventName { get { return EventStrings.EVENT_MATCH_START; } }

        public Dictionary<string, object> dataAsDictionary
        {
            get
            {
                var data = new Dictionary<string, object>();
                data.Add(EventStrings.ATTR_REASON, reason);
                data.Add(EventStrings.ATTR_SESSION_TIME, sessionTime);
                data.Add(EventStrings.ATTR_LEVEL, level);
                data.Add(EventStrings.ATTR_ORIGIN, origin);
                data.Add(EventStrings.ATTR_SOFT_CURRENCY, softCurrency);
                return data;
            }
        }
    }
}

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
    public struct OnAnalyticsSessionComplete : EventManager.Event, IAnalyticsEvent
    {        
        public string reason;
        public string screen;
        public float sessionTime;
        public int level;
        public int softCurrency;
        public int matchesPlayed;

        public OnAnalyticsSessionComplete(string reason, string screen, int level, int softCurrency, 
                                          int matchesPlayed)
        {
            this.reason = reason;
            this.screen = screen;            
            this.level = level;
            this.softCurrency = softCurrency;
            this.matchesPlayed = matchesPlayed;
            sessionTime = SessionTimeTracker.instance.sessionTime;
        }

        public string eventName { get { return EventStrings.EVENT_SESSION_COMPLETE; } }

        public Dictionary<string, object> dataAsDictionary
        {
            get
            {
                var data = new Dictionary<string, object>();
                data.Add(EventStrings.ATTR_REASON, reason);
                data.Add(EventStrings.ATTR_SCREEN, screen);
                data.Add(EventStrings.ATTR_SESSION_TIME, sessionTime);
                data.Add(EventStrings.ATTR_LEVEL, level);
                data.Add(EventStrings.ATTR_MATCHES_PLAYED, matchesPlayed);
                data.Add(EventStrings.ATTR_SOFT_CURRENCY, softCurrency);
                return data;
            }
        }
    }
}

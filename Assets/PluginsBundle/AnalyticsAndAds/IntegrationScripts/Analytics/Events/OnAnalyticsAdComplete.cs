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
    public struct OnAnalyticsAdComplete : EventManager.Event, IAnalyticsEvent
    {
        public float sessionTime;
        public string screen;

        public OnAnalyticsAdComplete(string screen)
        {
            sessionTime = SessionTimeTracker.instance.sessionTime;
            this.screen = screen;
        }

        public string eventName { get { return EventStrings.EVENT_AD_COMPLETE; } }
        
        public Dictionary<string, object> dataAsDictionary
        {
            get
            {
                var data = new Dictionary<string, object>();
                data.Add(EventStrings.ATTR_SCREEN, screen);
                data.Add(EventStrings.ATTR_SESSION_TIME, sessionTime);
                return data;
            }
        }

        public bool isRewarded()
        {
            return screen == EventStrings.AD_SCREEN_REWARDED;
        }
    }
}

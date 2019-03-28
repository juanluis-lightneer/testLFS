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
    public struct OnAnalyticsGameOver : EventManager.Event, IAnalyticsEvent
    {    
        public string reason;
        public float sessionTime;
        public int level;
        public float matchTime;
        public int softCurrency;
        public int score;
        public int currencyCollected;
        public int goldenNuggets;
        public bool highScore;

        public OnAnalyticsGameOver(string reason, int level, float matchTime, int softCurrency, 
                                   int score, int currencyCollected, int goldenNuggets, bool highScore)
        {
            this.reason = reason;
            this.level = level;
            this.matchTime = matchTime;
            this.softCurrency = softCurrency;
            this.score = score;
            this.currencyCollected = currencyCollected;
            this.goldenNuggets = goldenNuggets;
            this.highScore = highScore;
            sessionTime = SessionTimeTracker.instance.sessionTime;
        }

        public string eventName { get { return EventStrings.EVENT_MATCH_COMPLETE; } }

        public Dictionary<string, object> dataAsDictionary
        {
            get
            {
                var data = new Dictionary<string, object>();
                data.Add(EventStrings.ATTR_REASON, reason);
                data.Add(EventStrings.ATTR_SESSION_TIME, sessionTime);
                data.Add(EventStrings.ATTR_LEVEL, level);
                data.Add(EventStrings.ATTR_MATCH_TIME, matchTime);
                data.Add(EventStrings.ATTR_SOFT_CURRENCY, softCurrency);
                data.Add(EventStrings.ATTR_SCORE, score);
                data.Add(EventStrings.ATTR_CURRENCY_COLLECTED, currencyCollected);
                data.Add(EventStrings.ATTR_NUGGET, goldenNuggets);
                data.Add(EventStrings.ATTR_HIGH_SCORE, score);
                return data;
            }
        }
    }
}

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
using GameEventSystem;
using Lightneer.Analytics;

namespace Lightneer.Ads
{
    public class IronSrcEventInterpreter : AdEventInterpreter
    {
        private AdEventType[] interstitialSpecialEvents = 
        {
            AdEventType.INTERSTITIAL_AD_SHOW_REQUEST,
            AdEventType.INTERSTITIAL_AD_SHOW_FAILED,
            AdEventType.INTERSTITIAL_AD_OPENED,
            AdEventType.INTERSTITIAL_AD_CLOSED
        };

        public bool enableDebug;

        private bool mAdShowRequestResultPending;
        private float mAdShowRequestTime;

        private float currentSessionTime
        {
            get { return SessionTimeTracker.instance.sessionTime; }
        }

        public override void processAdEvent(AdEventInfo eventInfo)
        {
            if (isSpecialEvent(eventInfo))
            {
                handleSpecialEvents(eventInfo);
            }
            else
            {
                sendEvent(eventInfo, currentSessionTime);
            }
        }

        private void handleSpecialEvents(AdEventInfo eventInfo)
        {
            switch (eventInfo.eventType)
            {
                case AdEventType.INTERSTITIAL_AD_SHOW_REQUEST:
                    mAdShowRequestResultPending = true;
                    mAdShowRequestTime = currentSessionTime;
                    break;
                case AdEventType.INTERSTITIAL_AD_OPENED:
                    mAdShowRequestResultPending = false;
                    sendEvent(eventInfo, mAdShowRequestTime);
                    break;
                case AdEventType.INTERSTITIAL_AD_CLOSED:
                    sendEvent(eventInfo, currentSessionTime);
                    break;
            }
        }

        private void sendEvent(AdEventInfo eventInfo, float sessionTime)
        {
            if (isAdStartEvent(eventInfo))
            {
                EventManager.Send(new OnAnalyticsAdStart(getScreen(eventInfo)));
            }
            else if (isAdEndEvent(eventInfo))
            {
                EventManager.Send(new OnAnalyticsAdComplete(getScreen(eventInfo)));
            }
        }

        private static bool isAdEndEvent(AdEventInfo eventInfo)
        {
            return eventInfo.eventType == AdEventType.REWARDED_AD_CLOSED ||
                                 eventInfo.eventType == AdEventType.INTERSTITIAL_AD_CLOSED;
        }

        private static bool isAdStartEvent(AdEventInfo eventInfo)
        {
            return eventInfo.eventType == AdEventType.REWARDED_AD_STARTED ||
                            eventInfo.eventType == AdEventType.BANNER_AD_SCREEN_PRESENTED ||
                            eventInfo.eventType == AdEventType.INTERSTITIAL_AD_OPENED;
        }

        private string getScreen(AdEventInfo eventInfo)
        {
            string typeString = "";
            switch (eventInfo.adType)
            {
                case AdType.Banner:
                    typeString = EventStrings.AD_SCREEN_BANNER;
                    break;
                case AdType.Interstitial:
                    typeString = EventStrings.AD_SCREEN_INTERSTITIAL;
                    break;
                case AdType.RewardedAd:
                    typeString = EventStrings.AD_SCREEN_REWARDED;
                    break;
            }
            return typeString;
        }

        private bool isSpecialEvent(AdEventInfo eventInfo)
        {
            if (eventInfo.adType != AdType.Interstitial)
                return false;
            foreach(var t in interstitialSpecialEvents)
            {
                if (eventInfo.eventType == t)
                    return true;
            }
            return false;
        }
    }
}

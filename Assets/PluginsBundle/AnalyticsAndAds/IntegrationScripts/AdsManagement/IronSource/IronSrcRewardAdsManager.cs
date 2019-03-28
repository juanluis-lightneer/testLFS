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
using System;
using UnityEngine;

namespace Lightneer.Ads
{
    public class IronSrcRewardAdsManager : MonoBehaviour
    {
        public event OnAdEventCallback onAdEventInfo;
        private OnRewardedAdResult mOnRewardedAdResultCallback;
        private string mCurrentPlacementName;

        public void init(OnAdEventCallback adEventInfo)
        {
            onAdEventInfo += adEventInfo;
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

            IronSource.Agent.shouldTrackNetworkState(true);
        }

        /// <summary>
        /// Check rewarded ad availability
        /// </summary>
        /// <returns></returns>
        public bool isRewardedAdAvailable
        {
            get { return IronSource.Agent.isRewardedVideoAvailable(); }
        }

        public bool showRewardedAd(string placementName, OnRewardedAdResult callback)
        {
            if (!isRewardedAdAvailable)
            {
                onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_NOT_AVAILABLE });
                notRewardedCallback(placementName);
                return false;
            }
            ironSrcShowRewardedVideo(placementName);
            saveCallbackInfo(placementName, callback);
            return true;
        }

        private void saveCallbackInfo(string placementName, OnRewardedAdResult callback)
        {
            mOnRewardedAdResultCallback = callback;
            mCurrentPlacementName = placementName;
        }

        private void ironSrcShowRewardedVideo(string placementName)
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_SHOW_REQUEST });
            if (string.IsNullOrEmpty(placementName))
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                IronSource.Agent.showRewardedVideo(placementName);
            }
        }

        private void notRewardedCallback(string placementName)
        {
            doRewardedAdResultCallback(new RewardedAdResultData(placementName, false));
        }

        private void doRewardedAdResultCallback(RewardedAdResultData resultData)
        {
            if (mOnRewardedAdResultCallback == null)
                return;
            var callback = mOnRewardedAdResultCallback;
            mOnRewardedAdResultCallback = null;
            callback(resultData);            
        }

        /// Invoked on click
        private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_CLICKED });
        }

        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_OPENED });
        }

        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_CLOSED });
        }

        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            // Not used atm            
        }

        //  Note: the events below are not available for all supported rewarded video 
        //   ad networks. Check which events are available per ad network you choose 
        //   to include in your build.
        //   We recommend only using events which register to ALL ad networks you 
        //   include in your build.
        //Invoked when the video ad starts playing.
        void RewardedVideoAdStartedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_STARTED });
        }
        //Invoked when the video ad finishes playing.
        void RewardedVideoAdEndedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_ENDED });
        }
        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
        //
        //@param - placement - placement object which contains the reward data
        //
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_REWARDED });
            var callbackData = new RewardedAdResultData(mCurrentPlacementName, true);
            doRewardedAdResultCallback(callbackData);
        }
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.RewardedAd, eventType = AdEventType.REWARDED_AD_FAILED });
            notRewardedCallback(mCurrentPlacementName);
        }

    }
}

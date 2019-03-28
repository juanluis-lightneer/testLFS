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
    public class IronSrcInterstitialManager : MonoBehaviour
    {
        public bool enabledDebug;
        public event OnAdEventCallback onAdEventInfo;

        private OnInterstitialLoadResult mOnInterstitialReadyCallback;
        private OnInterstitialEnd mOnInterstitialEndCallback;
        private string mCurrentPlacementName;

        public void init(OnAdEventCallback adEventInfo)
        {
            onAdEventInfo += adEventInfo;
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
        }

        //Invoked when the initialization process has failed.
        //@param description - string - contains information about the failure.
        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            showDebug("InterstitialAdLoadFailedEvent", error.ToString() + " " + error.getDescription());
            onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_LOAD_FAILED });
            doInterstitialLoadResultCallback(false);            
        }

        //Invoked right before the Interstitial screen is about to open.
        void InterstitialAdShowSucceededEvent()
        {
            showDebug("InterstitialAdShowSucceededEvent", "INTERSTITIAL_AD_SHOW_SUCCEEDED");
            onAdEventInfo(new AdEventInfo()
            {
                adType = AdType.Interstitial,
                eventType =  AdEventType.INTERSTITIAL_AD_SHOW_SUCCEEDED
            });
            doInterstitialEndCallback();
        }

        //Invoked when the ad fails to show.
        //@param description - string - contains information about the failure.
        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            showDebug("InterstitialAdShowFailedEvent", error.ToString() + " " + error.getDescription());
            onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_SHOW_FAILED });
            doInterstitialEndCallback();            
        }

        // Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent()
        {
            showDebug("InterstitialAdClickedEvent", "INTERSTITIAL_AD_CLICKED");
            onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_CLICKED });
        }

        //Invoked when the interstitial ad closed and the user goes back to the application screen.
        void InterstitialAdClosedEvent()
        {
            showDebug("InterstitialAdClosedEvent", "INTERSTITIAL_AD_CLOSED");
            onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_CLOSED });
            doInterstitialEndCallback();
        }

        //Invoked when the Interstitial is Ready to shown after load function is called
        void InterstitialAdReadyEvent()
        {
            if (isInterstitialReady)
            {
                showDebug("InterstitialAdReadyEvent", "INTERSTITIAL_AD_READY");
                onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_READY });
            }
            doInterstitialLoadResultCallback(isInterstitialReady);
        }

        private static bool isInterstitialReady
        {
            get { return IronSource.Agent.isInterstitialReady(); }
        }

        //Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent()
        {
            showDebug("InterstitialAdOpenedEvent", "INTERSTITIAL_AD_OPENED");
            onAdEventInfo(new AdEventInfo() { adType = AdType.Interstitial, eventType = AdEventType.INTERSTITIAL_AD_OPENED });
        }

        private void showDebug(string methodName, string message)
        {
            if (!enabledDebug)
                return;
            Debug.Log("[IronSrcInterstitialManager] " + methodName + " " + message);
        }

        #region INTERSTITIALS_HANDLING

        public void loadInterstitial(OnInterstitialLoadResult onInterstitialReady)
        {
            onAdEventInfo(new AdEventInfo()
            {
                adType = AdType.Interstitial,
                eventType = AdEventType.INTERSTITIAL_AD_LOAD_REQUEST
            });
            mOnInterstitialReadyCallback = onInterstitialReady;
            IronSource.Agent.loadInterstitial();
        }

        public void showInterstitial(string interstitialName, OnInterstitialEnd onInterstitialEndCallback)
        {
            if (isInterstitialReady && !IronSource.Agent.isInterstitialPlacementCapped(interstitialName))
            {
                mOnInterstitialEndCallback = onInterstitialEndCallback;
                ironSrcShowInterstitial(interstitialName);
            }
            else
            {
                onAdEventInfo(new AdEventInfo()
                {
                    adType = AdType.Interstitial,
                    eventType = AdEventType.INTERSTITIAL_AD_NOT_AVAILABLE
                });
                onInterstitialEndCallback();
            }
        }

        private void ironSrcShowInterstitial(string interstitialName)
        {
            onAdEventInfo(new AdEventInfo()
            {
                adType = AdType.Interstitial,
                eventType = AdEventType.INTERSTITIAL_AD_SHOW_REQUEST
            });
            if (string.IsNullOrEmpty(interstitialName))
            {
                IronSource.Agent.showInterstitial();
            }
            else
            {
                IronSource.Agent.showInterstitial(interstitialName);
            }
        }

        private void doInterstitialLoadResultCallback(bool loadWasSuccesful)
        {
            if (mOnInterstitialReadyCallback == null)
                return;
            var callback = mOnInterstitialReadyCallback;
            mOnInterstitialReadyCallback = null;
            callback(loadWasSuccesful);
        }

        private void doInterstitialEndCallback()
        {
            if (mOnInterstitialEndCallback == null)
                return;
            var callback = mOnInterstitialEndCallback;
            mOnInterstitialEndCallback = null;
            callback();
        }
        #endregion
    }
}

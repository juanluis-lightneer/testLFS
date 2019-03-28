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
using System;
using UniQue;
using UnityEngine;

namespace Lightneer.Ads
{
    public delegate void OnAdEventCallback(AdEventInfo eventInfo);

    public class AdsManager : SingletonMonoBehaviour<AdsManager>
    {
        [Serializable]
        public struct References
        {
            public AdsProvider provider;
            public AdEventInterpreter eventInterpreter;
        }
        public References refs;

        public bool enableEventsDebug = false;

        public override void Awake()
        {
            base.Awake();
            refs.provider.init(onAdEventHappened);
        }

        private void onAdEventHappened(AdEventInfo eventInfo)
        {
            showDebugInfo("onAdEventHappened", eventInfo.adType + " " + eventInfo.eventType);
            refs.eventInterpreter.processAdEvent(eventInfo);
        }

        private void showDebugInfo(string method, string info)
        {
            if (!enableEventsDebug)
                return;
            Debug.Log("[AdsManager] " + method + " " + info);
        }

        public void showBanner()
        {
            refs.provider.showBanner();
        }

        public void hideBanner()
        {
            refs.provider.hideBanner();
        }

        public void loadInterstitialAd(OnInterstitialLoadResult callback = null)
        {
            refs.provider.loadInterstitial((x) =>
            {
                if (callback != null)
                {
                    callback(x);
                }
            });
        }

        public void showInterstitialdAd(OnInterstitialEnd callback = null)
        {
            showInterstitialdAd(null, callback);
        }

        public void showInterstitialdAd(string placement, OnInterstitialEnd callback = null)
        {
            refs.provider.showInterstitial(placement, () =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
        }

        public bool isRewardedAdAvailable()
        {
            return refs.provider.isRewardedAdAvailable();
        }

        public void showRewardedAd(OnRewardedAdResult callback = null)
        {
            showRewardedAd(null, callback);
        }

        public void showRewardedAd(string placement, OnRewardedAdResult callback = null)
        {
            refs.provider.showRewardedAd(placement, (x) =>
            {
                if (callback != null)
                {
                    callback(x);
                }
            });
        }
    }
}

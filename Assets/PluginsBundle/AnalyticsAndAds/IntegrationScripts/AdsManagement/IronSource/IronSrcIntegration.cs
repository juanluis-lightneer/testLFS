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
    public class IronSrcIntegration : AdsProvider
    {
        [Serializable]
        public struct References
        {
            public IronSrcBannerManager bannerMgr;
            public IronSrcInterstitialManager interstitialMgr;
            public IronSrcRewardAdsManager rewardAdsMgr;
        }
        public References refs;

        public string androidKey;
        public string iosKey;
        public bool debugEnabled = false;
        
        public string appKey
        {
            get
            {
                #if UNITY_IOS
                return iosKey;
                #else
                return androidKey;
                #endif
            }
        }
        
        public override void init(OnAdEventCallback onAdEvent)
        {
            initIronSourceAgent();
            adUnitsInit(onAdEvent);
        }

        private void adUnitsInit(OnAdEventCallback onAdEvent)
        {
            refs.bannerMgr.init(onAdEvent);
            refs.interstitialMgr.init(onAdEvent);
            refs.rewardAdsMgr.init(onAdEvent);
        }

        private void initIronSourceAgent()
        {
            IronSourceConfig.Instance.setClientSideCallbacks(true);
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL,
                                  IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
            IronSource.Agent.setAdaptersDebug(debugEnabled);
            // User consent for EU users for GDPR reasons, not required for people outside EU
            //IronSource.Agent.setConsent(true);
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public override void showBanner()
        {
            refs.bannerMgr.showBanner();
        }

        public override void hideBanner()
        {
            refs.bannerMgr.hideBanner();
        }

        public override void loadInterstitial(OnInterstitialLoadResult callback)
        {
            refs.interstitialMgr.loadInterstitial(callback);
        }

        public override void showInterstitial(string placement, OnInterstitialEnd callback)
        {
            refs.interstitialMgr.showInterstitial(placement, callback);
        }

        public override bool isRewardedAdAvailable()
        {
            return refs.rewardAdsMgr.isRewardedAdAvailable;
        }

        public override void showRewardedAd(string placement, OnRewardedAdResult callback)
        {
            refs.rewardAdsMgr.showRewardedAd(placement, callback);
        }
    }
}

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

using UnityEngine;

namespace Lightneer.Ads
{
    public class IronSrcBannerManager : MonoBehaviour
    {
        public IronSourceBannerSize size = IronSourceBannerSize.SMART;
        public IronSourceBannerPosition position;
        public bool enableDebug;
        public event OnAdEventCallback onAdEventInfo;

        public void init(OnAdEventCallback adEventInfo)
        {
            onAdEventInfo += adEventInfo;
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        }

        //Invoked once the banner has loaded
        private void BannerAdLoadedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_LOADED });
        }

        //Invoked when the banner loading process has failed.
        //@param description - string - contains information about the failure.
        private void BannerAdLoadFailedEvent(IronSourceError error)
        {
            if (enableDebug)
            {
                Debug.LogError(error.ToString());
            }
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_LOAD_FAILED });
        }

        // Invoked when end user clicks on the banner ad
        private void BannerAdClickedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_CLICKED });
        }

        //Notifies the presentation of a full screen content following user click
        private void BannerAdScreenPresentedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_SCREEN_PRESENTED });
        }

        //Notifies the presented screen has been dismissed
        private void BannerAdScreenDismissedEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_SCREEN_DIMISSED });
        }

        //Invoked when the user leaves the app
        private void BannerAdLeftApplicationEvent()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_LEFT_APPLICATION });
        }

        public void showBanner()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_SHOW_REQUEST });
            IronSource.Agent.loadBanner(size, position);
        }

        public void hideBanner()
        {
            onAdEventInfo(new AdEventInfo() { adType = AdType.Banner, eventType = AdEventType.BANNER_AD_HIDE_REQUEST });
            IronSource.Agent.hideBanner();
            IronSource.Agent.destroyBanner();
        }
    }
}

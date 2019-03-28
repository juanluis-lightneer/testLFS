using GameEventSystem;
using UnityEngine;

namespace PluginsIntegration
{
    public class IronSrcBannerManager : MonoBehaviour
    {
        public IronSourceBannerSize size = IronSourceBannerSize.SMART;
        public IronSourceBannerPosition position;

        void Start()
        {
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        }

        //Invoked once the banner has loaded
        void BannerAdLoadedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_LOADED, ""));
        }

        //Invoked when the banner loading process has failed.
        //@param description - string - contains information about the failure.
        void BannerAdLoadFailedEvent(IronSourceError error)
        {
            Debug.LogError(error.ToString());
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_LOAD_FAILED, error.ToString()));
        }

        // Invoked when end user clicks on the banner ad
        void BannerAdClickedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_CLICKED, ""));
        }

        //Notifies the presentation of a full screen content following user click
        void BannerAdScreenPresentedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_SCREEN_PRESENTED, ""));
        }

        //Notifies the presented screen has been dismissed
        void BannerAdScreenDismissedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_SCREEN_DIMISSED, ""));
        }

        //Invoked when the user leaves the app
        void BannerAdLeftApplicationEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_LEFT_APPLICATION, ""));
        }

        public void showBanner()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_SHOW_REQUEST, ""));
            IronSource.Agent.loadBanner(size, position);
        }

        public void hideBanner()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.BANNER_AD_HIDE_REQUEST, ""));
            IronSource.Agent.hideBanner();
            IronSource.Agent.destroyBanner();
        }
    }
}

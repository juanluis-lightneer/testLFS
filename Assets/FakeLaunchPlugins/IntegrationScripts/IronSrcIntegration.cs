using System;
using UnityEngine;

namespace PluginsIntegration
{
    public class IronSrcIntegration : MonoBehaviour
    {
        private static IronSrcIntegration sInstance;
        public static IronSrcIntegration instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = FindObjectOfType<IronSrcIntegration>();
                }
                return sInstance;
            }
        }

        public enum InterstitialPlacement { OnGameOver, OnTimeUp }

        [Serializable]
        public struct References
        {
            public IronSrcBannerManager bannerMgr;
            public IronSrcInterstitialManager interstitialMgr;
            public IronSrcRewardAdsManager rewardAdsMgr;
        }
        public References refs;

        [Serializable]
        public struct InterstitialConfig
        {
            public InterstitialPlacement placementType;
            public string placementName;
        }
        public InterstitialConfig[] interstitialConfigs;

        public string androidKey = "7926f64d";
        public string iosKey = "7926bcc5";

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
        
        void Awake()
        {
            sInstance = this;
            IronSourceConfig.Instance.setClientSideCallbacks(true);

            IronSource.Agent.validateIntegration();

            IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL,
                                  IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
            IronSource.Agent.setAdaptersDebug(true);

            // User consent for EU users for GDPR reasons, not required for people outside EU
            //IronSource.Agent.setConsent(true);

            /*Debug.Log("AD ID: ");
            Debug.Log(IronSource.Agent.getAdvertiserId());*/
        }

        private string getInterstitialName(InterstitialPlacement placementType)
        {
            foreach (var cfg in interstitialConfigs)
            {
                if (cfg.placementType == placementType)
                    return cfg.placementName;
            }
            return "";
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void showBanner()
        {
            refs.bannerMgr.showBanner();
        }

        public void loadInterstitial()
        {
            refs.interstitialMgr.loadInterstitial();
        }

        public void showInterstitial(InterstitialPlacement placementType)
        {
            refs.interstitialMgr.showInterstitial(getInterstitialName(placementType));
        }
    }
}

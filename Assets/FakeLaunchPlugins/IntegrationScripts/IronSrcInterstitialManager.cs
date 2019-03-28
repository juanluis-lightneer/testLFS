using System;
using GameEventSystem;
using UnityEngine;

namespace PluginsIntegration
{
    public class IronSrcInterstitialManager : MonoBehaviour
    {
        public bool enabledDebug;

        void OnEnable()
        {            
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
            IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += onInterstitialAdClosedDemandOnlyEvent;
        }

        private void onInterstitialAdClosedDemandOnlyEvent(string obj)
        {
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] onInterstitialAdClosedDemandOnlyEvent: not implemented. OBJ = " + obj);
            }
        }

        //Invoked when the initialization process has failed.
        //@param description - string - contains information about the failure.
        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_LOAD_FAILED, ""));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdLoadFailedEvent: " + error.ToString() + " " + error.getDescription());
            }
        }

        //Invoked right before the Interstitial screen is about to open.
        void InterstitialAdShowSucceededEvent()
        {
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdShowSucceededEvent == INTERSTITIAL_AD_SHOW_SUCCEEDED");
            }
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_SHOW_SUCCEEDED, ""));
        }

        //Invoked when the ad fails to show.
        //@param description - string - contains information about the failure.
        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_SHOW_FAILED, error.ToString()));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdShowFailedEvent: " + error.ToString() + " " + error.getDescription());
            }
        }

        // Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_CLICKED, ""));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdClickedEvent == INTERSTITIAL_AD_CLICKED");
            }
        }

        //Invoked when the interstitial ad closed and the user goes back to the application screen.
        void InterstitialAdClosedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_CLOSED, ""));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdClosedEvent == INTERSTITIAL_AD_CLOSED");
            }
        }

        //Invoked when the Interstitial is Ready to shown after load function is called
        void InterstitialAdReadyEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_READY, ""));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdReadyEvent == INTERSTITIAL_AD_READY");
            }
        }

        //Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_OPENED, ""));
            if (enabledDebug)
            {
                Debug.LogError("[IronSrcInterstitialManager] InterstitialAdOpenedEvent == INTERSTITIAL_AD_OPENED");
            }
        }

        public void loadInterstitial()
        {
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_LOAD_REQUEST, ""));
            IronSource.Agent.loadInterstitial();
        }

        public void showInterstitial(string interstitialName)
        {
            if (IronSource.Agent.isInterstitialReady() && !IronSource.Agent.isInterstitialPlacementCapped(interstitialName))
            {
                EventManager.Send(new OnIronSrcEvent(IronSrcEventType.INTERSTITIAL_AD_SHOW_REQUEST, ""));
                IronSource.Agent.showInterstitial(interstitialName);
            }
        }
    }
}

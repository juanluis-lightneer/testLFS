using GameEventSystem;
using UnityEngine;

namespace PluginsIntegration
{
    public class IronSrcRewardAdsManager : MonoBehaviour
    {

        void Start()
        {
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

        }
        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent()
        {
            SendAnalyticsEvent(AdEventType.ADStart);
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.REWARDED_AD_OPENED, ""));
        }
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent()
        {
            SendAnalyticsEvent(AdEventType.ADEnd);
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.REWARDED_AD_CLOSED, ""));
        }
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            //Change the in-app 'Traffic Driver' state according to availability.
            bool rewardedVideoAvailability = available;
        }
        //  Note: the events below are not available for all supported rewarded video 
        //   ad networks. Check which events are available per ad network you choose 
        //   to include in your build.
        //   We recommend only using events which register to ALL ad networks you 
        //   include in your build.
        //Invoked when the video ad starts playing.
        void RewardedVideoAdStartedEvent()
        {
        }
        //Invoked when the video ad finishes playing.
        void RewardedVideoAdEndedEvent()
        {
        }
        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
        //
        //@param - placement - placement object which contains the reward data
        //
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            EventManager.Send(SimpleEvent.REWARD_AD_SUCCESS);
            SendAnalyticsEvent(AdEventType.ADRewardSucceed);
            EventManager.Send(new OnIronSrcEvent(IronSrcEventType.REWARDED_AD_SUCCESS, ""));
        }
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            EventManager.Send(SimpleEvent.REWARD_AD_FAILURE);
            SendAnalyticsEvent(AdEventType.ADRewardFailed);
        }

       

        private void SendAnalyticsEvent (AdEventType eventType) {

            //TODO: Enable when implemented:
            var builder = new AnalyticsEventBuilder();
            Debug.LogError("Implementation incomplete!");
            /*
            builder.setTypeString(eventType.ToString());
            builder.addData("sessionTime", SessionTracker.getSessionTime());
            builder.addData("score", GameManager.instance.refs.scoreManager.getRoundScore());
            builder.addData("screen", AdType.Rewarded);
            */
            EventManager.Send(builder.build());
        }
    }
}

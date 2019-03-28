using System;
using System.Collections.Generic;

namespace GameEventSystem
{
    public enum AdEventType
    {
        ADStart,
        ADEnd,
        ADClick,
        ADShowFailed,
        AdLoadFailed,
        ADRewardSucceed,
        ADRewardFailed
    }

    public enum AdType
    {
        Banner,
        Interstitial,
        Rewarded
    }

    /// <summary>
    /// TODO: THIS NEEDS REFACTORING. NO TIME AT THE MOMENT :_(
    /// </summary>
    public class OnAdEvent : EventManager.Event
    {
        public const char DATA_SEPARATOR = ';';
        public const string DATA_KEY_STRING = "data";
        public const string CUSTOM_EVENT_NAME_KEY_STRING = "customEventType";

        public AdEventType adEventType { get; private set; }
        public AdType adType { get; private set; }

        public OnAdEvent(AdEventType _adEventType, AdType _adType)
        {
            this.adEventType = _adEventType;
            this.adType = _adType;
        }
    }
}

namespace GameEventSystem
{
    public enum IronSrcEventType
    {
        INTERSTITIAL_AD_READY,
        INTERSTITIAL_AD_LOAD_FAILED,
        INTERSTITIAL_AD_SHOW_SUCCEEDED,
        INTERSTITIAL_AD_SHOW_FAILED,
        INTERSTITIAL_AD_CLICKED,
        INTERSTITIAL_AD_OPENED,
        INTERSTITIAL_AD_CLOSED,
        INTERSTITIAL_AD_LOAD_REQUEST,
        INTERSTITIAL_AD_SHOW_REQUEST,
        BANNER_AD_LOADED,
        BANNER_AD_LOAD_FAILED,
        BANNER_AD_CLICKED,
        BANNER_AD_SCREEN_PRESENTED,
        BANNER_AD_SCREEN_DIMISSED,
        BANNER_AD_LEFT_APPLICATION,
        BANNER_AD_SHOW_REQUEST,
        BANNER_AD_HIDE_REQUEST,
        REWARDED_AD_OPENED,
        REWARDED_AD_CLOSED,
        REWARDED_AD_SUCCESS,
    }

    public class OnIronSrcEvent : EventManager.Event
    {
        public IronSrcEventType type;
        public string content;

        public OnIronSrcEvent(IronSrcEventType type, string content)
        {
            this.type = type;
            this.content = content;
        }
    }
}

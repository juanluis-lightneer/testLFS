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

namespace Lightneer.Analytics
{
    public static class EventStrings
    {
        // COMMON EVENT NAMES
        public const string EVENT_AD_START = "ad_start";
        public const string EVENT_AD_COMPLETE = "ad_complete";
        public const string EVENT_MATCH_START = "match_start";
        public const string EVENT_MATCH_COMPLETE = "match_complete";
        public const string EVENT_HIGH_SCORE = "high_score";
        public const string EVENT_GOLDEN_NUGGET = "golden_nugget";
        public const string EVENT_SESSION_START = "session_start";
        public const string EVENT_SESSION_COMPLETE = "session_complete";
        public const string EVENT_INSTALL = "install";
        public const string EVENT_IMPRESSION = "impression";
        public const string EVENT_CLICK = "click";
        public const string EVENT_SPEND = "spend";
        public const string EVENT_AD_CLICK = "ad_click";
        public const string EVENT_PURCHASE = "purchase";
        public const string EVENT_SESSION_ID = "session_id";

        // EVENT ATTRIBUTE NAMES
        public const string ATTR_SESSION_TIME = "sessionTime";
        public const string ATTR_CUSTOM_EVENT = "customEventType";
        public const string ATTR_MATCH_TIME = "matchTime";
        public const string ATTR_LEVEL = "level";
        public const string ATTR_REASON = "reason";
        public const string ATTR_SCORE = "score";
        public const string ATTR_SCREEN = "screen";
        public const string ATTR_STARS = "stars";
        public const string ATTR_SOFT_CURRENCY = "softCurrency";
        public const string ATTR_COUNT = "count";
        public const string ATTR_HIT_RATIO = "hitRatio";
        public const string ATTR_MATCHES_PLAYED = "matchesPlayed";
        public const string ATTR_CURRENCY_COLLECTED = "currencyCollected";
        public const string ATTR_NUGGET = "nugget";
        public const string ATTR_SESSION_PLAYED = "sessionPlayed";
        public const string ATTR_ORIGIN = "origin";
        public const string ATTR_CURRENCY_TYPE = "currency_type";
        public const string ATTR_CURRENCY_TOTAL = "currency_total";
        public const string ATTR_CURRENCY_BALANCE_LEFT = "currency_balance_left";
        public const string ATTR_USER_TOTAL_TIME = "user_total_time";
        public const string ATTR_ITEM = "item";
        public const string ATTR_PRICE = "price";
        public const string ATTR_AD_TIME = "ad_time";
        public const string ATTR_REVIVES = "revives";
        public const string ATTR_COLLISIONS = "collisions";
        public const string ATTR_PLAYER_SKIN = "player_skin";
        public const string ATTR_PLAYER_RANK = "player_rank";
        public const string ATTR_HEALTH = "health";
        public const string ATTR_AV_SCORE = "av_score";
        public const string ATTR_SCORE_SOURCE = "score_source";
        public const string ATTR_HIGH_SCORE = "high_score";
        public const string ATTR_GOLDEN_NUGGET = "golden_nugget";

        // BUSINESS EVENTS STRINGS
        public const string ATTR_BUSINESS_CURRENCY = "business_currency";
        public const string ATTR_BUSINESS_AMOUNT = "business_amount";
        public const string ATTR_BUSINESS_ITEM_TYPE = "business_item_type";
        public const string ATTR_BUSINESS_ITEM_ID = "business_item_id";
        public const string ATTR_BUSINESS_TRANSACTION_CONTEXT = "business_context";

        // PROGRESSION EVENTS
        public const string ATTR_PROGRESSION_STATUS = "progression_status";
        public const string ATTR_PROGRESSION_PROGRESSION_ID1 = "progression_id1";
        public const string ATTR_PROGRESSION_PROGRESSION_ID2 = "progression_id2";
        public const string ATTR_PROGRESSION_PROGRESSION_ID3 = "progression_id2";
        public const string ATTR_PROGRESSION_PROGRESSION_NUMERIC_VALUE = "progression_int";

        // RESOURCE EVENTS
        public const string ATTR_RESOURCE_CURRENCY = "resource_currency";
        public const string ATTR_RESOURCE_AMOUNT = "resource_amount";
        public const string ATTR_RESOURCE_ITEM_ID = "resource_item_id";
        public const string ATTR_RESOURCE_CONTEXT = "resource_context";
        public const string ATTR_RESOURCE_ITEM_TYPE = "resource_item_type";
        public const string ATTR_RESOURCE_BALANCE = "resource_balance";

        // ----- POSSIBLE STRING VALUES -----

        // SESSION STRINGS
        public const string SESSION_ORIGIN_NEW_SESSION = "new_session";
        public const string SESSION_ORIGIN_FROM_BACKGROUND = "from_background";
        public const string SESSION_REASON_PAUSE = "pause";
        public const string SESSION_REASON_QUIT = "quit";
        public const string SESSION_SCREEN_TUTORIAL = "tutorial";
        public const string SESSION_SCREEN_GAME = "game";
        public const string SESSION_SCREEN_AD = "ad";
        public const string SESSION_SCREEN_SCORE = "score";

        // ADS
        public const string AD_SCREEN_BANNER = "banner";
        public const string AD_SCREEN_REWARDED = "rewarded";
        public const string AD_SCREEN_INTERSTITIAL = "interstitial";

        // MATCH INFO
        public const string MATCH_REASON_START = "start";
        public const string MATCH_REASON_RESTART = "restart";
        public const string MATCH_REASON_REVIVE = "revive";
        public const string MATCH_ORIGIN_TUTORIAL = "tutorial;";
        public const string MATCH_ORIGIN_END_CARD = "end_card";
        public const string MATCH_ORIGIN_GAME = "game";
        public const string MATCH_ORIGIN_MENU = "menu";
        public const string MATCH_END_LOSE = "lose";
        public const string MATCH_END_WIN = "win";
        public const string MATCH_END_QUIT = "quit";
    }
}

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
namespace Lightneer.Ads
{
    public enum AdEventType
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
        INTERSTITIAL_AD_NOT_AVAILABLE,
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
        REWARDED_AD_CLICKED,
        REWARDED_AD_STARTED,
        REWARDED_AD_ENDED,
        REWARDED_AD_REWARDED,
        REWARDED_AD_NOT_AVAILABLE,
        REWARDED_AD_FAILED,
        REWARDED_AD_SHOW_REQUEST,
    }
}

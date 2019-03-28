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
    public delegate void OnRewardedAdResult(RewardedAdResultData rewardInfo);
    public delegate void OnInterstitialLoadResult(bool loadWasSuccesful);
    public delegate void OnInterstitialEnd();

    public abstract class AdsProvider : MonoBehaviour
    {
        public abstract void init(OnAdEventCallback onAdEvent);
        public abstract void showBanner();
        public abstract void hideBanner();
        public abstract void loadInterstitial(OnInterstitialLoadResult callback);
        public abstract void showInterstitial(string placement, OnInterstitialEnd callback);
        public abstract bool isRewardedAdAvailable();
        public abstract void showRewardedAd(string placement, OnRewardedAdResult callback);
    }
}

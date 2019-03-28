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
using System.Collections.Generic;

namespace Lightneer.Ads
{
    public struct RewardedAdResultData
    {
        public string placementName;
        public bool wasRewarded;
        private List<KeyValuePair<string, float>> mRewards;

        public RewardedAdResultData(string placementName, bool wasRewarded)
        {
            this.placementName = placementName;
            this.wasRewarded = wasRewarded;
            mRewards = new List<KeyValuePair<string, float>>();
        }

        public void addReward(string rewardName, float amount)
        {
            mRewards.Add(new KeyValuePair<string, float>(rewardName, amount));
        }

        public IEnumerable<KeyValuePair<string, float>> rewards
        {
            get
            {
                foreach (var r in mRewards)
                {
                    yield return r;
                }
            }
        }
    }
}

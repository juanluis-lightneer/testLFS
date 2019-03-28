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
using System;
using System.Collections;
using System.Collections.Generic;
using AdEventInfo = Lightneer.Ads.AdEventInfo;
using AdEventType = Lightneer.Ads.AdEventType;
using AdType = Lightneer.Ads.AdType;

namespace GameEventSystem
{
    public class OnAdEvent : EventManager.Event
    {
        public AdEventType adEventType
        {
            get { return mAdEventInfo.eventType; }
        }

        public AdType adType
        {
            get { return mAdEventInfo.adType; }
        }

        public DateTime timeStamp { get; private set; }

        private AdEventInfo mAdEventInfo;
        private Dictionary<string, object> mExtraData;

        public OnAdEvent(AdEventInfo adEventInfo, DateTime timeStamp)
        {
            mAdEventInfo = adEventInfo;
            this.timeStamp = timeStamp;
        }

        public IEnumerable<KeyValuePair<string, object>> extraData
        {
            get
            {
                if (mExtraData != null)
                    yield break;
                foreach (var entry in mExtraData)
                {
                    yield return entry;
                }
            }
        }

        public void addData(string key, object obj)
        {
            if (mExtraData == null)
                mExtraData = new Dictionary<string, object>();
            mExtraData.Add(key, obj);
        }
    }
}

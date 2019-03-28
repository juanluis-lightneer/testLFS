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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightneer
{
	public class AppsflyerIntegration : MonoBehaviour
	{
		private static AppsflyerIntegration sInstance;
		public static AppsflyerIntegration instance
		{
			get
			{
				if (sInstance == null)
				{
					sInstance = FindObjectOfType<AppsflyerIntegration>();
				}
				return sInstance;
			}
		}

		public bool isSandbox = false;
		public bool isDebug = false;
		public string androidAppId;
		public string iOSAppId;
		public string devKey;

		public void Awake()
		{
			sInstance = this;			
			Initialize();
		}

		public void Initialize()
		{
			AppsFlyer.setIsDebug(isDebug);
			AppsFlyer.setIsSandbox(isSandbox);
			AppsFlyer.setAppsFlyerKey(devKey);            
#if UNITY_IOS
			AppsFlyer.setAppID(iOSAppId);
			AppsFlyer.trackAppLaunch();
#elif UNITY_ANDROID
            AppsFlyer.setAppID(androidAppId);
			AppsFlyer.init(devKey, "AppsFlyerTrackerCallbacks");
#endif
		    AppsFlyer.trackAppLaunch();
		}
	}
}

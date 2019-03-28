using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluginsIntegration
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
			AppsFlyer.setAppId(iOSAppId);
			AppsFlyer.trackAppLaunch();
#elif UNITY_ANDROID
			AppsFlyer.setAppID(androidAppId);
			AppsFlyer.init(devKey, "AppsFlyerTrackerCallbacks");
#endif

		AppsFlyer.trackAppLaunch();
		}
	}
}
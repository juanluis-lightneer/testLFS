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
using System.Collections.Generic;
using Facebook.Unity;
using GameEventSystem;

namespace Lightneer.Analytics
{
    public class FacebookAnalyticsProvider : AnalyticsProvider
    {
        private bool isActive;
        private List<IAnalyticsEvent> mPendingEvents;

        public override void init()
        {
            isActive = true;
            mPendingEvents = new List<IAnalyticsEvent>();
            facebookInitialization();
            EventManager.Connect<OnAnalyticsGameOver>(onAnalyticsGameOver);
            EventManager.Connect<OnAnalyticsGameStart>(onAnalyticsGameStart);
            EventManager.Connect<OnAnalyticsAdStart>(onAnalyticsAdStart);
            EventManager.Connect<OnAnalyticsAdComplete>(onAnalyticsAdComplete);
            EventManager.Connect<OnAnalyticsSessionStart>(onAnalyticsSessionStart);
            EventManager.Connect<OnAnalyticsSessionComplete>(onAnalyticsSessionComplete);
            EventManager.Connect<OnAnalyticsCustomEvent>(onAnalyticsCustomEvent);
        }

        private void facebookInitialization()
        {
            FacebookInit.instance.initialize(() => { onFacebookInitialized(); });
        }

        private void onFacebookInitialized()
        {
            FB.ActivateApp();
            if (FB.IsInitialized)
            {
                sendPendingMessages();
            }
        }

        private void sendPendingMessages()
        {
            if (mPendingEvents == null)
                return;
            foreach (var e in mPendingEvents)
            {
                sendFacebookEvent(e);
            }
            mPendingEvents.Clear();
        }

        private void sendFacebookEvent(IAnalyticsEvent evt)
        {
            if (!FB.IsInitialized)
            {
                if (mPendingEvents.Count < 50)
                    mPendingEvents.Add(evt);
                return;
            }
            FB.LogAppEvent(evt.eventName, parameters: evt.dataAsDictionary);
        }

        private void onAnalyticsCustomEvent(OnAnalyticsCustomEvent data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart data)
        {
            sendFacebookEvent(data);
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver data)
        {
            sendFacebookEvent(data);
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus && isActive)
            {
                facebookInitialization();
            }
        }
    }
}

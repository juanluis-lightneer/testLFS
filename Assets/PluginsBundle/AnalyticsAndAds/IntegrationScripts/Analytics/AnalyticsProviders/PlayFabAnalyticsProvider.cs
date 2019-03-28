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
using GameEventSystem;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lightneer.Analytics
{
    public class PlayFabAnalyticsProvider : AnalyticsProvider
    {
        public bool enableDebug = false;
        private List<WriteClientPlayerEventRequest> mPendingEvents;

        public override void init()
        {               
            if (!PlayFabManager.instance.isLoggedIn)
            {
                PlayFabManager.instance.onPlayFabLogIn += () => 
                {
                    sendPendingMessages();
                };
            }
            mPendingEvents = new List<WriteClientPlayerEventRequest>();
            EventManager.Connect<OnAnalyticsGameOver>(onAnalyticsGameOver);
            EventManager.Connect<OnAnalyticsGameStart>(onAnalyticsGameStart);
            EventManager.Connect<OnAnalyticsAdStart>(onAnalyticsAdStart);
            EventManager.Connect<OnAnalyticsAdComplete>(onAnalyticsAdComplete);
            EventManager.Connect<OnAnalyticsSessionStart>(onAnalyticsSessionStart);
            EventManager.Connect<OnAnalyticsSessionComplete>(onAnalyticsSessionComplete);
            EventManager.Connect<OnAnalyticsCustomEvent>(onAnalyticsCustomEvent);
        }

        private static WriteClientPlayerEventRequest getPlayerEventRequest(IAnalyticsEvent data)
        {
            return new WriteClientPlayerEventRequest()
            {
                Body = data.dataAsDictionary,
                EventName = data.eventName
            };
        }

        private void sendPendingMessages()
        {
            foreach (var m in mPendingEvents)
            {
                sendPlayFabEvent(m);                
            }
            mPendingEvents.Clear();
        }

        private void onAnalyticsCustomEvent(OnAnalyticsCustomEvent data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsSessionComplete(OnAnalyticsSessionComplete data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsSessionStart(OnAnalyticsSessionStart data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsAdComplete(OnAnalyticsAdComplete data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsAdStart(OnAnalyticsAdStart data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsGameStart(OnAnalyticsGameStart data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void onAnalyticsGameOver(OnAnalyticsGameOver data)
        {
            sendEvent(getPlayerEventRequest(data));
        }

        private void sendEvent(WriteClientPlayerEventRequest request)
        {
            if (!PlayFabManager.instance.isLoggedIn)
            {
                if (mPendingEvents.Count < 50)
                    mPendingEvents.Add(request);
                return;
            }
            sendPlayFabEvent(request);
        }

        private void sendPlayFabEvent(WriteClientPlayerEventRequest request)
        {
            PlayFabClientAPI.WritePlayerEvent(request, result =>
            {
                showDebug("Event send succeeded");
            },
            error =>
            {
                showDebug(error.GenerateErrorReport());
            });
        }

        private void showDebug(string message)
        {
            if (enableDebug)
            {
                Debug.Log("[PlayFabAnalyticsProvider] " + message);
            }
        }
    }
}

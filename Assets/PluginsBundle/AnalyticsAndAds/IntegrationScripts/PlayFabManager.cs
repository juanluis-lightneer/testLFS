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
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

namespace Lightneer
{
    public class PlayFabManager : MonoBehaviour
    {
        public event Action onPlayFabLogIn;

        private static PlayFabManager sInstance;
        public static PlayFabManager instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = FindObjectOfType<PlayFabManager>();
                }
                return sInstance;
            }
        }

        public bool enableDebug = false;

        public Dictionary<string, string> titleData = new Dictionary<string, string>();
        public Dictionary<string, UserDataRecord> userData = new Dictionary<string, UserDataRecord>();

        public bool isLoggedIn { get; private set; }

        public void Awake()
        {
            sInstance = this;
        }
        
        public void Start()
        {
#if UNITY_EDITOR
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucceed, OnError);
#elif UNITY_IOS
            var request = new LoginWithIOSDeviceIDRequest
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true,
                DeviceModel = SystemInfo.deviceModel, OS = SystemInfo.operatingSystem
            };
            PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSucceed, OnError);
#elif UNITY_ANDROID
            var request = new LoginWithAndroidDeviceIDRequest
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true,
                AndroidDevice = SystemInfo.deviceModel, OS = SystemInfo.operatingSystem
            };
            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSucceed, OnError);
#endif
        }

        private void OnLoginSucceed(LoginResult result)
        {
            isLoggedIn = true;
            showDebug(string.Format("Successfully logged in as {0}", result.PlayFabId));
            loadUserData();
            loadTitleData();
            doLogInCallbacks();
        }

        private void doLogInCallbacks()
        {
            if (onPlayFabLogIn != null)
            {
                onPlayFabLogIn();
            }
        }

        private void loadTitleData()
        {
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), (GetTitleDataResult titleDataResult) =>
            {
                titleData = titleDataResult.Data;
            }, OnError);
        }

        private void loadUserData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), (GetUserDataResult userDataResult) =>
            {
                userData = userDataResult.Data;
            }, OnError);
        }

        public void OnError(PlayFabError error)
        {
            showDebug("Error: " + error.GenerateErrorReport());
        }

        private void showDebug(string message)
        {
            if (enableDebug)
            {
                Debug.Log("[PlayFabManager] " + message);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace PluginsIntegration
{
    public class PlayFabManager : MonoBehaviour
    {
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
        
        public Dictionary<string, string> titleData = new Dictionary<string, string>();
        public Dictionary<string, UserDataRecord> userData = new Dictionary<string, UserDataRecord>();

        public void Awake()
        {
            sInstance = this;
        }
        
        public void Start()
        {
#if UNITY_EDITOR
            var request = new LoginWithCustomIDRequest
                {CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucceed, OnError);
#elif UNITY_IOS
        var request = new LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true, DeviceModel = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSucceed, OnError);
        #elif UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true, AndroidDevice =
 SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSucceed, OnError);
        #endif
        }

        public void OnLoginSucceed(LoginResult result)
        {
            Debug.Log(string.Format("PlayFab: Successfully logged in as {0}", result.PlayFabId));

            var request = new GetUserDataRequest();
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
                (GetUserDataResult userDataResult) => { userData = userDataResult.Data; }, OnError);

            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
                (GetTitleDataResult titleDataResult) => { titleData = titleDataResult.Data; }, OnError);
        }

        public void OnError(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}
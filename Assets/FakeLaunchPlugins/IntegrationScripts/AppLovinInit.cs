using UnityEngine;

namespace PluginsIntegration
{
    public class AppLovinInit : MonoBehaviour
    {
        public string sdkKey;

        void Awake()
        {
            AppLovin.SetSdkKey(sdkKey);
            AppLovin.InitializeSdk();
        }
    }
}

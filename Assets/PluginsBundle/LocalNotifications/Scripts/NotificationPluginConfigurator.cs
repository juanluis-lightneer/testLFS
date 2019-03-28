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
using UnityEngine;

namespace Lightneer.Notifications
{
    public class NotificationPluginConfigurator : MonoBehaviour
    {
        [Serializable]
        public struct References
        {
            public NotificationScheduler scheduler;
        }
        public References refs;

        public bool enableEditorLog;
        public string classForAndroidNotifications;
        public string classForIOSNotifications;

        private string notificationStrategyClassString
        {
            get
            {
#if UNITY_ANDROID
                return classForAndroidNotifications;
#else
                return classForIOSNotifications;
#endif
            }
        }

        private INotificationPluginStrategy getStrategy()
        {
            INotificationPluginStrategy strategy = new DummyNotificationPluginStrategy(enableEditorLog);
#if !UNITY_EDITOR
            try
            {                
                var type = Type.GetType(notificationStrategyClassString);
                strategy = (INotificationPluginStrategy)Activator.CreateInstance(type);
                if (enableEditorLog)
                {
                    if (strategy != null)
                        Debug.Log("[NotificationPluginConfigurator] Created instance of " + notificationStrategyClassString);
                    else
                        Debug.LogError("[NotificationPluginConfigurator] Instance of plugin strategy not created");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
#endif
            return strategy;
        }

        public void Start()
        {
            var strategy = getStrategy();
            refs.scheduler.initialize(strategy);
        }
    }
}

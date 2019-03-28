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
#if !UNITY_IOS
using GameEventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UTNotifications;

namespace Lightneer.Notifications
{
    public class UTNotificationsStrategy : INotificationPluginStrategy
    {
        public bool isInitialized
        {
            get;
            private set;
        }

        public void init(Action onInitialized)
        {
            var manager = Manager.Instance;
            if (manager != null)
            {
                subscribeToManagerEvents(manager);
                initializeUTNotifications(onInitialized, manager);
            }
            else
            {
                isInitialized = false;
                Debug.LogError("[LocalNotifications] UTNotifications manager not present");
            }
        }

        private void initializeUTNotifications(Action onInitialized, Manager manager)
        {
            manager.OnInitialized += () =>
            {
                isInitialized = true;
                Manager.Instance.SetBadge(0);
                if (onInitialized != null)
                    onInitialized();
            };
            manager.Initialize(true, 0, false);
        }

        private void subscribeToManagerEvents(Manager manager)
        {
        
        }


        public void onDestroy()
        {
        }

        public void cancel(int id)
        {
            Manager.Instance.CancelNotification(id);
        }

        public void cancelAll()
        {
            Manager.Instance.CancelAllNotifications();
            Manager.Instance.SetBadge(0);
        }

        public void scheduleLocalNotification(LocalNotificationData notificationData)
        {
            if (isInitialized)
            {
                if (!notificationData.isRepeatingNotification)
                {
                    Manager.Instance.ScheduleNotification(notificationData.triggerInHours.hoursToSeconds(),
                        notificationData.title, notificationData.text, notificationData.id,
                        notificationProfile: notificationData.profile, badgeNumber: notificationData.badge);
                }
                else
                {
                    Manager.Instance.ScheduleNotificationRepeating(notificationData.triggerInHours.hoursToSeconds(),              
                        notificationData.intervalInHours.hoursToSeconds(), notificationData.title, notificationData.text,
                        notificationData.id, notificationProfile: notificationData.profile, badgeNumber: notificationData.badge);
                }
                var timeSpan = TimeExtensions.hoursToTimeSpan(notificationData.triggerInHours);
                notificationData.utcScheduledTime = TimeExtensions.currentUtcTime.Add(timeSpan);
            }
        }

        public int getBadge()
        {
            return Manager.Instance.GetBadge();
        }

        public void setBadge(int number)
        {
            Manager.Instance.SetBadge(number);
        }

        public void onApplicationPause(bool pause)
        {
            
        }
    }
}
#endif

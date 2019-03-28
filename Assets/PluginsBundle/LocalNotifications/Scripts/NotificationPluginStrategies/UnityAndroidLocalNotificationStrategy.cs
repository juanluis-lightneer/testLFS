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
#if UNITY_ANDROID && USE_UNITY_NOTIFICATIONS_PACKAGE
using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace Lightneer.Notifications
{
    public class UnityAndroidLocalNotificationStrategy : INotificationPluginStrategy
    {
        public bool isInitialized { get { return true; } }

        private NotificationIdEquivalence notificationEquivalences = new NotificationIdEquivalence();

        public string channelId
        {
            get { return Application.productName; }
        }

        public void init(Action onInitialized)
        {
            registerNotificationChannel();
            if (onInitialized != null)
                onInitialized();
        }
        
        private void registerNotificationChannel()
        {
            var c = new AndroidNotificationChannel()
            {
                Id = channelId,
                Name = Application.productName,
                Importance = Importance.Default,
                Description = Application.productName + " notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(c);
        }

        public void cancel(int id)
        {
            var externalId = notificationEquivalences.getExternalId(id);
            if (externalId >= 0)
            {
                notificationEquivalences.removeEquivalenceByOwnId(id);
                AndroidNotificationCenter.CancelScheduledNotification(externalId);
            }
        }

        public void cancelAll()
        {
            notificationEquivalences.clearEquivalences();
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        public int getBadge()
        {
            return 0;
        }

        public void onApplicationPause(bool pause) { }

        public void onDestroy() { }

        public void scheduleLocalNotification(LocalNotificationData notificationData)
        {
            var notification = createNotification(notificationData);
            var externalId = AndroidNotificationCenter.SendNotification(notification, channelId);
            notificationEquivalences.addEquivalence(notificationData.id, externalId);
            updateScheduledTime(notificationData);
        }

        private static void updateScheduledTime(LocalNotificationData notificationData)
        {
            var timeSpan = TimeExtensions.hoursToTimeSpan(notificationData.triggerInHours);
            notificationData.utcScheduledTime = TimeExtensions.currentUtcTime.Add(timeSpan);
        }

        private static AndroidNotification createNotification(LocalNotificationData notificationData)
        {
            var notification = new AndroidNotification();
            notification.Title = notificationData.title;
            notification.Text = notificationData.text;
            notification.FireTime = System.DateTime.Now.AddHours(notificationData.triggerInHours);
            if (notificationData.isRepeatingNotification)
            {
                notification.RepeatInterval = TimeSpan.FromHours(notificationData.intervalInHours);
            }
            return notification;
        }

        public void setBadge(int number)
        {
            
        }
    }
}

#endif

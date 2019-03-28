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
#if UNITY_IOS && USE_UNITY_NOTIFICATIONS_PACKAGE
using System;
using System.Collections;
using System.Collections.Generic;
using UniQue;
using Unity.Notifications.iOS;
using UnityEngine;

namespace Lightneer.Notifications
{
    public class UnityIOSLocalNotificationStrategy : INotificationPluginStrategy
    {
        public bool isInitialized { get; private set; }
        
        public void init(Action onInitialized)
        {
            Coroutines.start(initEnumerator(onInitialized));
        }

        IEnumerator initEnumerator(Action onInitialized)
        {
            using (var req = new AuthorizationRequest(AuthorizationOption.AuthorizationOptionAlert | AuthorizationOption.AuthorizationOptionBadge, false))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string res = "\n RequestAuthorization: \n";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(res);
                isInitialized = req.Granted;
                if (onInitialized != null)
                    onInitialized();
            }
        }

        public void cancel(int id)
        {
            if (!isInitialized)
                return;
            iOSNotificationCenter.RemoveScheduledNotification(id.ToString());
            iOSNotificationCenter.RemoveDeliveredNotification(id.ToString());
        }

        public void cancelAll()
        {
            if (!isInitialized)
                return;
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public int getBadge()
        {
            return 0;
        }
        
        public void onApplicationPause(bool pause)
        {
        }

        public void onDestroy()
        {
            
        }

        public void scheduleLocalNotification(LocalNotificationData notificationData)
        {
            if (!isInitialized)
                return;
            var timeSpan = TimeExtensions.hoursToTimeSpan(notificationData.triggerInHours);
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = timeSpan,
                Repeats = notificationData.isRepeatingNotification
            };            
            notificationData.utcScheduledTime = TimeExtensions.currentUtcTime.Add(timeSpan);
            iOSNotification notification = createNotification(notificationData, timeTrigger);
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void setBadge(int number)
        {
            
        }

        private static iOSNotification createNotification(LocalNotificationData notificationData, iOSNotificationTimeIntervalTrigger timeTrigger)
        {
            var notification = new iOSNotification()
            {
                // You can optionally specify a custom Identifier which can later be 
                // used to cancel the notification, if you don't set one, an unique 
                // string will be generated automatically.
                Identifier = notificationData.id.ToString(),
                Title = notificationData.title,
                Body = notificationData.text,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.NotificationPresentationOptionAlert,
                CategoryIdentifier = Application.productName,
                //ThreadIdentifier = thread,
                Trigger = timeTrigger,
            };
            return notification;
        }
    }
}
#endif

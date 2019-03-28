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

#if UNITY_IOS
using GameEventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using LocalNotification = UnityEngine.iOS.LocalNotification;

namespace Lightneer.Notifications
{
    public class UnityOldIOSLocalNotificationStrategy : INotificationPluginStrategy
    {
        private const char DELIMITER_CHARACTER = '.';
        private const string NOTIFICATION_KEYWORD = "Notification";
        private const string ID_KEY = "id";
        private const string UTC_DATE_KEY = "utc_date";

        public bool isInitialized { get { return true; } }

        private static void registerForNotifications()
        {
#if USE_UNITY_NATIVE_IOS_NOTIFICATIONS
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
                                                                          UnityEngine.iOS.NotificationType.Badge |
                                                                          UnityEngine.iOS.NotificationType.Sound);
#endif
        }

        private static void unregisterForNotifications()
        {
            UnityEngine.iOS.NotificationServices.UnregisterForRemoteNotifications();
        }

        private static LocalNotification getLatestClickedNotification()
        {
            var lastItemIndex = UnityEngine.iOS.NotificationServices.localNotificationCount - 1;
            return UnityEngine.iOS.NotificationServices.localNotifications[lastItemIndex];
        }
        
        public void init(Action onInitialized)
        {
            registerForNotifications();
            if (onInitialized != null)
                onInitialized();
        }

        public void onDestroy()
        {
            unregisterForNotifications();
        }

        public void onApplicationPause(bool pause)
        {
            if (pause)
            {
                unregisterForNotifications();
            }
            else
            {
                notifyClickedNotification();
                registerForNotifications();
                UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
            }
        }

        private void notifyClickedNotification()
        {
            if (UnityEngine.iOS.NotificationServices.localNotificationCount < 1)
                return;
            int tmpAppNotificationId = -1;
            var latestClickedNotification = getLatestClickedNotification();
            if (tryGetInAppIdFromIOSNotification(latestClickedNotification, out tmpAppNotificationId))
            {
                EventManager.Send(new OnNotificationClicked(tmpAppNotificationId));
            }
        }
                
        public void scheduleLocalNotification(LocalNotificationData notificationData)
        {
            schedule(notificationData);
        }

        private void schedule(LocalNotificationData notificationData)
        {
            var notification = getIOSLocalNotification(notificationData);
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
            var dateTime = getIosUtcDateTime(notification);
            notificationData.utcScheduledTime = dateTime;
        }

        public void cancelAll()
        {
            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        }

        public void cancel(int idToCancel)
        {
            string tmpNotificationPhoneId;
            var iPhoneIdToCancel = localIdToPhoneId(idToCancel);
            foreach (var ln in UnityEngine.iOS.NotificationServices.scheduledLocalNotifications)
            {
                if (tryGetIOSNotificationPhoneId(ln, out tmpNotificationPhoneId))
                {
                    if (iPhoneIdToCancel == tmpNotificationPhoneId)
                    {
                        UnityEngine.iOS.NotificationServices.CancelLocalNotification(ln);
                    }
                }
            }
        }

        private bool tryGetIOSNotificationPhoneId(LocalNotification iOSNotification, out string phoneId)
        {
            phoneId = null;
            var userInfo = iOSNotification.userInfo;            
            if (userInfo != null && userInfo.Contains(ID_KEY))
            {
                phoneId = (string)userInfo[ID_KEY];
                return true;
            }
            return false;
        }

        private DateTime getIosUtcDateTime(LocalNotification iOSNotification)
        {
            var userInfo = iOSNotification.userInfo;
            if (userInfo != null && userInfo.Contains(UTC_DATE_KEY))
            {
                return TimeExtensions.utcDateTimeFromFileTimeString((string)userInfo[UTC_DATE_KEY], DateTime.MinValue);
            }
            return DateTime.MinValue;
        }

        private bool tryGetInAppIdFromIOSNotification(LocalNotification iOSNotification, out int appID)
        {
            appID = -1;
            string phoneId;
            if (tryGetIOSNotificationPhoneId(iOSNotification, out phoneId))
            {
                appID = phoneIdToLocalId(phoneId);
                return true;
            }
            return false;
        }

        public int getBadge()
        {
            // Not used at the moment
            return 0;
        }
        
        public void setBadge(int number)
        {
            // Not used at the moment
        }
        
        public LocalNotification getIOSLocalNotification(LocalNotificationData notification)
        {
            LocalNotification newNotification = new LocalNotification();
            newNotification.alertAction = notification.title;
            newNotification.alertBody = notification.text;
            newNotification.fireDate = notification.triggerInHours.localDateTimeAfterThisHours();
            if (notification.isRepeatingNotification)
            {
                newNotification.repeatInterval = IosUtils.convertToCalendarUnit(notification.intervalInHours);
            }
            // TODO: should it accept actions?
            newNotification.hasAction = false;
            newNotification.applicationIconBadgeNumber = notification.badge;

            string notificationID = localIdToPhoneId(notification.id);
            Dictionary<string, string> userInfo = new Dictionary<string, string>(1);
            userInfo[ID_KEY] = notificationID;
            userInfo[UTC_DATE_KEY] = notification.triggerInHours.utcDateTimeAfterThisHours().utcToFileTimeString();
            newNotification.userInfo = userInfo;

            return newNotification;
        }

        private static string localIdToPhoneId(int id)
        {
            return Application.productName + DELIMITER_CHARACTER + NOTIFICATION_KEYWORD + DELIMITER_CHARACTER + id.ToString();
        }               

        private static int phoneIdToLocalId(string phoneId)
        {
            int parsed = -1;
            var split = phoneId.Split(DELIMITER_CHARACTER);
            if (split.Length == 3)
            {
                int.TryParse(split[2], out parsed);
            }
            return parsed;
        }
    }
}
#endif

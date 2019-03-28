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
using System.Collections.Generic;
using UnityEngine;

namespace Lightneer.Notifications
{
    public class NotificationsAccessor : MonoBehaviour
    {
        public float maxSecondsFromNotificationToOpen = 10;

        public bool debugEnabled;
        [SerializeField]
        public NotificationStorage notificationStorage;

        public IEnumerable<LocalNotificationData> notificationsToBeScheduled
        {
            get
            {
                foreach (var n in notificationStorage.allNotifications)
                {
                    if (n.shouldBeScheduled)
                        yield return n;
                }
            }
        }
        
        public void checkReceivedNotifications()
        {
            foreach (var n in notificationStorage.allNotifications)
            {
                if (n.hasNotificationDate && n.utcScheduledTime.isUtcDateInThePast())
                {
                    showDebug("notification.onNotificationNotified() " + n.id);                    
                    n.onNotificationNotified();
                }
            }
        }

        public void checkClickedNotifications()
        {
            foreach (var n in notificationStorage.allNotifications)
            {
                if (notificationHappenedJustBeforeOpen(n))
                {
                    showDebug("notification clicked-> " + n.id);
                    EventManager.Send(new OnNotificationClicked(n.id));
                }
            }
        }

        private bool notificationHappenedJustBeforeOpen(LocalNotificationData n)
        {
            if (!n.hasNotificationDate)
                return false;
            var secondsSinceScheduleTime = (float)(TimeExtensions.currentUtcTime - n.utcScheduledTime).TotalSeconds;
            return secondsSinceScheduleTime < maxSecondsFromNotificationToOpen && secondsSinceScheduleTime >= 0.0f;
        }

        public void cancelNotification(int id)
        {
            var notification = notificationStorage.getNotificationByID(id);
            if (notification != null)
            {
                notification.cancel();
            }
        }

        public void cancelAllNotifications()
        {
            foreach (var n in notificationStorage.allNotifications)
            {
                n.cancel();
            }
        }

        public void setNotificationState(int id, bool enabled)
        {
            foreach (var n in notificationStorage.allNotifications)
            {
                if (n.id == id)
                {
                    showDebug("setNotificationState: id " + n.id + " to " + enabled);
                    n.isEnabled = enabled;
                    return;
                }
            }
        }

        private void showDebug(string debugStr)
        {
            if (!debugEnabled)
                return;
            Debug.Log("[NotificationsKeeper] " + debugStr);
        }
    }
}

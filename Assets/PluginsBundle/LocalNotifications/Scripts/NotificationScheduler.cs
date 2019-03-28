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
using UniQue;
using UnityEngine;

namespace Lightneer.Notifications
{
    public class NotificationScheduler : SingletonMonoBehaviour<NotificationScheduler>
    {
        [Serializable]
        public struct References
        {
            public NotificationsAccessor notificationsAccessor;
        }
        public References refs;

        private bool mDidAppInitialize;
        private bool mIsInitialized;
        private INotificationPluginStrategy mNotificationStrategy;

        public void initialize(INotificationPluginStrategy notificationsPluginStrategy)
        {
            mIsInitialized = false;
            mNotificationStrategy = notificationsPluginStrategy;
            mNotificationStrategy.init(onNotificationsInitialized);
        }

        public void setNotificationState(int notificationId, bool notificationState)
        {
            refs.notificationsAccessor.setNotificationState(notificationId, notificationState);
        }

        private void onNotificationsInitialized()
        {
            mIsInitialized = true;
            cancellAllNotifications();
            mDidAppInitialize = true;
        }

        private void cancellAllNotifications()
        {
            mNotificationStrategy.cancelAll();
        }

        private void scheduleNotifications()
        {
            foreach (var n in refs.notificationsAccessor.notificationsToBeScheduled)
            {
                mNotificationStrategy.scheduleLocalNotification(n);
            }
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            scheduleNotifications();
        }
#endif
        private void OnApplicationPause(bool pause)
        {
#if UNITY_EDITOR
            if (Application.isEditor)
                return;
#endif
            if (!mDidAppInitialize)
                return;
            if (!pause)
            {
                refs.notificationsAccessor.checkReceivedNotifications();
                refs.notificationsAccessor.cancelAllNotifications();
                mNotificationStrategy.onApplicationPause(pause);
                cancellAllNotifications();
            }
            else
            {
                scheduleNotifications();
                mNotificationStrategy.onApplicationPause(pause);
            }

        }
    }
}

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
    public class DummyNotificationPluginStrategy : INotificationPluginStrategy
    {
        public bool isInitialized { get { return true; } }
                
        private int mBadge;
        private bool mDebug;

        public DummyNotificationPluginStrategy(bool debug)
        {
            mDebug = debug;
        }

        public void cancel(int id)
        {
            if (mDebug)
            {
                Debug.Log("cancel notification " + id);
            }
        }

        public void cancelAll()
        {
            if (mDebug)
            {
                Debug.Log("cancel all notifications");
            }
        }
        
        public int getBadge()
        {
            return mBadge;
        }

        public void init(Action onInitialized)
        {
            if (mDebug)
            {
                Debug.Log("initialize notifications");
            }
            if (onInitialized != null)
            {
                onInitialized();
            }
        }

        public void onDestroy()
        {
            if (mDebug)
            {
                Debug.Log("onDestroy notifications");
            }
        }

        public void scheduleLocalNotification(LocalNotificationData notificationData)
        {
            if (mDebug)
            {
                string repeating = notificationData.isRepeatingNotification ? "(repeating)" : "";
                Debug.Log("schedule local notification: " + notificationData.id + " - " + notificationData.title + " " + repeating);
            }
        }

        public void setBadge(int number)
        {
            if (mDebug)
            {
                Debug.Log("set badge for notifications: " + number);
            }
            mBadge = number;
        }

        public void onApplicationPause(bool pause)
        {
            
        }
    }
}

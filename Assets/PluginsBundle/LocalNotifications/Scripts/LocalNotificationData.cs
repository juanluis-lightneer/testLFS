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
    [Serializable]
    public class LocalNotificationData
    {
        public const string PLAYER_PREFS_KEY_PREFIX = "LocalNotification_";
        public const string PLAYER_PREFS_NOTIFICATION_COUNT = "NotificationCount_";
        public const string PLAYER_PREFS_STATE = "State_";
        public const string PLAYER_PREFS_SCHEDULE_TIME = "ScheduleTime_";

        [Tooltip("Unique ID")]
        public int id;
        public string title;
        public string text;
        [Tooltip("Used for UTNotifications profile")]
        public string profile;
        [Tooltip("Not in use at the moment")]
        public int badge;
        [Tooltip("Hours since the app is closed to the moment notification shows up")]
        public float triggerInHours;
        [Tooltip("Makes a notification repeat indefinitely every X hours. 0 means no interval")]
        public float intervalInHours;
        [SerializeField]
        private bool enabled;
        [Tooltip("How many times will be notified. 0 means forever")]
        [SerializeField]
        private int numberOfTimesToBeNotified;

        public LocalNotificationData() { }

        public LocalNotificationData(int id, string title, string text, float triggerInHours, float intervalInHours)
        {
            this.id = id;
            this.title = title;
            this.text = text;
            this.triggerInHours = triggerInHours;
            this.intervalInHours = intervalInHours;
        }

        public void onNotificationNotified()
        {
            if (hasLimitedTimesOfNotification)
                timesNotified += 1;
            removeNotificationDate();
        }

        public bool isEnabled
        {
            get
            {
                if (PlayerPrefs.HasKey(playerPrefsStateKey))
                {
                    return savedState;
                }
                return enabled;
            }
            set { savedState = value; }
        }

        public bool shouldBeScheduled
        {
            get
            {
                return (!hasLimitedTimesOfNotification || (timesNotified < numberOfTimesToBeNotified)) && isEnabled;
            }
        }

        /// <summary>
        /// FIXME: this field is only used for IOS local notifications strategy. Refactor?
        /// </summary>
        public DateTime utcScheduledTime
        {
            get
            {
                if (hasNotificationDate)
                {
                    var longString = PlayerPrefs.GetString(playerPrefsScheduledTime);
                    var date = TimeExtensions.utcDateTimeFromFileTimeString(longString, DateTime.MinValue);
                    return date;
                }
                return DateTime.MinValue;
            }
            set
            {
                PlayerPrefs.SetString(playerPrefsScheduledTime, value.utcToFileTimeString()); 
                PlayerPrefs.Save();
            }
        }

        public bool hasNotificationDate
        {
            get { return PlayerPrefs.HasKey(playerPrefsScheduledTime); }
        }

        private bool savedState
        {
            get { return (PlayerPrefs.GetInt(playerPrefsStateKey) == 1); }
            set
            {
                if (!value)
                {
                    removeNotificationDate();
                }
                PlayerPrefs.SetInt(playerPrefsStateKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        private string playerPrefsNotificationCountKey
        {
            get { return PLAYER_PREFS_KEY_PREFIX + PLAYER_PREFS_NOTIFICATION_COUNT + id; }
        }

        private string playerPrefsStateKey
        {
            get { return PLAYER_PREFS_KEY_PREFIX + PLAYER_PREFS_STATE + id; }
        }

        private string playerPrefsScheduledTime
        {
            get { return PLAYER_PREFS_KEY_PREFIX + PLAYER_PREFS_SCHEDULE_TIME + id; }
        }

        private int timesNotified
        {
            get { return PlayerPrefs.GetInt(playerPrefsNotificationCountKey); }
            set
            {
                PlayerPrefs.SetInt(playerPrefsNotificationCountKey, value);
                PlayerPrefs.Save();
            }
        }

        private bool hasLimitedTimesOfNotification
        {
            get { return (numberOfTimesToBeNotified > 0); }
        }

        public void cancel()
        {
            removeNotificationDate();
        }

        private void removeNotificationDate()
        {
            if (hasNotificationDate)
            {
                PlayerPrefs.DeleteKey(playerPrefsScheduledTime);
                PlayerPrefs.Save();
            }
        }

        public bool isRepeatingNotification
        {
            get { return intervalInHours > 0; }
        }
    }
}

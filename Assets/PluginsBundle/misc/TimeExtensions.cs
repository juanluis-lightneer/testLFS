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

public static class TimeExtensions
{
    public const float SECONDS_IN_AN_HOUR = 3600.0f;

    public static int hoursToSeconds(this float hours)
    {
        return (int)(hours * SECONDS_IN_AN_HOUR);
    }

    public static float hoursUntilUtcDateFromNow(this DateTime utcDateTime)
    {
        return (float)(utcDateTime - currentUtcTime).TotalHours;
    }

    public static float differenceInMinutesFrom(this DateTime time, DateTime fromTime)
    {
        return (float)time.Subtract(fromTime).TotalMinutes;
    }

    public static DateTime utcDateTimeAfterThisHours(this float hours)
    {
        return currentUtcTime.AddHours((double)hours);
    }

    public static DateTime localDateTimeAfterThisHours(this float hours)
    {
        return currentLocalTime.AddHours((double)hours);
    }

    public static bool isUtcDateInThePast(this DateTime utcDateTime)
    {
        return (utcDateTime.CompareTo(currentUtcTime) < 0);
    }

    public static string utcToFileTimeString(this DateTime dateTime)
    {
        return dateTime.ToFileTimeUtc().ToString();
    }

    public static DateTime utcDateTimeFromFileTimeString(string stringContainingALong, DateTime defaultDateTime)
    {
        long validValue;
        if (long.TryParse(stringContainingALong, out validValue))
            return DateTime.FromFileTimeUtc(validValue);
        return defaultDateTime;
    }

    public static DateTime currentLocalTime
    {
        get { return DateTime.Now; }
    }

    public static DateTime currentUtcTime
    {
        get { return DateTime.UtcNow; }
    }

    public static TimeSpan hoursToTimeSpan(float triggerInHours)
    {
        int intHours = (int)triggerInHours;
        int intMinutes = 0;
        int intSeconds = 0;
        triggerInHours -= intHours;
        if (triggerInHours > 0.0f)
        {
            float triggerInMinutes = triggerInHours * 60.0f;
            intMinutes = (int)triggerInMinutes;
            triggerInMinutes -= intMinutes;
            if (triggerInMinutes > 0.0f)
            {
                intSeconds = (int)(triggerInMinutes * 60.0f);
            }
        }
        return new TimeSpan(intHours, intMinutes, intSeconds);
    }
}

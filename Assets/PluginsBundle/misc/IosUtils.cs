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
using UnityEngine.iOS;

namespace Lightneer.Notifications
{
    public static class IosUtils
    {
        public static CalendarUnit convertToCalendarUnit(float hours)
        {
            CalendarUnit unit = CalendarUnit.Year;
            if (hours < 24)
            {
                unit = CalendarUnit.Hour;
            }
            else if (hours >= 24 && hours < 48)
            {
                unit = CalendarUnit.Day;
            }
            else if (hours >= 48 && hours < 168)
            {
                unit = CalendarUnit.Weekday;
            }
            else if (hours >= 168 && hours < 672)
            {
                unit = CalendarUnit.Week;
            }
            else if (hours >= 672 && hours <= 744)
            {
                unit = CalendarUnit.Month;
            }
            else if (hours > 744 && hours <= 2208)
            {
                unit = CalendarUnit.Quarter;
            }
            return unit;
        }
    }
}
#endif

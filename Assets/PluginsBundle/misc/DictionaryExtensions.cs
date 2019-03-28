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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public static class DictionaryExtensions
{
    public static Dictionary<string, string> toStringValuedDictionary(this IDictionary<string, object> originalDictionary)
    {
        var dict = new Dictionary<string, string>();
        foreach (var k in originalDictionary.Keys)
        {
            dict.Add(k, originalDictionary[k].ToString());
        }
        return dict;
    }

    public static Dictionary<string, object> toDictionary(this ReadOnlyDictionary<string, object> readonlyDicitonary)
    {
        return readonlyDicitonary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}

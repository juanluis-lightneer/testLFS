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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneInstanceOfThisPrefab : MonoBehaviour
{
    private static Dictionary<string, GameObject> mSavedPrefabs;
    public static Dictionary<string, GameObject> savedPrefabs
    {
        get
        {
            if (mSavedPrefabs == null)
                mSavedPrefabs = new Dictionary<string, GameObject>();
            return mSavedPrefabs;
        }
    }

    public string prefabName;

    private void Awake()
    {
        if (savedPrefabs.ContainsKey(prefabName))
        {
            Destroy(gameObject);
        }
        else
        {
            savedPrefabs.Add(prefabName, gameObject);
            enableChildren();
        }
    }

    private void enableChildren()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}

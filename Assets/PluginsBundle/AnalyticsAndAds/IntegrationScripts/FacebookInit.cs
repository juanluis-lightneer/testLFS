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
using Facebook.Unity;
using System;
using UnityEngine;

/// <summary>
/// This class initializes facebook for automatic events. It does automatically
/// or on demand.
/// </summary>
public class FacebookInit : MonoBehaviour
{
    private static FacebookInit sInstance;
    public static FacebookInit instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = FindObjectOfType<FacebookInit>();
                if (sInstance == null)
                {
                    var go = new GameObject("FacebookInit");
                    sInstance = go.AddComponent<FacebookInit>();
                    go.AddComponent<DontDestroyOnLoad>();
                }
            }
            return sInstance;
        }
        private set { sInstance = value; }
    }

    private bool isInitializing = false;
    private event Action onInitialized;

    void Awake()
    {
        sInstance = this;
    }

	void Start ()
    {
        initialize(null);
    }

    public void initialize(Action onInitialized)
    {
        if (FB.IsInitialized)
        {
            if (onInitialized != null)
                onInitialized();
            return;
        }
        if (onInitialized != null)
        {
            this.onInitialized += onInitialized;
        }
        if (!isInitializing)
        {
            isInitializing = true;
            FB.Init(() =>
            {
                isInitializing = false;
                doCallbacks();
            });
        }
    }

    private void doCallbacks()
    {
        if (onInitialized != null)
        {
            onInitialized();
        }
    }
}

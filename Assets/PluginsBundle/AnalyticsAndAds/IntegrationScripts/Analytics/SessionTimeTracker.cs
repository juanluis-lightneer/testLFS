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
using System.Collections.Generic;
using UniQue;
using UnityEngine;

namespace Lightneer.Analytics
{
    public class SessionTimeTracker : SingletonMonoBehaviour<SessionTimeTracker>
    {
        public override void Awake()
        {
            base.Awake();
        }

        public float sessionTime
        {
            get { return Time.realtimeSinceStartup; }
        }
    }
}

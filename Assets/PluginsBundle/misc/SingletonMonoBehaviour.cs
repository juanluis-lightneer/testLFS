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
//-----------------------------------------------------------------------------------------------------------
//
// SingletonMonoBehaviour.cs
//
// Base class for singleton classes which also derive from MonoBehaviour.
// The singleton is represented in the scene so that it can override functions like Start, Update and more.
// The singleton can also be viewed in the editor to help debug.
// 
//

using UnityEngine;

namespace UniQue
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public enum DestroyStrategy
        {
            COMPONENT,
            GAMEOBJECT,
        }

        private static T msInstance = null;

        private static bool applicationQuit = false;

        public static T instance
        {
            get
            {
                if (msInstance == null && !applicationQuit)
                {
                    GameObject obj = new GameObject();
                    obj.name = "<" + typeof(T).ToString() + ">";
                    msInstance = obj.AddComponent<T>();

                    if (msInstance.isPersistent())
                    {
                        GameObject.DontDestroyOnLoad(obj);
                    }
                }

                return msInstance;
            }
        }

        public virtual void Awake()
        {
            if (msInstance == null)
            {
                msInstance = (T)this;
            }
            else if (msInstance != null && msInstance != this)
            {
                if (multipleInstanceDestroyStrategy == DestroyStrategy.GAMEOBJECT)
                {
                    Destroy(gameObject);
                }
                else if (multipleInstanceDestroyStrategy == DestroyStrategy.COMPONENT)
                {
                    Destroy(this);
                }
            }
        }

        public virtual void OnDestroy()
        {
            if (msInstance == this)
            {
                msInstance = null;
            }
        }

        protected virtual bool isPersistent()
        {
            return true;
        }

        protected virtual DestroyStrategy multipleInstanceDestroyStrategy
        {
            get
            {
                return DestroyStrategy.COMPONENT;
            }
        }

        void OnApplicationQuit()
        {
            applicationQuit = true;
        }
    }
}

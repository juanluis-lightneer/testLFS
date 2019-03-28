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
using UnityEngine;

namespace Lightneer.Misc
{    
    public class JsonListContainer<T> : IJsonContainer
    {
        [Serializable]
        public class ListContainer<F>
        {
            public List<F> list;

            public ListContainer()
            {
                list = new List<F>();
            }
        }

        [SerializeField]
        private ListContainer<T> container;
        public List<T> list
        {
            get { return container.list; }
        }

        public JsonListContainer()
        {
            container = new ListContainer<T>();
        }

        public JsonListContainer(string json)
        {
            initFromJson(json);
        }

        public void initFromJson(string json)
        {
            container = JsonUtility.FromJson<ListContainer<T>>(json);
        }

        public string serialize()
        {
            return JsonUtility.ToJson(container);
        }
    }
}

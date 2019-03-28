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
using Lightneer.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lightneer.Notifications
{
    public class NotificationIdEquivalence
    {
        [Serializable]
        public struct IdEquivalence
        {
            public int externalId;
            public int ownId;
        }

        public const string NOTIFICATIONS_KEY = "NotificationEquivalences";

        private JsonListContainer<IdEquivalence> mEquivalences;
        public List<IdEquivalence> equivalences
        {
            get
            {
                if (mEquivalences == null)
                {
                    initEquivalences();
                }
                return mEquivalences.list;
            }
        }
        
        private string serializedEquivalences
        {
            get { return mEquivalences.serialize(); }
        }

        private void initEquivalences()
        {
            if (PlayerPrefs.HasKey(NOTIFICATIONS_KEY))
            {
                var jsonString = PlayerPrefs.GetString(NOTIFICATIONS_KEY);
                mEquivalences = new JsonListContainer<IdEquivalence>(jsonString);
            }
            else
            {
                mEquivalences = new JsonListContainer<IdEquivalence>();
            }
        }

        public int getExternalId(int ownId)
        {
            int index = 0;
            if (findIndexByOwnId(ownId, out index))
            {
                return equivalences[index].externalId;
            }
            return -1;
        }

        public int getOwnId(int externalId)
        {
            int index = 0;
            if (findIndexByExternalId(externalId, out index))
            {
                return equivalences[index].ownId;
            }
            return -1;
        }

        public void addEquivalence(int ownId, int externalId)
        {
            int index = 0;
            if (findIndexByExternalId(externalId, out index))
            {
                var eq = equivalences[index];
                eq.ownId = ownId;
                equivalences[index] = eq;
            }
            else if (findIndexByOwnId(ownId, out index))
            {
                var eq = equivalences[index];
                eq.externalId = externalId;
                equivalences[index] = eq;
            }
            else
            {
                var newEquivalence = new IdEquivalence() { ownId = ownId, externalId = externalId };
                equivalences.Add(newEquivalence);
            }
            saveToPlayerPrefs();
        }

        public void removeEquivalenceByOwnId(int ownId)
        {
            int index = 0;
            if (findIndexByOwnId(ownId, out index))
            {
                equivalences.RemoveAt(index);
            }
        }

        public void removeEquivalenceByExternalId(int externalId)
        {
            int index = 0;
            if (findIndexByExternalId(externalId, out index))
            {
                equivalences.RemoveAt(index);
            }
        }

        public void clearEquivalences()
        {
            equivalences.Clear();
            saveToPlayerPrefs();
        }

        private bool findIndexByOwnId(int ownId, out int index)
        {
            for (int i = 0; i < equivalences.Count; i++)
            {
                if (equivalences[i].ownId == ownId)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        private bool findIndexByExternalId(int externalId, out int index)
        {
            for (int i = 0; i < equivalences.Count; i++)
            {
                if (equivalences[i].externalId == externalId)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        private void saveToPlayerPrefs()
        {
            PlayerPrefs.SetString(NOTIFICATIONS_KEY, serializedEquivalences);
            PlayerPrefs.Save();
        }
    }
}

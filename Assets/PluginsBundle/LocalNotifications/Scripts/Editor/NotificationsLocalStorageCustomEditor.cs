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
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Lightneer.Notifications
{
    [CustomEditor(typeof(NotificationLocalStorage))]
    public class NotificationLocalStorageCustomEditor : Editor
    {
        private ReorderableList reorderableList;
        private static List<bool> mOpenElements;

        private NotificationLocalStorage notificationsLocalStorage
        {
            get { return target as NotificationLocalStorage; }
        }
        
        private void OnEnable()
        {
            reorderableList = new ReorderableList(serializedObject, 
                                                  serializedObject.FindProperty("notifications"), 
                                                  true, true, true, true);
            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;
            reorderableList.onAddCallback += AddItem;
            reorderableList.onRemoveCallback += RemoveItem;
            reorderableList.elementHeightCallback += onHeightCallback;
            initOpenElements();
        }

        private void OnDisable()
        {
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;
            reorderableList.onAddCallback -= AddItem;
            reorderableList.onRemoveCallback -= RemoveItem;
            reorderableList.elementHeightCallback -= onHeightCallback;
            reorderableList = null;
            deInitOpenElements();
        }

        private void initOpenElements()
        {
            if (mOpenElements != null)
                return;
            mOpenElements = new List<bool>();
            for (int i = 0; i < reorderableList.count; ++i)
            {
                mOpenElements.Add(false);
            }
        }

        private void deInitOpenElements()
        {
            if (mOpenElements == null)
                return;
            mOpenElements.Clear();
            mOpenElements = null;
        }

        private bool isElementOpen(int index)
        {
            if (mOpenElements == null || mOpenElements.Count <= index)
            {                
                return false;
            }
            return mOpenElements[index];
        }

        private void setElementOpen(int index, bool state)
        {
            if (mOpenElements == null || mOpenElements.Count <= index)
            {
                return;
            }
            mOpenElements[index] = state;
        }

        private void addElementToOpenElements(bool state)
        {
            mOpenElements.Add(state);
        }

        private void removeElementFromOpenElements(int index)
        {
            mOpenElements.RemoveAt(index);
        }

        private float onHeightCallback(int index)
        {
            var margin = EditorGUIUtility.standardVerticalSpacing;
            if (!isElementOpen(index))
            {
                return 20 + margin;
            }
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var elementHeight = EditorGUI.GetPropertyHeight(element);
            return elementHeight + margin + 24;
        }
                
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Notifications");
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            var item = notificationsLocalStorage.getNotificationByIndex(index);

            EditorGUI.BeginChangeCheck();
            setElementOpen(index, EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), isElementOpen(index)));

            if (!isElementOpen(index))
            {
                item.title = EditorGUI.TextField(new Rect(rect.x + 20, rect.y, rect.width - 18, rect.height), item.title);
            }
            else
            {
                rect.y += 20;
                EditorGUI.PropertyField(rect, serializedObject.FindProperty("notifications").GetArrayElementAtIndex(index), true);
                serializedObject.FindProperty("notifications").GetArrayElementAtIndex(index).isExpanded = true;
                serializedObject.ApplyModifiedProperties();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void AddItem(ReorderableList list)
        {
            var notifications = serializedObject.FindProperty("notifications");
            if (!notifications.isArray)
                return;
            ++notifications.arraySize;
            serializedObject.ApplyModifiedProperties();
            addElementToOpenElements(false);
            EditorUtility.SetDirty(target);            
        }

        private void RemoveItem(ReorderableList list)
        {
            var notifications = serializedObject.FindProperty("notifications");
            if (!notifications.isArray)
                return;
            notifications.DeleteArrayElementAtIndex(list.index);
            serializedObject.ApplyModifiedProperties();
            removeElementFromOpenElements(list.index);
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Space(15);
            GUILayout.Label("NumberOfTimesToBeNotified > 0 to limit how many times it will show");
            GUILayout.Label("Id should be unique");
            GUILayout.Label("IntevalInHours = 0 means non repeating notification");
            GUILayout.Space(5);
            reorderableList.DoLayoutList();
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

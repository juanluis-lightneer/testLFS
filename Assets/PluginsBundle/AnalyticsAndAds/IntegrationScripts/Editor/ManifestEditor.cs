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
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

namespace Lightneer.AdsEditor
{
    /// <summary>
    /// [WARNING!!!] QUICK AND DIRTY MANIFEST EDITOR
    /// IT JUST ADDS AN ELEMENT INSIDE A UNIQUE TAG
    /// CREATED TO INSERT ADMOB META-DATA ELEMENT
    /// INSIDE APPLICATION TAG
    /// </summary>
    public class ManifestEditor
    {
        private TextAsset mTextAsset = null;

        public bool isDocumentOpen
        {
            get { return mTextAsset != null; }
        }

        public ManifestEditor(string path)
        {
            childrendToAppend = new List<KeyValuePair<string, string>>();
            mTextAsset = loadDocument(path);
        }

        private TextAsset loadDocument(string path)
        {
            return AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        }

        public void addChildToUniqueTag(string tagName, string xmlChildToAdd)
        {
            if (!isDocumentOpen)
                return;
            childrendToAppend.Add(new KeyValuePair<string, string>(tagName, xmlChildToAdd));
        }

        private List<KeyValuePair<string, string>> childrendToAppend;

        public void save(string absolutePath)
        {
            if (!isDocumentOpen)
                return;
            var text = mTextAsset.text;
            foreach (var childToAppend in childrendToAppend)
            {
                text = insertElement(text, childToAppend);
            }
            using (StreamWriter sw = new StreamWriter(absolutePath))
            {
                sw.Write(text);
                sw.Close();
            }
            cleanup();
        }

        private void cleanup()
        {
            mTextAsset = null;
            childrendToAppend.Clear();
            childrendToAppend = null;
        }

        private string insertElement(string text, KeyValuePair<string,string> childToAppend)
        {
            if (!text.Contains(childToAppend.Key))
                return text;
            var index = text.LastIndexOf(childToAppend.Key);
            while (text[index] != '<')
            {
                --index;
            }
            --index;
            return text.Insert(index, childToAppend.Value + "\n");
        }
    }
}

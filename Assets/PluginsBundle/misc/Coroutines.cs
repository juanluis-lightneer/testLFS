//-----------------------------------------------------------------------------------------------------------
//
// Coroutines.cs
//
// SingletonMonoBehaviour that holds all the currently running Coroutines that are not necessarily attached
// to a GameObject.
//
//

using UnityEngine;
using System.Collections;

namespace UniQue
{
    public class Coroutines : SingletonMonoBehaviour<Coroutines>
    {
        public static Coroutine start(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void stop(IEnumerator enumerator)
        {
            instance.StopCoroutine(enumerator);
        }

        public static void stop(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }

        protected override bool isPersistent()
        {
            return true;
        }
    }
}

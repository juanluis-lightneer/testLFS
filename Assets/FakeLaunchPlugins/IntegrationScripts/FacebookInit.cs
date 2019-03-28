using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookInit : MonoBehaviour
{    	
	void Start ()
    {
        FB.Init();
	}

    private bool mPressed = false;

    private void Update()
    {
        if (Input.touchCount > 2)
        {
            if (!mPressed)
            {
                mPressed = true;
                Debug.LogError("[FACEBOOK] Is initialized: " + FB.IsInitialized);
            }
        }
        else if (Input.touchCount == 0)
        {
            mPressed = false;
        }
    }
}

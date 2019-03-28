=========
* Plugin info
=========

--------------------
** IMPORTANT!!!!
--------------------

- The project contains an EventManager but it's only used for test project. You will
   need to add your own EventManager.cs since the file included it's surrounded by 
   #if TEST_PROJECT . EventManager is used for IronSource events.

- Modify both the main AndroidManifest (Assets/Plugins/Android/AndroidManifest.xml) 
   so it contains the right bundle id of your game
   
- Make sure your jenkins configuration contains the next parameters (you can check 
   FakeLaunchPlugins configurations as an example):
	- BUNDLE_ID (string, com.lightneer.yourprojectid)	
	- PROJECT_NAME (string, to determine the directories in target -> target/projectName-XX/projectName/)
	- PROJECT_BUNDLE_NAME (string, from com.lightneer.yourprojectid -->>> yourprojectid)
	- GradleBuild (boolean, it should be true so it uses gradle)
	
-----------
** Prefab
-----------

- Prefab folder contains an object with most of the needed scripts, just drag it to the scene.

--------------------
** GameAnalytics
--------------------
URL: https://gameanalytics.com/docs/unity-sdk

- Go to Window/GameAnalytics/Select Settings

- Now insert User/Password

- You can add the specific game inside unity or from website

- A settings asset will be created under resources.

- To complete the setup press "Window/GameAnalytics/Create GameAnalytics Object"

--------------
** Tenjin
--------------
URL: https://github.com/tenjin/tenjin-unity-sdk

- Check API_KEY in prefab: "Plugins / Tenjin"

TODO: GDPR, IAP(?)

----------------
** IronSource
----------------
URL: https://developers.ironsrc.com/ironsource-mobile/unity/unity-plugin

- Go to "Plugins / Ironsource" and fill android and ios game keys

- IronSource events will be sent on banner/interstitial events, the event class is OnIronSrcEvent

- IronSrcIntegration can handle banners and interstitials through public methods and can be 
  accessed statically with IronSrcIntegration.instance. Methods are:
   -- public void showBanner()
   -- public void loadInterstitial()
   -- public void showInterstitial(InterstitialPlacement placementType)
# Ads and Analytics

Common code for Ads and Analytics.

## Plugins included

### [Facebook SDK](https://developers.facebook.com/docs/unity/) 7.15.1
Used for user acquisition

### [PlayFab SDK](https://api.playfab.com/sdks/unity) 2.62.190304
Event sending available through Lightner SDK

### [PlayFab Editor Extensions](https://api.playfab.com/sdks/unity) 2.62.190304
No code added for it yet

### [IronSource](https://developers.ironsrc.com/ironsource-mobile/unity/unity-plugin) 6.8.2
Used for monetization
* [Integrations](https://developers.ironsrc.com/ironsource-mobile/unity/mediation-networks-unity/#step-1)
	* AdMob 4.3.2
	* AppLovin 4.3.4
	* UnityAds 4.1.3
	* Facebook 4.3.3

### [GameAnalytics](https://gameanalytics.com/docs/unity-sdk) 5.1.6
Used for analytics

## Installation

### **Note on upgrading**

If you are upgrading from a previous version of plugins you will need
to remove the previous versions of some iron source integration
libraries (e.g. Plugins/Android/IronSource/libs/mediationsdk-X.Y.Z.jar.), 
this will lead to some errors about dupplicated symbols when building.
Keep in mind that if you made any modifications to AndroidManifest.xml
or mainTemplate.gradle they might be overwritten.

### Clean install

1. If you installed anything before getting this package, cleanup
   previous plugin installations and config files:
	* **If you already have any of the included plugins**, remove them from your
	project. 
	* **If you have some manifest or gradle config**, move it somewhere else, you
	can afterwards manually merge it with the one included in the package
2. Download latest unity package from Github project releases	
3. Open .unitypackage with unity to import all the files.
4. Plugin configuration -> "Check Plugin configuration" section

**The app will not work if Facebook app id it's not correctly
configured due to AndroidManifest.xml**

## Plugin configuration

**For server side configuration please refer to [confluence docs (to be created).](https://confluence.lightneer.org/display/STU/GameLab)**

### Editor windows:
Some of the settings can be adjusted through editor windows found in Unity menus:
* Lightneer / Ads / Config
* Lightneer / Analytics / Config 

### GameAnalytics

* Go to Window/GameAnalytics/Select Settings (or from Lightneer editor window "Open GameAnalytics settings"
* Now insert User/Password (you should have one invitation in your mail)
* You can add the specific game inside unity or from website
* A settings asset will be created under resources.
* To complete the setup press "Window/GameAnalytics/Create GameAnalytics Object"

### Tenjin

**Tenjin is currently removed. If something points towards Tenjin, it
should be removed.**

### PlayFab

Fill in "Title id" in "Lightneer / Analytics / Config" editor window

### Iron Source

* Update Android and iOS keys through the editor window "Lightneer / Ads / Config". This will update the analytics prefab.
* Make sure you have the IronSourceEventsPrefab in your scene or drag
it from "PluginsBundle/AnalyticsAndAds/Prefab"
* IronSource events will be sent on banner/interstitial/rewardedAd
  events, the event class is OnIronSrcEvent
#### AdMob integration (can be done through editor window)
* (Android Only) Go to the AndroidManifest.xml found in 
"Assets/Plugins/Android/IronSource/" and replace the value in 
"[ADMOB_APP_ID]" with your app id
```xml
<meta-data android:name="com.google.android.gms.ads.APPLICATION_ID"
           android:value="[ADMOB_APP_ID]"/>
```
#### AppLovin
* (Android) AndroidManifest.xml needs to have AppLovin key (already
included in the SDK package). [See documentation for more info.](https://developers.ironsrc.com/ironsource-mobile/unity/admob-mediation-guide/)
```xml
<meta-data android:name="applovin.sdk.key" android:value="[KEY]" />
```

#### Iron Source Facebook integration

* Currently not in use and *needs some fixing*, causes this error:
```
com.facebook.ads.AudienceNetworkActivity - MISSING
```

### Facebook Analytics

* Create your game setup in PlayStore and AppStore (you will need the
  bundle id for this)
* Contact Gus to get your app into Facebook. He will give you the App
Id. You also access the game page in facebook and get your Client
Token from Settings / Advanced
	* Insert your key and token in Facebook settings. In Unity got to menu Facebook and
	select "Edit settings". 
	* After inserting the data click on "Regenerate Android Manifest"
* At this point FB should be setup and plugins prefab should contain
  the script FacebookInit

## Usage

### ADS

#### AdsManager

* AdsManager handles banners, rewarded ads and interstitials through
public methods and can be accessed statically through
AdsManager.instance. Methods are:

	* Show banner
```csharp
	public void showBanner()
	public void hideBanner()
```
	* Show interstitial
```csharp
	first, load interstitial:
 	 - public void loadInterstitial(OnInterstitialLoadResult callback)
	second, show interstitial:
	 |- public void showInterstitial(OnInterstitialEnd callback) 
     |- public void showInterstitial(string placement, OnInterstitialEnd callback)
	you can also check if interstitial is available
	 - public cbool isRewardedAdAvailable()
```
	* Show rewarded Ad
```csharp
   public void showRewardedAd(OnRewardedAdResult callback)
   public void showRewardedAd(string placement, OnRewardedAdResult callback)
```

### ANALYTICS

#### IMPORTANT INFORMATION!!
* Analytics SDK will take care of ad events, session start and session
  complete. It's **required** for you to:
  * Send match start and match complete analytics events
  * (If you want screen tracking) Create a class that inherits from
    AbstractLastScreenTracker (more info in AbstractLastScreenTracker 
	section).
* Session time should be retrieved like this:
```csharp
SessionTimeTracker.instance.sessionTime
``` 

#### AbstractLastScreenTracker
* This object lets the SDK know what was the last screen when it's
  sending session complete event. It only implies overriding this method:
```csharp
  public abstract string lastScreen { get; }
```
* It's up to you how to implement this inherit class, it just needs to
  have updated information of the last screen
* When you get it done add it to a game object inside the plugins
  prefab, after that, link it to the references in
  LastScreenTracking.LastScreen
* At this point your screen tracking should be ready. You can also use
  it like this:
```csharp
  string lastScreen = LastScreen.instance.lastScreen;
```

#### For analytics events:

* To send events through the SDK you need to create an instance of the
  corresponding analytics class and send it through
  EventManager. 
  Things to consider:
  * All analytics classes start with "OnAnalytics" (e.g. OnAnalyticsAdStart)
  * If you don't find an specific event, use OnAnaliticsCustomEvent or 
    contact SDK owner to include it if needed.
  * Analytics event constructors contain the required parameters and custom
    event exposes a dictionary where you can include the required data.
```csharp 
var matchStartEvent = new OnAnalyticsGameStart(EventStrings.MATCH_REASON_START, 0, EventStrings.MATCH_ORIGIN_MENU, 0);
EventManager.Send(matchStartEvent);
``` 
  * In EventStrings you can find common string literals for some
    events and some of their parameters, that way every project uses
    same ones, making the work of data analists easier.

## References

### Event sending
* [PlayFab](https://api.playfab.com/docs/tutorials/landing-analytics/pf-events)
* [AppsFlyer](https://support.appsflyer.com/hc/en-us/articles/213766183-AppsFlyer-SDK-Integration-Unity)
* [Facebook](https://developers.facebook.com/docs/app-events/unity/)
* [GameAnalytics](https://gameanalytics.com/docs/item/unity-sdk)
* [UnityAnalytics](https://docs.unity3d.com/Manual/UnityAnalyticsEvents.html)

## Ads UML
![UML](https://github.com/lightneeroy/plugins-bundle/blob/develop/docs/adsUML.jpg)

## Analytics UML
![UML](https://github.com/lightneeroy/plugins-bundle/blob/develop/docs/analyticsUML.jpg)

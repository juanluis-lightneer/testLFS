# Local notifications

The SDK currently can provide local notifications through:
* [UTNotifications](http://universal-tools.github.io/UTNotifications/Manual_1.7.pdf)
* Unity native iOS Local Notifications
* [Unity Mobile Notifications Package](https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.0/manual/index.html)

## Important notes
1. Local notifications rely on EventManager, please install it or copy
it from "Assets/Plugins/AnalyticsAndAds/IntegrationScripts/3rdParty" to
a new location and remove the conditional compile symbol "TEST_PROJECT".
2. The best candidate to be used as notification provider is based on
a unity package currently in preview state. To use it, install the
package from package manager (enable preview packages from
"Advanced/Show preview packages") and add
"USE_UNITY_NOTIFICATIONS_PACKAGE" to "Scripting Define Symbols" both
in Android and iOS Player Settings.

## Plugins included
### UTNotifications
Used for Android notifications. Not used for iOS since it requires the
project to have special permissions in provisioning profile.  Might be
removed in the future in favour of Mobile Notifications Package.

### Unity native iOS Local Notification
Local notifications from Unity currently requires special permissions
in provisioning profile, therefore it might be removed in the future.

### Unity Mobile Notifications Package
Handles both Android and iOS, it's currently in preview state. It
seems to work fine although it doesn't handle notification clicks. It
doesn't require special requirements in iOS provisioning profile.

## On architecture

The SDK architecture has been designed taking into account the 
possibility of changes in the plugins used for local notifications. 
Therefore, if the SDK needs a different one, we only need a class 
that implements NotificationPluginStrategy. After that you will 
need to set the string(s) named ClassForAndroidNotifications and/or 
ClassForIOSNotifications in NotificationPluginConfigurator in your 
Notifications prefab. This fields are the class types and look 
like this:
```
	Lightneer.Notifications.UTNotificationsStrategy
```
After being set, NotificationPluginConfigurator will create an 
object of that class and inject it into the NotificationScheduler.

## Basic behaviour

The current code is designed to have a in-scene notification container
where all notifications are kept. Whenever the game opens all
notifications are cancelled so they are not fired during
gameplay. When the game is closed, notifications that are active are
re-scheduled.

## Configuration

1. Set settings: 
* **(For UTNotifications)** Open Edit / Project Settings /
UTNotifications and configure if needed. You might want to set icons
in there. Profiles are not supported at the moment.  For more info
check [manual](http://universal-tools.github.io/UTNotifications/Manual_1.7.pdf).
* **(For Mobile Notifications Pckage)** Go to "Edit/Project Settings" and
you will see "Mobile Notification Settings". In this section you can
adjust the settings, including notification icon.
2. Drag Notifications prefab (.. / PluginsBundle / LocalNotifications / Prefabs) into main scene. The prefab has a DontDestroyOnLoad script
3. Configure the required notifications in prefab inside NotificationsKeeper.
	* Press "+" to add notifications to the list.
	* Tick toggle button to open specific notification and fill in the fields 
	   (check tooltips for more info)
	* Keep adding/removing/modifying notifications as needed and save the scene.
4. At this point you should start seing the scheduled notifications in your mobile devie.

## Code usage

### Enable/disable notifications

NotificationState let's the programmer enable/disable an specific notification knowing
it's id using the following code:
```
	NotificationScheduler.instance.setNotificationState(int id, bool enabled)
```
	
## Future work
To allow programmers (or any other team member) include new
notifications either by code or from online services (like PlayFab),
INotificationStorage can be implemented to provide new notification
sources. E.g: PlayFabNotificationStorage (present in UML class diagram)
can be implemented, using PlayFab as source of new events and use 
LocalNotificationStorage for predefined events.

## Class diagram
![UML](https://github.com/lightneeroy/plugins-bundle/blob/develop/docs/localNotificationsUML.jpg)



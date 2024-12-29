using UnityEngine;

public static class NotificationLogic
{
    private static AndroidJavaObject activity;

    public static void StartNotificationSetting()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", activity, new AndroidJavaClass("com.Parkrye.notification.ForegroundService"));
            activity.Call("startService", intent);
        }
    }

    public static void ShowNotification(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass notificationHelper = new AndroidJavaClass("com.yourcompany.notification.NotificationHelper");
            notificationHelper.CallStatic("showPersistentNotification", activity, text);
        }
    }

    public static void UpdateNotification(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass notificationHelper = new AndroidJavaClass("com.yourcompany.notification.NotificationHelper");
            notificationHelper.CallStatic("updateNotification", activity, text);
        }
    }

    public static void RemoveNotification()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass notificationHelper = new AndroidJavaClass("com.yourcompany.notification.NotificationHelper");
            notificationHelper.CallStatic("removeNotification", activity);
        }
    }
}

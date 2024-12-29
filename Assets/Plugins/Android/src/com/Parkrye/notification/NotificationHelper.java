package com.Parkrye.notification;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.app.Service;
import android.os.IBinder;

public class NotificationHelper {
    private static final String CHANNEL_ID = "step_counter_channel";
    private static final int NOTIFICATION_ID = 1;

    public static void createNotificationChannel(Context context) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel(
                CHANNEL_ID,
                "Step Counter Notifications",
                NotificationManager.IMPORTANCE_LOW
            );
            channel.setDescription("Persistent notification for step counter.");
            NotificationManager manager = context.getSystemService(NotificationManager.class);
            if (manager != null) {
                manager.createNotificationChannel(channel);
            }
        }
    }

    public static void showPersistentNotification(Context context, String text) {
        createNotificationChannel(context);
        Notification notification = new Notification.Builder(context, CHANNEL_ID)
            .setContentTitle("Step Counter")
            .setContentText(text)
            .setSmallIcon(android.R.drawable.ic_dialog_info) // 기본 아이콘
            .setOngoing(true) // 사용자가 알림을 제거하지 못하도록 설정
            .build();

        NotificationManager manager = context.getSystemService(NotificationManager.class);
        if (manager != null) {
            manager.notify(NOTIFICATION_ID, notification);
        }
    }

    public static void updateNotification(Context context, String text) {
        showPersistentNotification(context, text);
    }

    public static void removeNotification(Context context) {
        NotificationManager manager = context.getSystemService(NotificationManager.class);
        if (manager != null) {
            manager.cancel(NOTIFICATION_ID);
        }
    }
}

public class ForegroundService extends Service {
    private static final String CHANNEL_ID = "step_counter_channel";

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        createNotificationChannel();
        Notification notification = new Notification.Builder(this, CHANNEL_ID)
            .setContentTitle("Step Counter")
            .setContentText("Running...")
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .build();

        startForeground(1, notification);
        return START_STICKY;
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    private void createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel(
                CHANNEL_ID,
                "Step Counter Notifications",
                NotificationManager.IMPORTANCE_LOW
            );
            NotificationManager manager = getSystemService(NotificationManager.class);
            if (manager != null) {
                manager.createNotificationChannel(channel);
            }
        }
    }
}
package com.yourcompany.vibrationplugin;

import android.content.Context;
import android.os.VibrationEffect;
import android.os.Vibrator;
import com.unity3d.player.UnityPlayer;

public class VibrationPlugin {
    public static void Vibrate(int milliseconds, int amplitude) {
        Context context = UnityPlayer.currentActivity.getApplicationContext();
        Vibrator vibrator = (Vibrator) context.getSystemService(Context.VIBRATOR_SERVICE);
        if (vibrator != null) {
            if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
                vibrator.vibrate(VibrationEffect.createOneShot(milliseconds, amplitude));
            } else {
                vibrator.vibrate(milliseconds);
            }
        }
    }
}
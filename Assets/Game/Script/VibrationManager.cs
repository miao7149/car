using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static void Vibrate(int milliseconds, int amplitude)
    {
        if (GlobalManager.Instance.IsVibrate == false)
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass vibrationPlugin = new AndroidJavaClass("com.yourcompany.vibrationplugin.VibrationPlugin"))
                {
                    vibrationPlugin.CallStatic("Vibrate", milliseconds, amplitude);
                }
            }
        }

#elif UNITY_IOS && !UNITY_EDITOR
         Vibration.VibratePeek();
#endif
    }
}

using UnityEngine;
using CandyCoded.HapticFeedback;

[RequireComponent(typeof(AudioSource))]
public class EffectManager : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip; // The audio clip to be played
   
    private AudioSource _audioSource; // Audio source component

    private void Start()
    {
        // Initialize the audio source
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audioClip;

        // Trigger a default vibration (basic haptic feedback)
        Handheld.Vibrate();

        // Ensure Richtap system is initialized
        
    }

    public void PlayCollectingSound()
    {
        // Play haptic effect if assigned
        HapticFeedback.LightFeedback();
        // Optionally play the audio clip
        if (_audioClip != null && _audioSource != null)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip or AudioSource is missing.");
        }
    }

    private void OnDestroy()
    {
        // Release RichTap resources when this object is destroyed
        
    }

    // Check if Android version is Oreo (API 26) or higher
    private static bool AndroidVersionIsOreoOrHigher()
    {
        int sdkInt;
        using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            sdkInt = versionClass.GetStatic<int>("SDK_INT");
        }
        return sdkInt >= 26; // API 26 corresponds to Android Oreo
    }
}

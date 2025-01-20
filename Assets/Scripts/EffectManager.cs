using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip _audioClip;

    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        Handheld.Vibrate();
    }

    public void PlayCollectingSound()
    {

       
        _audioSource.Play();
    }
    

    private static bool AndroidVersionIsOreoOrHigher()
    {
        int sdkInt;
        using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            sdkInt = versionClass.GetStatic<int>("SDK_INT");
        }
        return sdkInt >= 26; // API 26 is Android Oreo
    }   

}

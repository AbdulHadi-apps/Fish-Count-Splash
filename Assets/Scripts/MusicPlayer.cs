//using UnityEngine;

//public class MusicPlayer : MonoBehaviour
//{
//    private static MusicPlayer instance;
//    private AudioSource audioSource;

//    void Awake()
//    {
//        if (instance != null && instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        instance = this;
//        DontDestroyOnLoad(gameObject);

//        audioSource = GetComponent<AudioSource>();
//        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
//        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

//        audioSource.volume = savedVolume;
//        audioSource.loop = true;
//        audioSource.mute = !isMusicOn;
//        audioSource.Play();
//    }

//    public static AudioSource GetAudioSource()
//    {
//        return instance != null ? instance.GetComponent<AudioSource>() : null;
//    }
//}

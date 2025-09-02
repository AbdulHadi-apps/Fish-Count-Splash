using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;
    public AudioSource myMusic;

    //public Sprite muteIcon;      // 🔇 icon
    //public Sprite unmuteIcon;    // 🔊 icon
    //public Image iconImage;      // The Image UI that shows the icon

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = isMusicOn;
            musicToggle.onValueChanged.AddListener(SetMusicState);
        }

        myMusic.volume = savedVolume;
        myMusic.mute = !isMusicOn;

       // UpdateIcon(isMusicOn);
    }

    public void SetVolume(float volume)
    {
        myMusic.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetMusicState(bool isOn)
    {
        myMusic.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
       // UpdateIcon(isOn);
    }

    //private void UpdateIcon(bool isMusicOn)
    //{
    //    if (iconImage != null)
    //    {
    //        iconImage.sprite = isMusicOn ? unmuteIcon : muteIcon;
    //    }
    //}
}
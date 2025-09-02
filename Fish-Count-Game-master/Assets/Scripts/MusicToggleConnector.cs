using UnityEngine;
using UnityEngine.UI;

public class MusicToggleConnector : MonoBehaviour
{
    private Toggle toggle;
    private MusicControl musicControl;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        GameObject musicObj = GameObject.FindGameObjectWithTag("Music");
        if (musicObj != null)
        {
            musicControl = musicObj.GetComponent<MusicControl>();

            // Set the toggle state based on current music mute status
            toggle.isOn = !musicControl.myMusic.mute;

            // Link toggle event to music control
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        else
        {
            Debug.LogWarning("Music object with tag 'Music' not found.");
        }
    }

    void OnToggleChanged(bool isOn)
    {
        if (musicControl != null)
        {
            musicControl.SetMusicState(isOn);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SFXToggleConnector : MonoBehaviour
{
    public Toggle sfxToggle;
    public Image speakerIconImage; // 🎯 Reference to the speaker icon Image
    public Sprite speakerOnSprite; // 🔊 Sprite when SFX is ON
    public Sprite speakerOffSprite; // 🔇 Sprite when SFX is OFF

   

    void Start()
    {
        if (sfxToggle != null)
        {
            sfxToggle.isOn = NumbersVoice.IsSFXOn;
            UpdateSpeakerIcon(sfxToggle.isOn); // 🖼️ Set initial icon

            sfxToggle.onValueChanged.AddListener(delegate {
                NumbersVoice.SetSFXState(sfxToggle.isOn);
                UpdateSpeakerIcon(sfxToggle.isOn); // 🔁 Update icon on toggle
            });
        }
    }

    void UpdateSpeakerIcon(bool isOn)
    {
        if (speakerIconImage != null)
        {
            speakerIconImage.sprite = isOn ? speakerOnSprite : speakerOffSprite;
        }
    }
}

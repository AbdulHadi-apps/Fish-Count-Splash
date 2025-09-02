using UnityEngine;

public class NumbersVoice : MonoBehaviour
{
    public AudioSource source;

    // ✅ Multiple correct answer variations
    public AudioClip[] correctAnswerClips; // size = 3 in Inspector

    // ✅ Multiple wrong answer variations
    public AudioClip[] wrongAnswerClips;   // size = 3 in Inspector

    public static bool IsSFXOn = true;

    public static void SetSFXState(bool isOn)
    {
        IsSFXOn = isOn;
    }

    public void PlayCorrectSound()
    {
        if (!IsSFXOn || source == null || correctAnswerClips == null || correctAnswerClips.Length == 0) return;

        AudioClip clip = correctAnswerClips[Random.Range(0, correctAnswerClips.Length)];
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlayWrongSound()
    {
        if (!IsSFXOn || source == null || wrongAnswerClips == null || wrongAnswerClips.Length == 0) return;

        AudioClip clip = wrongAnswerClips[Random.Range(0, wrongAnswerClips.Length)];
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }
}

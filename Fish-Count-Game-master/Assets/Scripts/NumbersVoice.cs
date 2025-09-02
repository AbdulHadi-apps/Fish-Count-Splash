using UnityEngine;

public class NumbersVoice : MonoBehaviour
{
    public AudioSource source;
    public AudioClip one, two, three, four, five, six, seven, eight, nine, ten;
    public AudioClip wrongAnswer; // 🔴 Single sound for all wrong answers
    public AudioClip levelCompleteSound;
    public AudioClip starAwardSound;

    public static bool IsSFXOn = true;

    public static void SetSFXState(bool isOn)
    {
        IsSFXOn = isOn;
    }

    public void PlayCorrectNumberSound(int number)
    {
        if (!IsSFXOn || source == null) return;

        AudioClip clip = null;
        switch (number)
        {
            case 1: clip = one; break;
            case 2: clip = two; break;
            case 3: clip = three; break;
            case 4: clip = four; break;
            case 5: clip = five; break;
            case 6: clip = six; break;
            case 7: clip = seven; break;
            case 8: clip = eight; break;
            case 9: clip = nine; break;
            case 10: clip = ten; break;
        }

        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlayWrongSound()
    {
        if (!IsSFXOn || source == null || wrongAnswer == null) return;

        source.clip = wrongAnswer;
        source.Play();
    }

    public void PlayLevelCompleteSound()
    {
        if (!IsSFXOn || source == null || levelCompleteSound == null) return;
        source.clip = levelCompleteSound;
        source.Play();
    }

    public void PlayStarAwardSound()
    {
        if (!IsSFXOn || source == null || starAwardSound == null) return;
        source.clip = starAwardSound;
        source.Play();
    }


}

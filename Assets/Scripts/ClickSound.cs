using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource clickSource;
    public AudioClip click;



    public void ClickSoundButton()
    {
        clickSource.clip = click;
        clickSource.Play();
    }
}

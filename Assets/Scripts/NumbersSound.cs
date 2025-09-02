using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NumbersSound : MonoBehaviour
{

    public AudioSource source;
    
    public AudioClip clip;
   

    public void playSound()
    {
        source.clip = clip;
        source.Play();
    }
    public void OnButtonClick()
    {
        AnimatePop();

        // Add your logic here like playing sound, moving to next screen, etc.
    }
    public void AnimatePop()
    {
        // Reset scale first
        transform.localScale = Vector3.one;

        // Pop up and down animation
        transform.DOScale(1.5f, 0.4f)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() =>
                 {
                     transform.DOScale(1f, 0.3f)
                              .SetEase(Ease.InBack);
                 });
    }

    // Call this on button click
    


}


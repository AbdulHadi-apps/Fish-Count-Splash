using UnityEngine;

public class SplashSpeechAnimator : MonoBehaviour
{
    public Animator animator;             
    public string talkParameter = "IsTalking";
    public bool isTalking;

    public AudioSource audioSource;
    public AudioSource audioSource1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isTalking = (audioSource != null && audioSource.isPlaying) ||
                    (audioSource1 != null && audioSource1.isPlaying);

        animator.SetBool(talkParameter, isTalking);
    }
}

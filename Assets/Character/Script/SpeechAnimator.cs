using UnityEngine;

public class SpeechAnimator : MonoBehaviour
{
    public Animator animator;             
    public string talkParameter = "IsTalking";

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isTalking = audioSource.isPlaying;
        animator.SetBool(talkParameter, isTalking);
    }
}

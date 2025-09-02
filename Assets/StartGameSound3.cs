using UnityEngine;
using System.Collections;

public class StartGameSound3 : MonoBehaviour
{
    public AudioClip[] startGameSFX;
    public AudioSource startgame;
    public Animator characterAnimator;
    public string animationTriggerName = "PlayWave";

    public float idleThreshold = 10f;
    private float idleTimer = 0f;
    private bool isGameIdle = false;

    void Start()
    {
        if (startGameSFX.Length > 0 && characterAnimator != null && startgame != null)
        {
            PlaySoundAndAnimation();
            StartCoroutine(PlayWhenIdle());
        }
    }

    void Update()
    {
        if (Input.anyKey || Input.touchCount > 0 || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            idleTimer = 0f;
            isGameIdle = false;
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold)
            {
                isGameIdle = true;
            }
        }
    }

    void PlaySoundAndAnimation()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger(animationTriggerName);
        }

        // ✅ Use the same IsSFXOn toggle from NumbersVoice
        if (NumbersVoice.IsSFXOn && startGameSFX.Length > 0 && startgame != null)
        {
            int randomIndex = Random.Range(0, startGameSFX.Length);
            startgame.PlayOneShot(startGameSFX[randomIndex]);
        }
    }

    IEnumerator PlayWhenIdle()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            if (isGameIdle)
            {
                PlaySoundAndAnimation();
            }
        }
    }
}

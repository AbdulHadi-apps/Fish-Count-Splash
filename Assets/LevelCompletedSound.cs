using UnityEngine;
using System.Collections;

public class LevelCompletedSound : MonoBehaviour
{
    public AudioClip[] startGameSFX;
    public AudioSource startgame;

    private void Start()
    {
        StartCoroutine(PlayLevelCompletedSoundWithDelay());
    }

    private IEnumerator PlayLevelCompletedSoundWithDelay()
    {
        yield return new WaitForSeconds(0f);

        if (NumbersVoice.IsSFXOn && startGameSFX.Length > 0 && startgame != null)
        {
            int randomIndex = Random.Range(0, startGameSFX.Length);
            startgame.clip = startGameSFX[randomIndex];
            startgame.Play();
        }
    }
}

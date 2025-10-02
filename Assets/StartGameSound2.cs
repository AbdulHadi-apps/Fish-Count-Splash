using UnityEngine;
using System.Collections;

public class StartGameSound2 : MonoBehaviour
{
    public AudioClip[] startGameSFX;
    public AudioSource startgame;

    void Start()
    {
        if (startGameSFX.Length > 0 && startgame != null)
        {
            StartCoroutine(PlayStartGameSoundsLoop());
        }
    }

    IEnumerator PlayStartGameSoundsLoop()
    {
        while (true) // Infinite loop
        {
            // ✅ Check global sound toggle before playing
            if (NumbersVoice.IsSFXOn && startGameSFX.Length > 0)
            {
                int randomIndex = Random.Range(0, startGameSFX.Length);
                startgame.clip = startGameSFX[randomIndex];
                startgame.Play();

                // Wait until the clip finishes
                yield return new WaitForSeconds(startgame.clip.length);
            }

            // Always wait extra 5 seconds before checking again
            yield return new WaitForSeconds(12f);
        }
    }
}

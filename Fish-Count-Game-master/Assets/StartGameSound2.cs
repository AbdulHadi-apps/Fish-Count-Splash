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
            // Pick a random clip
            int randomIndex = Random.Range(0, startGameSFX.Length);
            startgame.clip = startGameSFX[randomIndex];
            startgame.Play();

            // Wait until the clip finishes playing
            yield return new WaitForSeconds(startgame.clip.length);

            // Wait an extra 5 seconds before playing the next clip
            yield return new WaitForSeconds(5f);
        }
    }
}

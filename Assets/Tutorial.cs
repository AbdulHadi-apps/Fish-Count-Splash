using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    public GameObject Hand;
    public GameObject[] Points; // [0] = start position, [1] = will be set at runtime

    void Awake()
    {
        instance = this;
    }

    public void StartTutorialAtAnswer(GameObject correctAnswerObj)
    {
        Points[1] = correctAnswerObj;
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        Hand.SetActive(true);

        // Move hand to correct answer
        while (Vector3.Distance(Hand.transform.position, Points[1].transform.position) > 0.01f)
        {
            Hand.transform.position = Vector3.MoveTowards(
                Hand.transform.position,
                Points[1].transform.position,
                Time.deltaTime * 5f
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // pause on target

        // Move back to start
        while (Vector3.Distance(Hand.transform.position, Points[0].transform.position) > 0.01f)
        {
            Hand.transform.position = Vector3.MoveTowards(
                Hand.transform.position,
                Points[0].transform.position,
                Time.deltaTime * 5f
            );
            yield return null;
        }

        // Optionally disable hand
        // Hand.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class HandTutorial : MonoBehaviour
{
    public static HandTutorial instance;
    public GameObject[] Points; // Assign second point manually in Inspector
    public GameObject Hand;     // The tutorial hand (UI Image)
    public float speed = 300f;  // pixels per second

    private RectTransform handRect;
    private RectTransform canvasRect;

    void Awake()
    {
        instance = this;
        handRect = Hand.GetComponent<RectTransform>();
        canvasRect = handRect.parent as RectTransform;
    }

    IEnumerator Start()
    {
        // Wait until QuizGameLevel2 sets the correct button
        yield return new WaitUntil(() => QuizGameLevel2.CorrectAnswerButton != null);

        // Now play
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        Hand.SetActive(true);

        if (Points.Length < 2 || Points[1] == null)
        {
            Debug.LogError("HandTutorial: Points[1] is not assigned in Inspector!");
            yield break;
        }

        Points[1] = QuizGameLevel2.CorrectAnswerButton.gameObject;

        RectTransform start = Points[0].GetComponent<RectTransform>();
        RectTransform target = Points[1].GetComponent<RectTransform>();

        // Convert positions into local space of Hand’s parent
        Vector2 startPos, targetPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(null, start.position),
            null,
            out startPos
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(null, target.position),
            null,
            out targetPos
        );

        // Move to target
        while (Vector2.Distance(handRect.anchoredPosition, targetPos) > 1f)
        {
            handRect.anchoredPosition = Vector2.MoveTowards(
                handRect.anchoredPosition,
                targetPos,
                Time.deltaTime * speed
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Move back to start
        while (Vector2.Distance(handRect.anchoredPosition, startPos) > 1f)
        {
            handRect.anchoredPosition = Vector2.MoveTowards(
                handRect.anchoredPosition,
                startPos,
                Time.deltaTime * speed
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.8f);
        Hand.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using TMPro;

public class QuizGameLevel2 : MonoBehaviour
{
    public TransitionSettings transition;
    public float startDelay;
    public Animator animator;
    public FishSpawner fishSpawner;
    public Button[] answerButtons;
    public Image[] feedbackIcons; // Child images for tick/cross
    public Text questionText;
    public Text scoreText;
    public int currentlevel;
    public TextMeshProUGUI levelText;
    public static Button CorrectAnswerButton;


    public GameObject segmentResultPanel; // ✅ For every 5 questions
    public GameObject finalGamePanel; // ✅ For 100 questions

    public Text segmentResultText;
    public Text finalResultText;

    public Image star1, star2, star3;
    public Sprite filledStar, emptyStar;

    public Sprite tickSprite;   // ✅ Assign in Inspector
    public Sprite crossSprite;  // ❌ Assign in Inspector

    public AudioSource correctAudio;
    public AudioSource errorAudio;

    private int segmentScore = 0;
    private int segmentQuestions = 0;

    // 🔊 Added for audio
    public NumbersVoice numbersVoice;

    private int currentQuestion = 1;
    private int correctAnswer;
    private int correctButtonIndex;
    private bool answered = false;
    private int totalQuestions = 100;

    private int score = 0; // correct answers
    private int questionsAnswered = 0; // total answered so far

    void Start()
    {
        currentlevel = PlayerPrefs.GetInt("CurrentLevel",1);
        UpdateScoreText();
        levelText.text="Level: " + currentlevel;
        AskQuestion();
    }

    void AskQuestion()
    {
        animator.Play("TextAppear", -1, 0f);
        TransitionManager.Instance().Transition(transition, startDelay);
        answered = false;

        foreach (Transform child in fishSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        // Progressive difficulty: more fish as questions go on
        int minFish = Mathf.Min(1 + (currentQuestion / 10), 10);   // increases every 10 Qs
        int maxFish = Mathf.Min(5 + (currentQuestion / 5), 18);    // increases every 5 Qs

        int fishCount = Random.Range(minFish, maxFish + 1);
        fishSpawner.SpawnFish(fishCount);

        correctAnswer = fishSpawner.currentFishCount;

        questionText.text = $"Question {currentQuestion} of {totalQuestions}\nHow many fish do you see?";
        SetupAnswers();
    }

    void SetupAnswers()
    {
        correctButtonIndex = Random.Range(0, 4);

        // Keep track of answers we already used
        HashSet<int> usedAnswers = new HashSet<int>();
        usedAnswers.Add(correctAnswer);

        for (int i = 0; i < 4; i++)
        {
            int answer;
            if (i == correctButtonIndex)
            {
                answer = correctAnswer;
                CorrectAnswerButton = answerButtons[i];
            }
            else
            {
                do
                {
                    answer = Random.Range(1, 19); // 1–18 possible answers
                } while (usedAnswers.Contains(answer));

                usedAnswers.Add(answer);
            }

            // 🔹 Animate text change
            Text buttonText = answerButtons[i].GetComponentInChildren<Text>();
            StartCoroutine(AnimateTextChange(buttonText, answer.ToString()));

            feedbackIcons[i].gameObject.SetActive(false);

            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    IEnumerator AnimateTextChange(Text textComponent, string newText)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 originalScale = textComponent.transform.localScale;

        // Scale up
        float time = 0f;
        float duration = 0.25f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(1f, 1.8f, t);
            textComponent.transform.localScale = originalScale * scale;
            yield return null;
        }

        // Change text while large
        textComponent.text = newText;

        // Scale back down
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(2f, 1.8f, t);
            textComponent.transform.localScale = originalScale * scale;
            yield return null;
        }

        textComponent.transform.localScale = originalScale;
    }

    int GetUniqueWrongAnswer()
    {
        int wrong;
        do
        {
            wrong = Random.Range(1, 19);
        } while (wrong == correctAnswer);
        return wrong;
    }

    void CheckAnswer(int index)
    {
        if (answered) return;
        answered = true;

        questionsAnswered++;
        segmentQuestions++;

        if (index == correctButtonIndex)
        {
            feedbackIcons[index].sprite = tickSprite;
            feedbackIcons[index].gameObject.SetActive(true);
            score++;
            segmentScore++;

            // 🔊 Play random correct answer clip
            if (numbersVoice != null && NumbersVoice.IsSFXOn)
            {
                numbersVoice.PlayCorrectSound();
                correctAudio.Play();
            }
        }
        else
        {
            feedbackIcons[index].sprite = crossSprite;
            feedbackIcons[index].gameObject.SetActive(true);

            feedbackIcons[correctButtonIndex].sprite = tickSprite;
            feedbackIcons[correctButtonIndex].gameObject.SetActive(true);

            // 🔊 Play random wrong answer clip
            if (numbersVoice != null && NumbersVoice.IsSFXOn)
            {
                numbersVoice.PlayWrongSound();
                errorAudio.Play();
            }
        }

        UpdateScoreText();
        StartCoroutine(NextStep());
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}/{questionsAnswered}";
    }

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < feedbackIcons.Length; i++)
        {
            feedbackIcons[i].gameObject.SetActive(false);
        }

        if (currentQuestion % 5 == 0 && currentQuestion < totalQuestions)
        {
            ShowSegmentResult();
            currentQuestion++;
            yield break;
        }

        if (currentQuestion < totalQuestions)
        {
            currentQuestion++;
            AskQuestion();
        }
        else
        {
            ShowFinalResult();
        }
    }

    void ShowSegmentResult()
    {
        segmentResultPanel.SetActive(true);

        // Show progress of just this segment
        segmentResultText.text = $"Score: {segmentScore}/{segmentQuestions}";

        // Calculate stars for this segment only
        float percentage = ((float)segmentScore / segmentQuestions) * 100f;

        star1.sprite = emptyStar;
        star2.sprite = emptyStar;
        star3.sprite = emptyStar;

        if (percentage >= 80f)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = filledStar;
        }
        else if (percentage >= 50f)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
        }
        else if (percentage >= 20f)
        {
            star1.sprite = filledStar;
        }

        /*if (Initialize.Instance != null)
        {
            Initialize.Instance.LoadInterstitialAd();
            Initialize.Instance.ShowInterstitialAd();
        }*/
    }

    void ShowFinalResult()
    {
        questionText.text = "";
        finalGamePanel.SetActive(true);
        finalResultText.text = $"Final Score: {score}/{totalQuestions}";
        UpdateScoreText();

        float percentage = ((float)score / totalQuestions) * 100f;

        if (percentage >= 80f)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = filledStar;
        }
        else if (percentage >= 50f)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = emptyStar;
        }
        else if (percentage >= 20f)
        {
            star1.sprite = filledStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
        }
        else
        {
            star1.sprite = emptyStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
        }

        PlayerPrefs.SetInt("Level3", 1);
        PlayerPrefs.Save();
    }

    public void ContinueGameAfterSegment()
    {
        segmentResultPanel.SetActive(false);

        // Reset counters for next segment
        segmentScore = 0;
        segmentQuestions = 0;
        currentlevel++;
        PlayerPrefs.SetInt("CurrentLevel",currentlevel);
        levelText.text = "Level: " + currentlevel;
        PlayerPrefs.Save();

        AskQuestion();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("NumbersLearning");
    }
}

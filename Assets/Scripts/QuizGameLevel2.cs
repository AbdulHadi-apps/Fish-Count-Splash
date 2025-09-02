using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizGameLevel2 : MonoBehaviour
{
    public FishSpawner fishSpawner;
    public Button[] answerButtons;
    public Image[] feedbackIcons; // Child images for tick/cross
    public Text questionText;
    public Text scoreText;

    public GameObject segmentResultPanel; // ✅ For every 5 questions
    public GameObject finalGamePanel; // ✅ For 100 questions

    public Text segmentResultText;
    public Text finalResultText;

    public Image star1, star2, star3;
    public Sprite filledStar, emptyStar;

    public Sprite tickSprite;   // ✅ Assign in Inspector
    public Sprite crossSprite;  // ❌ Assign in Inspector

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
        UpdateScoreText();
        AskQuestion();
    }

    void AskQuestion()
    {
        answered = false;

        foreach (Transform child in fishSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        int fishCount = Random.Range(1, 19);
        fishSpawner.SpawnFish(fishCount);
        correctAnswer = fishSpawner.currentFishCount;

        questionText.text = $"Question {currentQuestion} of {totalQuestions}\nHow many fish do you see?";
        SetupAnswers();
    }

    void SetupAnswers()
    {
        correctButtonIndex = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            int answer = (i == correctButtonIndex) ? correctAnswer : GetUniqueWrongAnswer();
            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();

            feedbackIcons[i].gameObject.SetActive(false);

            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
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

        if (index == correctButtonIndex)
        {
            feedbackIcons[index].sprite = tickSprite;
            feedbackIcons[index].gameObject.SetActive(true);
            score++;

            // 🔊 Play random correct answer clip
            if (numbersVoice != null)
            {
                numbersVoice.PlayCorrectSound();
            }
        }
        else
        {
            feedbackIcons[index].sprite = crossSprite;
            feedbackIcons[index].gameObject.SetActive(true);

            feedbackIcons[correctButtonIndex].sprite = tickSprite;
            feedbackIcons[correctButtonIndex].gameObject.SetActive(true);

            // 🔊 Play random wrong answer clip
            if (numbersVoice != null)
            {
                numbersVoice.PlayWrongSound();
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
        segmentResultText.text = $"Progress: {score}/{questionsAnswered}";

        if (Initialize.Instance != null)
        {
            Initialize.Instance.LoadInterstitialAd();
            Initialize.Instance.ShowInterstitialAd();
        }
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

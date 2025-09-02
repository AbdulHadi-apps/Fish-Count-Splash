using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class QuizGameLevel5 : MonoBehaviour
{
    public Image[] fruitImages = new Image[3];
    public Button[] answerButtons = new Button[4];
    public Text questionText;
    public Text scoreText;

    public Sprite[] fruitsSprites;
    public string[] fruitsNames;
    public NumbersVoice numbersVoice;

    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStar;
    public Sprite emptyStar;

    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public int questions;
    public GameObject finalScorePanel;
    public Text finalScoreText;
    public GameObject Imagespanel;

    private int correctAnswer;
    private int score = 0;
    private int totalQuestions = 0;
    private bool isAnswering = false;

    private string[] numberWords = {
        "ONE", "TWO", "THREE", "FOUR", "FIVE",
        "SIX", "SEVEN", "EIGHT", "NINE", "TEN",
    };

    private int[] currentButtonAnswers = new int[4];

    void Start()
    {
        if (fruitsSprites.Length != fruitsNames.Length || fruitsSprites.Length < 2)
        {
            Debug.LogError("Fruit sprites and names must match and be at least 2.");
            return;
        }

        resultImage.gameObject.SetActive(false);
        finalScorePanel.SetActive(false);
        scoreText.gameObject.SetActive(true);

        SetupNewQuestion();
    }

    void SetupNewQuestion()
    {
        isAnswering = false;
        totalQuestions++;

        int[] chosenFruits = new int[3];
        for (int i = 0; i < 3; i++)
        {
            int randIndex = Random.Range(0, fruitsSprites.Length);
            chosenFruits[i] = randIndex;
            fruitImages[i].sprite = fruitsSprites[randIndex];
        }

        int targetIndex = chosenFruits[Random.Range(0, 3)];
        string targetFruitName = fruitsNames[targetIndex];

        correctAnswer = 0;
        foreach (int index in chosenFruits)
        {
            if (index == targetIndex) correctAnswer++;
        }

        questionText.text = $"How many {targetFruitName} are in this picture?";

        List<int> options = new List<int> { correctAnswer };
        while (options.Count < 4)
        {
            int rand = Random.Range(1, 6);
            if (!options.Contains(rand))
                options.Add(rand);
        }

        // Shuffle options
        for (int i = 0; i < options.Count; i++)
        {
            int temp = options[i];
            int rand = Random.Range(i, options.Count);
            options[i] = options[rand];
            options[rand] = temp;
        }

        for (int i = 0; i < 4; i++)
        {
            int answer = options[i];
            currentButtonAnswers[i] = answer;

            string word = numberWords[answer - 1];
            answerButtons[i].GetComponentInChildren<Text>().text = word;
            answerButtons[i].onClick.RemoveAllListeners();

            int capturedAnswer = answer;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(capturedAnswer));
            answerButtons[i].image.color = Color.white;
        }

        UpdateScore();
    }

    void CheckAnswer(int selected)
    {
        if (isAnswering) return;
        isAnswering = true;

        resultImage.gameObject.SetActive(true);
        bool isCorrect = (selected == correctAnswer);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (currentButtonAnswers[i] == selected)
            {
                answerButtons[i].image.color = isCorrect ? Color.green : Color.red;
            }
        }

        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;
        resultImage.color = isCorrect ? Color.green : Color.red;

        questionText.text = isCorrect ? "Correct!" : $"Wrong! It was {numberWords[correctAnswer - 1]}";

        if (isCorrect)
        {
            score++;
            //numbersVoice.PlayCorrectNumberSound(correctAnswer);
        }
        else
        {
            numbersVoice.PlayWrongSound();
        }

        UpdateScore();
        scoreText.gameObject.SetActive(true);

        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        foreach (var btn in answerButtons)
        {
            btn.image.color = Color.white;
        }

        resultImage.gameObject.SetActive(false);
        resultImage.color = Color.white;

        if (totalQuestions >= questions)
            EndGame();
        else
            SetupNewQuestion();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + totalQuestions.ToString();
        scoreText.gameObject.SetActive(true);
    }

    void EndGame()
    {
        questionText.text = "Game Over!";
        Imagespanel.SetActive(false);
        finalScorePanel.SetActive(true);
        finalScoreText.text = "Score: " + score + " / " + totalQuestions;
        scoreText.gameObject.SetActive(true);

        PlayerPrefs.SetInt("Level6", 1); // Unlock Level 6
        PlayerPrefs.Save();

        int percentage = Mathf.RoundToInt(((float)score / totalQuestions) * 100);

        if (percentage >= 80)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = filledStar;
            //numbersVoice.PlayLevelCompleteSound();
        }
        else if (percentage >= 50)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = emptyStar;
            //numbersVoice.PlayLevelCompleteSound();
        }
        else if (percentage >= 20)
        {
            star1.sprite = filledStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
            //numbersVoice.PlayStarAwardSound();
        }
        else
        {
            star1.sprite = emptyStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
            //numbersVoice.PlayStarAwardSound();
        }

        // ✅ Show ad every 2 levels
        int levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
        levelsCompleted++;
        PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
        PlayerPrefs.Save();

        if (levelsCompleted % 2 == 0)
        {
            Debug.Log("Showing interstitial ad after 2 levels...");
            Initialize.Instance.LoadInterstitialAd();
            Initialize.Instance.ShowInterstitialAd();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

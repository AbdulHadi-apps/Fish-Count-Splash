using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class QuizGameLevel3 : MonoBehaviour
{
    public Image firstNumberImage;
    public Image secondNumberImage;
    public Image operationImage;           // Sprite between numbers (e.g. +)
    public Sprite plusSprite;              // Assigned in Inspector
    public NumbersVoice numbersVoice;
    public Text questionText;
    public Button[] answerButtons;
    public Text[] answerButtonTexts;
    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStar;
    public Sprite emptyStar;

    public GameObject finalScorePanel;
    public Text finalScoreText;
    public Text scoreText;
    public GameObject imagepanel;
    public Sprite[] numberSprites;         // Number images (1-10)
    private string[] numberWords = {
        "ONE", "TWO", "THREE", "FOUR", "FIVE",
        "SIX", "SEVEN", "EIGHT", "NINE", "TEN",
        "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN",
        "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN", "TWENTY"
    };

    private int correctAnswer;
    private int score = 0;
    private int totalQuestions = 0;
    public int totalRounds = 15;
    private bool isAnswering = false;

    private ColorBlock[] originalColors;

    void Start()
    {
        originalColors = new ColorBlock[answerButtons.Length];
        for (int i = 0; i < answerButtons.Length; i++)
        {
            originalColors[i] = answerButtons[i].colors;
        }

        finalScorePanel.SetActive(false);
        resultImage.gameObject.SetActive(false);
        operationImage.gameObject.SetActive(false);

        NextQuestion();
    }

    void NextQuestion()
    {
        if (totalQuestions >= totalRounds)
        {
            EndGame();
            return;
        }

        isAnswering = false;
        resultImage.gameObject.SetActive(false);
        operationImage.gameObject.SetActive(false);

        totalQuestions++;
        scoreText.text = "Score: " + score + "/" + totalQuestions;

        // Generate two random numbers (1-10)
        int a = Random.Range(1, 10); // 1 to 9
        int b = Random.Range(1, 11 - a); // 1 to (10 - a)
        correctAnswer = a + b;

        // Display number images
        firstNumberImage.sprite = numberSprites[a - 1];
        secondNumberImage.sprite = numberSprites[b - 1];

        // Show plus sprite
        operationImage.sprite = plusSprite;
        operationImage.gameObject.SetActive(true);

        questionText.text = "What is the sum of these numbers?";

        // Generate answer options
        List<int> options = new List<int> { correctAnswer };
        while (options.Count < 4)
        {
            int rand = Random.Range(2, 11); // possible sum range
            if (!options.Contains(rand))
                options.Add(rand);
        }

        // Shuffle
        for (int i = 0; i < options.Count; i++)
        {
            int temp = options[i];
            int randIndex = Random.Range(i, options.Count);
            options[i] = options[randIndex];
            options[randIndex] = temp;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = options[i];
            int index = i;
            answerButtonTexts[i].text = numberWords[answer - 1];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer, index));
            answerButtons[i].interactable = true;
            answerButtons[i].colors = originalColors[i];
        }
    }

    void CheckAnswer(int selectedAnswer, int index)
    {
        if (isAnswering) return;
        isAnswering = true;

        bool isCorrect = selectedAnswer == correctAnswer;

        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;
        resultImage.color = isCorrect ? Color.green : Color.red;
        resultImage.gameObject.SetActive(true);

        if (isCorrect)
        {
            score++;
            questionText.text = "Correct!";
            numbersVoice.PlayCorrectNumberSound(correctAnswer);
        }
        else
        {
            questionText.text = $"Wrong! It was {numberWords[correctAnswer - 1]}";
            numbersVoice.PlayWrongSound();
        }

        HighlightButton(index, isCorrect ? Color.green : Color.red);

        StartCoroutine(WaitAndContinue());
    }

    void HighlightButton(int index, Color color)
    {
        var cb = answerButtons[index].colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        answerButtons[index].colors = cb;
    }

    IEnumerator WaitAndContinue()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
        }

        NextQuestion();
    }

    void EndGame()
    {
        questionText.text = "Level Complete!";
        finalScorePanel.SetActive(true);
        finalScoreText.text = $"Score: {score} / {totalRounds}";
        imagepanel.SetActive(false);
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
        }
        PlayerPrefs.SetInt("Level4", 1); // Unlock Level 2
        PlayerPrefs.Save();

        int percentage = Mathf.RoundToInt(((float)score / totalQuestions) * 100);

        if (percentage >= 80)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = filledStar;
            numbersVoice.PlayLevelCompleteSound();

        }
        else if (percentage >= 50)
        {
            star1.sprite = filledStar;
            star2.sprite = filledStar;
            star3.sprite = emptyStar;
            numbersVoice.PlayLevelCompleteSound();

        }
        else if (percentage >= 20)
        {
            star1.sprite = filledStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
            numbersVoice.PlayStarAwardSound();

        }
        else
        {
            star1.sprite = emptyStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
            numbersVoice.PlayStarAwardSound();

        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

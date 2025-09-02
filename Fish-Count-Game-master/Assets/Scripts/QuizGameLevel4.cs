using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class QuizGameLevel4 : MonoBehaviour
{
    public Text instructionText;
    public Image[] sequenceImages;
    public Button[] answerButtons;
    public Text[] answerButtonTexts;
    public Sprite[] numberSprites;
    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public GameObject finalScorePanel;
    public Text finalScoreText;
    public Text scoreText;
    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStar;
    public Sprite emptyStar;
    private int[] sequenceNumbers = new int[4];
    private int questionPosition;
    private int correctAnswer;
    private int score = 0;
    public NumbersVoice numbersVoice;

    private int totalQuestions = 0;
    public int questions = 15;
    private string[] numberWords = {
   "ONE", "TWO", "THREE", "FOUR", "FIVE",
    "SIX", "SEVEN", "EIGHT", "NINE", "TEN"
    };

    private Color defaultResultColor;
    private ColorBlock[] originalButtonColorBlocks;
    private bool hasAnswered = false;

    void Start()
    {
        originalButtonColorBlocks = new ColorBlock[answerButtons.Length];
        for (int i = 0; i < answerButtons.Length; i++)
        {
            originalButtonColorBlocks[i] = answerButtons[i].colors;
            answerButtons[i].gameObject.SetActive(false);
        }

        resultImage.gameObject.SetActive(false);
        defaultResultColor = resultImage.color;
        finalScorePanel.SetActive(false);
        UpdateScoreUI();

        StartCoroutine(ShowSequenceAndAsk());
    }

    IEnumerator ShowSequenceAndAsk()
    {
        totalQuestions++;
        UpdateScoreUI();
        hasAnswered = false;

        // Generate 4 random numbers and show their sprites
        for (int i = 0; i < 4; i++)
        {
            sequenceNumbers[i] = Random.Range(1, 11);
            sequenceImages[i].sprite = numberSprites[sequenceNumbers[i] - 1];
            sequenceImages[i].gameObject.SetActive(true);
        }

        instructionText.text = "Observe these numbers carefully";
        yield return new WaitForSeconds(3f);

        // Hide images
        foreach (var img in sequenceImages)
            img.gameObject.SetActive(false);

        // Pick a position to ask about
        questionPosition = Random.Range(0, 4);
        correctAnswer = sequenceNumbers[questionPosition];
        instructionText.text = $"What was the {PositionText(questionPosition)} number?";

        // Generate options including correct answer
        List<int> options = new List<int> { correctAnswer };
        while (options.Count < 4)
        {
            int rand = Random.Range(1, 11);
            if (!options.Contains(rand)) options.Add(rand);
        }

        // Shuffle options
        for (int i = 0; i < options.Count; i++)
        {
            int tmp = options[i];
            int r = Random.Range(i, options.Count);
            options[i] = options[r];
            options[r] = tmp;
        }

        // Show answer buttons with number words
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int ans = options[i];
            int idx = i;
            answerButtonTexts[i].text = numberWords[ans - 1];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(ans, idx));
            answerButtons[i].colors = originalButtonColorBlocks[i];
            answerButtons[i].gameObject.SetActive(true);
        }
    }

    public void CheckAnswer(int selectedAnswer, int buttonIndex)
    {
        if (hasAnswered) return;
        hasAnswered = true;
        resultImage.gameObject.SetActive(true);

        bool correct = selectedAnswer == correctAnswer;
        if (correct)
        {
            score++;
            instructionText.text = "Correct!";
            resultImage.sprite = correctSprite;
            resultImage.color = Color.green;
            numbersVoice.PlayCorrectNumberSound(correctAnswer);

        }
        else
        {
            instructionText.text = $"Wrong! It was {numberWords[correctAnswer - 1]}";
            resultImage.sprite = wrongSprite;
            resultImage.color = Color.red;
            numbersVoice.PlayWrongSound();

        }

        UpdateScoreUI();
        StartCoroutine(FlashButton(buttonIndex, correct ? Color.green : Color.red));
        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator FlashButton(int idx, Color flashCol)
    {
        var btn = answerButtons[idx];
        var ob = originalButtonColorBlocks[idx];

        var cb = btn.colors;
        cb.normalColor = flashCol;
        cb.highlightedColor = flashCol;
        cb.pressedColor = flashCol;
        cb.selectedColor = flashCol;
        cb.disabledColor = flashCol;
        btn.colors = cb;

        yield return new WaitForSeconds(1f);
        btn.colors = ob;
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        resultImage.gameObject.SetActive(false);
        resultImage.color = defaultResultColor;

        foreach (var btn in answerButtons)
            btn.gameObject.SetActive(false);

        if (totalQuestions >= questions)
            EndGame();
        else
            StartCoroutine(ShowSequenceAndAsk());
    }

    void EndGame()
    {
       //instructionText.text = "Game Over!";
        finalScorePanel.SetActive(true);
        finalScoreText.text = $"Score: {score} / {totalQuestions}";

        foreach (var btn in answerButtons)
            btn.gameObject.SetActive(false);

        PlayerPrefs.SetInt("Level5", 1); // Unlock Level 2
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

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {score} / {totalQuestions}";
    }

    string PositionText(int i)
    {
        switch (i)
        {
            case 0: return "1st";
            case 1: return "2nd";
            case 2: return "3rd";
            case 3: return "4th";
            default: return "";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

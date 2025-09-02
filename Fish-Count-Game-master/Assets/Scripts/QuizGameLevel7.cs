using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuizGameLevel7 : MonoBehaviour
{
    public Image displayImage;                // To show number as image
    public Sprite[] numberSprites;            // 0 = One, 1 = Two, ..., 4 = Five

    public Text questionText;
    public Button[] answerButtons;
    public Text scoreText;

    public GameObject finalScorePanel;
    public Text finalScoreText;

    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStar;
    public Sprite emptyStar;
    public NumbersVoice numbersVoice;

    private List<int> numberSequence = new List<int>();
    private int correctAnswer;
    private int score = 0;
    private int totalQuestions = 0;
    private bool isAnswering = false;
    public int questions;
    private string[] numberWords = { "ONE", "TWO", "THREE", "FOUR", "FIVE" };
    public GameObject imagespanel;
    void Awake()
    {
        finalScorePanel.SetActive(false);
        resultImage.gameObject.SetActive(false);
        displayImage.gameObject.SetActive(false);
        SetupNewQuestion();
    }

    void SetupNewQuestion()
    {
        isAnswering = false;
        totalQuestions++;

        questionText.text = "Look Carefully...";
        scoreText.text = $"Score: {score}/{totalQuestions - 1}";

        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
            btn.GetComponentInChildren<Text>().text = "";
            btn.image.color = Color.white;
        }

        StartCoroutine(ShowNumberSequence());
    }

    IEnumerator ShowNumberSequence()
    {
        yield return new WaitForSeconds(1f);

        numberSequence.Clear();

        for (int i = 0; i < 10; i++)
        {
            int num = Random.Range(1, 6); // 1 to 5
            numberSequence.Add(num);

            displayImage.sprite = numberSprites[num - 1];
            displayImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            displayImage.gameObject.SetActive(false); // Blink effect
            yield return new WaitForSeconds(0.4f);
        }

        displayImage.gameObject.SetActive(false);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        correctAnswer = numberSequence
            .GroupBy(n => n)
            .OrderByDescending(g => g.Count())
            .First().Key;

        questionText.text = "Which number appeared most?";
        SetupAnswerButtons();
    }

    void SetupAnswerButtons()
    {
        HashSet<int> options = new HashSet<int> { correctAnswer };

        while (options.Count < 4)
        {
            int rand = Random.Range(1, 6);
            options.Add(rand);
        }

        List<int> optionList = new List<int>(options);

        // Shuffle the options so correct answer isn't always first
        for (int i = 0; i < optionList.Count; i++)
        {
            int temp = optionList[i];
            int randIndex = Random.Range(i, optionList.Count);
            optionList[i] = optionList[randIndex];
            optionList[randIndex] = temp;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = optionList[i];
            Button btn = answerButtons[i];

            btn.GetComponentInChildren<Text>().text = numberWords[answer - 1];
            btn.image.color = Color.white;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => CheckAnswer(answer));
            btn.interactable = true;
        }

        scoreText.text = $"Score: {score}/{totalQuestions - 1}";
    }

    void CheckAnswer(int selected)
    {
        if (isAnswering) return;
        isAnswering = true;

        bool isCorrect = (selected == correctAnswer);

        foreach (Button btn in answerButtons)
        {
            string btnText = btn.GetComponentInChildren<Text>().text;
            int btnValue = System.Array.IndexOf(numberWords, btnText) + 1;

            if (btnValue == selected)
                btn.image.color = isCorrect ? Color.green : Color.red;
        }

        resultImage.gameObject.SetActive(true);
        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;
        resultImage.color = isCorrect ? Color.green : Color.red;

        questionText.text = isCorrect ? "Correct!" : $"Wrong! It was {numberWords[correctAnswer - 1]}";

        if (isCorrect)
        {
            score++;
            numbersVoice.PlayCorrectNumberSound(correctAnswer);

        }
        else
        {
            numbersVoice.PlayWrongSound();

        }
        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        resultImage.gameObject.SetActive(false);

        if (totalQuestions >= questions)
        {
            EndGame();
        }
        else
        {
            SetupNewQuestion();
        }
    }

    void EndGame()
    {
        displayImage.gameObject.SetActive(false);
       // questionText.text = "Game Over!";
        finalScorePanel.SetActive(true);
        imagespanel.SetActive(false);
        finalScoreText.text = $" Score: {score} / {totalQuestions}";

        PlayerPrefs.SetInt("Level8", 1); // Unlock Level 2
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

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class QuizGameLevel8 : MonoBehaviour
{
    public Image[] numberImages; // 4 image holders for number sprites
    public Sprite[] numberSprites; // Sprites from 1 to 10 (index 0 = 1, index 9 = 10)
    public Button[] answerButtons; // 4 answer buttons

    public Text questionText;
    public Text scoreText;
    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStar;
    public Sprite emptyStar;
    public NumbersVoice numbersVoice;

    public GameObject finalScorePanel;
    public Text finalScoreText;
    public int questions;
    private int correctAnswer;
    private int score = 0;
    private int totalQuestions = 0;
    private bool isAnswering = false;
    public GameObject imagespanels;
    private string[] numberWords = {"ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", };
                                                                          
    void Start()
    {
        finalScorePanel.SetActive(false);
        resultImage.gameObject.SetActive(false);
        SetupQuestion();
    }

    void SetupQuestion()
    {
        isAnswering = false;
        totalQuestions++;
        questionText.text = "Which number is the largest?";

        List<int> usedNumbers = new List<int>();

        // Assign random unique numbers to 4 image slots
        for (int i = 0; i < numberImages.Length; i++)
        {
            int num;
            do
            {
                num = Random.Range(1, 11); // 1 to 10
            } while (usedNumbers.Contains(num));

            usedNumbers.Add(num);
            numberImages[i].sprite = numberSprites[num - 1];
            numberImages[i].gameObject.SetActive(true);
        }

        correctAnswer = Mathf.Max(usedNumbers.ToArray());

        // Shuffle the answer options
        List<int> options = new List<int>(usedNumbers);
        for (int i = 0; i < options.Count; i++)
        {
            int temp = options[i];
            int rand = Random.Range(i, options.Count);
            options[i] = options[rand];
            options[rand] = temp;
        }

        // Assign options to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = options[i];
            Button btn = answerButtons[i];
            btn.GetComponentInChildren<Text>().text = numberWords[answer - 1];
            btn.image.color = Color.white;
            btn.interactable = true;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => CheckAnswer(answer));
        }

        UpdateScore();
    }

    void CheckAnswer(int selected)
    {
        if (isAnswering) return;
        isAnswering = true;

        bool isCorrect = selected == correctAnswer;

        foreach (Button btn in answerButtons)
        {
            string btnText = btn.GetComponentInChildren<Text>().text;
            int value = System.Array.IndexOf(numberWords, btnText) + 1;

            if (value == selected)
            {
                btn.image.color = isCorrect ? Color.green : Color.red;
            }
        }

        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;
        resultImage.color = isCorrect ? Color.green : Color.red;
        resultImage.gameObject.SetActive(true);

        if (isCorrect)
        {
            score++;
            numbersVoice.PlayCorrectNumberSound(correctAnswer);

        }
        else
        {
            numbersVoice.PlayWrongSound();

        }

        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(2.5f);
        resultImage.gameObject.SetActive(false);

        if (totalQuestions >= questions)
        {
            EndGame();
        }
        else
        {
            SetupQuestion();
        }
    }

    void EndGame()
    {
        foreach (var img in numberImages)
            img.gameObject.SetActive(false);

        foreach (var btn in answerButtons)
        {
            btn.interactable = false;
            btn.GetComponentInChildren<Text>().text = "";
        }

        finalScorePanel.SetActive(true);
        imagespanels.SetActive(false);
        finalScoreText.text = $"Score: {score} / {totalQuestions}";
        //questionText.text = "Game Over!";

        PlayerPrefs.SetInt("ShouldResetLevelsNextTime", 1); // <-- Just mark to reset later
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

    void UpdateScore()
    {
        scoreText.text = $"Score: {score} / {totalQuestions - 1}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
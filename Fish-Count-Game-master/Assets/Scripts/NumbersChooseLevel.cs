using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour {

    public void LoadLevelOne()
    {
        SceneManager.LoadScene("NumbersGameLevel1");
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("NumbersGameLevel2");
    }
    public void LoadLevelThree()
    {
        SceneManager.LoadScene("NumbersGameLevel3");
    }
    public void LoadLevelFour()
    {
        SceneManager.LoadScene("NumbersGameLevel4");

    }
    public void LoadLevelFive()
    {
        SceneManager.LoadScene("NumbersGameLevel5");

    }
    public void LoadLevelSix()
    {
        SceneManager.LoadScene("NumbersGameLevel6");

    }
    public void LoadLevelSeven()
    {
        SceneManager.LoadScene("NumbersGameLevel7");

    }
    public void LoadLevelEight()
    {
        SceneManager.LoadScene("NumbersGameLevel8");

    }
    public void GoBack()
    {
        SceneManager.LoadScene("NumbersLearning");
    }
}

using DG.Tweening;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePopAnimation : MonoBehaviour
{
    public TransitionSettings transition;
    public float startDelay;


    public void LoadScene(string _sceneName)
    {
        TransitionManager.Instance().Transition(_sceneName, transition, startDelay);
    }


    public void LoadByIndex(string _sceneName)
    {

        TransitionManager.Instance().Transition(_sceneName, transition, startDelay);

    }
    //public void LoadByIndex1(string _sceneName)
    //{

    //    TransitionManager.Instance().Transition(_sceneName, transition, startDelay);

    //}
}

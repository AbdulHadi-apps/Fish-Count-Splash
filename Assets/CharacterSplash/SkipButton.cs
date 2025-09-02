using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    public TransitionSettings transition;
    public float startDelay;
    public void SkipSplash()
    { 
            TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex+1, transition, startDelay);
    }
}
 
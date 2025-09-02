using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class SplashScreen : MonoBehaviour
{
    public string SceneName;
    public TransitionSettings transition;
    public float startDelay;
    private void Start()
    {
        Invoke("LoadScene", 7.8f);
    }
    private void LoadScene()
    {
        //SceneManager.LoadScene(SceneName);
        TransitionManager.Instance().Transition(SceneName, transition, startDelay);
    }
}

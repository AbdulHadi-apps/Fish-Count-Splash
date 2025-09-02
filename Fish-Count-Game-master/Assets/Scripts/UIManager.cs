using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingPanel;
    public  AudioSource source1;
    public AudioClip clip1;

    private void Start()
    {
        // Panel initially hidden
        settingPanel.transform.localScale = Vector3.zero;
        settingPanel.SetActive(false);
    }

    //public void OpenSettings()
    //{

    //    settingPanel.SetActive(true);
    //    source1.clip = clip1;
    //    source1.Play();
    //    settingPanel.transform.localScale = Vector3.zero;
    //    settingPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    //    Time.timeScale = 0f;
    //}
    public void OpenSettings()
    {
        settingPanel.SetActive(true);
        settingPanel.transform.localScale = Vector3.zero;

        // Play sound
        source1.clip = clip1;
        source1.Play();

        // Animate scale with UnscaledTime
        settingPanel.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // 👉 this makes it use UnscaledTime

        // Stop game after animation is done
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Time.timeScale = 0f;
        }).SetUpdate(true);
    }


    public void CloseSettings()
    {
        source1.clip = clip1;
        source1.Play();
        settingPanel.transform
            .DOScale(Vector3.zero, 0.4f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                settingPanel.SetActive(false);
                Time.timeScale = 1f;
            });
    }


    public void HomeButton()
    {
       
        SceneManager.LoadScene(0);
    }

    //public void settingButton()
    //{
    //    source1.clip = clip1;
    //    source1.Play();
    //}

}

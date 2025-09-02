using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ResetUIHandler : MonoBehaviour
{
    public GameObject resetPanel; // Assign this in the Inspector
    public GameObject levelsettingPanel;


    private void Start()
    {
        resetPanel.transform.localScale = Vector3.zero;
        resetPanel.SetActive(false);
    }

    public void ShowResetPanel()
    {
        if (resetPanel != null)
        {
            resetPanel.SetActive(true);
            levelsettingPanel.SetActive(false );
            resetPanel.transform
           .DOScale(Vector3.one, 0.5f)
           .SetEase(Ease.OutBack)
           .SetUpdate(true); // 👉 this makes it use UnscaledTime

            // Stop game after animation is done
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Time.timeScale = 0f;
            }).SetUpdate(true);
        }
    }

    public void HideResetPanel()
    {
        if (resetPanel != null)
        {
            resetPanel.transform
            .DOScale(Vector3.zero, 0.1f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                resetPanel.SetActive(false);
                Time.timeScale = 1f;
            });
            levelsettingPanel.SetActive(true);
            levelsettingPanel.transform.localScale = Vector3.zero;

            levelsettingPanel.transform
           .DOScale(Vector3.one, 0.7f)
           .SetEase(Ease.OutBack)
           .SetUpdate(true); // 👉 this makes it use UnscaledTime

            // Stop game after animation is done
            DOVirtual.DelayedCall(0.5f, () =>
            {
               
                Time.timeScale = 0f;
            }).SetUpdate(true);
        }
    }

    public void ConfirmReset()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetLevelsFromButton();
        }
    }
   
}

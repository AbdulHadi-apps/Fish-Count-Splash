using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextPopAnime : MonoBehaviour
{
    public Text targetText; // Assign the button's child Text manually in Inspector

    public void buttonPress()
    {
        AnimatePop();
    }

    public void AnimatePop()
    {
        if (targetText == null) return;

        targetText.transform.localScale = Vector3.one;

        targetText.transform.DOScale(1.4f, 0.4f)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() =>
                 {
                     targetText.transform.DOScale(1f, 0.3f)
                              .SetEase(Ease.InBack);
                 });
    }
}

using UnityEngine;

public class RemoveButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       DisableAdsButton();
    }
    
    public void DisableAdsButton()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void TriggerRemoveAds()
    {
        Initialize.Instance.RemoveBannerAds();
        Initialize.Instance.RemoveAds();
    }
}

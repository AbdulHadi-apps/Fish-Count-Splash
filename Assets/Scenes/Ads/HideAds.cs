using UnityEngine;

public class HideAds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize.Instance.hideBannerAd();
        Initialize.Instance.RemoveBannerAds();
        Debug.Log("REMOVE");
    }

    public void HideBannerBtn()
    {
        Initialize.Instance.RemoveBannerAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

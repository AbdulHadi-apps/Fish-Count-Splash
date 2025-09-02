using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize.Instance.LoadBannerAd();
        //Initialize.Instance.ShowBannerAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class RewardsAdManager : MonoBehaviour
{
    public int adcounter = 0;
    public static RewardsAdManager instance = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowRewardAd()
    {
        if (adcounter == 5) {
            Initialize.Instance.LoadInterstitialAd();
            Initialize.Instance.ShowInterstitialAd();
            adcounter = 0;
        }
        else
        {
            adcounter++;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public enum LoadGoogleAds { Yes, No }
public enum LoadUnityAds { Yes, No }

public class Initialize : MonoBehaviour
{

    [SerializeField] AppOpenAdController appOpenAdController;

    public static Initialize Instance = null;

    [SerializeField] LoadGoogleAds useAdmob;
    [SerializeField] LoadUnityAds useUnity;
    private bool allowAds;

    [SerializeField] string BannerID1, BannerID2, InterstitialID, RewardedID, RewardedInterstitialID, AppOpenID;
    [SerializeField] string Unity_ID;
    [SerializeField] string VIDEO_PLACEMENT, REWARDED_VIDEO_PLACEMENT;

    BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private NativeOverlayAd _nativeOverlayAd;


    [SerializeField] bool testMode = true;
    private static bool? _isInitialized;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    private void Start()
    {
        if (testMode)
        {
            BannerID1 = "ca-app-pub-3940256099942544/6300978111";
            BannerID2 = "ca-app-pub-3940256099942544/6300978111";
            InterstitialID = "ca-app-pub-3940256099942544/1033173712";
            RewardedID = "ca-app-pub-3940256099942544/5224354917";
            RewardedInterstitialID = "ca-app-pub-3940256099942544/5354046379";
            AppOpenID = "ca-app-pub-3940256099942544/9257395921";
        }
        // On Android, Unity is paused when displaying interstitial or rewarded video.
        // This setting makes iOS behave consistently with Android.
        MobileAds.SetiOSAppPauseOnBackground(true);
        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
        MobileAds.RaiseAdEventsOnUnityMainThread = true;


       InitializeGoogleMobileAds();
        //InitializeAds();
       // InitializeUnityAds();
        //appOpenAdController.LoadAd();
    }

    private void InitializeGoogleMobileAds()
    {
        // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
        Debug.Log("Google Mobile Ads Initializing.");
        if (_isInitialized.HasValue)
        {
            return;
        }
        _isInitialized = false;
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.Log("Google Mobile Ads initialization failed.");
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");

            _isInitialized = true;
            //LoadBannerAd();
            // LoadBanner2Ad();
            //LoadInterstitialAd();
            //LoadRewardedAd();
           // appOpenAdController.LoadAd();
            //LoadAppOpenAd();
            //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        });
    }

    #region BannerAds
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");
        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyBannerView();
        }
        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(BannerID1, AdSize.Banner, AdPosition.Top);
    }
    public void LoadBannerAd()
    {
        if (bannerView == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
        bannerView.Show();
    }
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerView()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            bannerView.Destroy();
            bannerView = null;
        }
    }

    #endregion


    #region InterstitialAds
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.adobm");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(InterstitialID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.Log("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                // Logger("Interstitial ad loaded with response : " + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.Log("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }

    #endregion



    #region Google Rewarded
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(RewardedID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.Log("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(ad);
            });
    }
    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.Log("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion




    #region Unity Ads
    //utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

   /* void InitializeUnityAds()
    {
        if (Advertisement.isSupported)
        {
            DebugLog(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(Unity_ID, testMode, this);
    }

    void LoadUnityRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    void ShowUnityRewardedAd()
    {
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    void LoadNonRewardedUnityAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    void ShowNonRewardedUnityAd()
    {
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }*/

    #region Interface Implementations
    /*public void OnInitializationComplete()
    {
        DebugLog("Init Success");
        LoadNonRewardedUnityAd();
        LoadUnityRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }*/

    public void OnUnityAdsAdLoaded(string placementId)
    {
        DebugLog($"Load Success: {placementId}");
    }

    /*public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
        if (placementId == VIDEO_PLACEMENT)
        {
            LoadNonRewardedUnityAd();
        }
        else
        {
            LoadUnityRewardedAd();
           PopUpManager.instance.DisplayPopUp("Sorry!", "Please check your internet and try again after some time.");
        }
    }*/

    /*public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        DebugLog($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
        if (placementId == VIDEO_PLACEMENT)
        {
            LoadNonRewardedUnityAd();
        }
        else if (placementId == REWARDED_VIDEO_PLACEMENT)
        {
            UserCompletedRewardedVideo();
            LoadUnityRewardedAd();
        }
    }*/
    #endregion

    public void OnGameIDFieldChanged(string newInput)
    {
        Unity_ID = newInput;
    }

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }
    #endregion


    #region Public Methods
   /* public void ShowInterstitialAds()
    {
        if (useAdmob == LoadGoogleAds.Yes && interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else if (useUnity == LoadUnityAds.Yes)
        {
            Debug.Log("In");
            ShowNonRewardedUnityAd();
        }

        else { Debug.Log("No Ad Found"); }
    }*/
    public void ShowBannerAds()
    {
        if (!AdsEnabled())
        {
            return;
        }
        bannerView.Show();
        //bannerView2.Show();
    }
    public void hideBannerAd()
    {
        if (!AdsEnabled())
        {
            return;
        }
        if (bannerView != null)
        {
            bannerView.Hide();

        }
       /* if (bannerView2 != null)
        {
            bannerView2.Hide();

        }*/
    }
   [HideInInspector] public string rewardedName;
   /* public void ShowRewardedVideo(string name)
    {
        rewardedName = name;
        if (useAdmob == LoadGoogleAds.Yes && rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                UserCompletedRewardedVideo();
            });
        }

        else if (useUnity == LoadUnityAds.Yes)
        {
            Debug.Log("in rewarded");
            ShowUnityRewardedAd();
        }
    }
    public bool isRewardedVideoAvailable()
    {
        if (useAdmob == LoadGoogleAds.Yes && rewardedAd != null)
        {
            return true;
        }
        //else if (useAdmob == LoadGoogleAds.Yes && rewardedInterstitialAd != null)
        //{
        //    return true;
        //}
        else
        {
            ShowUnityRewardedAd();
            return true;
        }
    }
    public void UserCompletedRewardedVideo()
    {

        FindFirstObjectByType<GameManager>().mainRewardCompleted();
       *//* switch (rewardedName)
        {
            case "coin":
                FindObjectOfType<GameManager>().rewardedCoins();
                break;
            case "time":
                FindObjectOfType<GameManager>().rewardedTime();
                break;
        }*//*
    }*/
    public void MoreGames()
    {
        //Application.OpenURL("https://play.google.com/store/apps/dev?id=6324425771244810643");


    }
    public void PrivacyPolicy()
    {
        Application.OpenURL("https://appexgames.co/privacy%20policy/");

    }
    public void RateUs()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
    public void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        PlayerPrefs.Save();
    }

    public void RemoveBannerAds()
    {
        bannerView.Hide();
    }
    bool AdsEnabled()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0 && allowAds)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion


   /* public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowRewardedVideo("nam");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInterstitialAds();
        }
    }*/








}


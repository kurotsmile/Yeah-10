using com.unity3d.mediation;
using UnityEngine;
using UnityEngine.Events;

public class IronSourceAds : MonoBehaviour
{
    [Header("Config General")]
    public int count_step_show_interstitial = 5;
    private int count_step = 0;

    [Header("Emplement Ui Ads")]
    public GameObject[] emplement_Ads;

    [Header("Config IronSource")]
    public string app_key;
    public string id_banner;
    public string id_video;
    public string id_rewarded;
    public UnityAction onRewardedSuccess;
    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    private bool is_ads = false;

    public void On_Load()
    {
        if (PlayerPrefs.GetInt("is_ads", 0) == 0)
            this.is_ads = true;
        else
            this.is_ads = false;

        if (this.is_ads)
        {
            IronSource.Agent.validateIntegration();
            LevelPlay.Init(this.app_key,adFormats:new []{LevelPlayAdFormat.REWARDED});

            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        }
        this.Check_Emplement_Ads();
    }

    void EnableAds()
    {
        this.is_ads=true;
        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        CreateBannerAd();
        CreateInterstitialAd();
        this.LoadInterstitialAd();
    }

    void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: "+ config);
        EnableAds();
    }
    
    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: "+ error);
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void Check_Emplement_Ads()
    {
        if (this.emplement_Ads.Length != 0)
        {
            for (int i = 0; i < this.emplement_Ads.Length; i++)
            {
                if (this.is_ads)
                    this.emplement_Ads[i].SetActive(true);
                else
                    this.emplement_Ads[i].SetActive(false);
            }
        }
    }

    #region AdInfo Rewarded Video
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdOpenedEvent With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdAvailable With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdUnavailable");
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdShowFailedEvent With Error" + ironSourceError + "And AdInfo " + adInfo);
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        this.onRewardedSuccess?.Invoke();
        Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);
    }
    #endregion

    #region Banner Ads
    void CreateBannerAd()
    {
        bannerAd = new LevelPlayBannerAd(this.id_banner, LevelPlayAdSize.BANNER, LevelPlayBannerPosition.TopCenter);
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
        this.LoadBannerAd();
    }

    void LoadBannerAd()
    {
        if(bannerAd!=null) bannerAd.LoadAd();
    }
    public void ShowBannerAd()
    {
        if(bannerAd!=null) bannerAd.ShowAd();
    }
    public void HideBannerAd()
    {
        if(bannerAd!=null) bannerAd.HideAd();
    }
    public void DestroyBannerAd()
    {
        if(bannerAd!=null) bannerAd.DestroyAd();
    }

    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { 
        this.ShowBannerAd();
    }
    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError) { }
    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }
    #endregion

    public void ShowRewardedVideo()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo(this.id_rewarded);
        }
        else
        {
            Debug.Log("Quảng cáo chưa sẵn sàng.");
        }
    }

    #region InterstitialAd
    void CreateInterstitialAd()
    {
        interstitialAd = new LevelPlayInterstitialAd(this.id_video);
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }

    void LoadInterstitialAd()
    {
        interstitialAd.LoadAd();
    }

    public void show_ads_Interstitial()
    {
        if(this.is_ads){
            this.count_step++;
            if (this.count_step > this.count_step_show_interstitial)
            {
                this.count_step=0;
                this.ShowInterstitialAd();
            }
        }
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
    }

    void DestroyInterstitialAd()
    {
        interstitialAd.DestroyAd();
    }

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
    #endregion

    public void RemoveAds()
    {
        this.HideBannerAd();
        PlayerPrefs.SetInt("is_ads", 1);
        this.is_ads = false;
        this.Check_Emplement_Ads();
    }

    public bool get_status_ads()
    {
        return this.is_ads;
    }

    #region ImpressionSuccess callback handler

    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
    }
    #endregion

    private void OnDisable()
    {
        bannerAd?.DestroyAd();
        interstitialAd?.DestroyAd();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartboostSDK;
using AudienceNetwork;
using GoogleMobileAds.Api;
using System;

public class AdManager : PluginSingleton<AdManager>
{

    //----------------------------- FACEBOOK ---------------------------------//

    [System.Serializable]
    public class FacebookID
    {
        public string banner = "YOUR_PLACEMENT_ID";
        public string instertitial = "YOUR_PLACEMENT_ID";
    }

    public FacebookID androidFacebookID;
    public FacebookID iosFacebookID;
    private FacebookID facebookID;

    private AudienceNetwork.InterstitialAd interstitialAdFacebook;
    private AudienceNetwork.AdView adViewFacebook;
    private bool isFacebookLoaded;
    private bool isBannerFacebookLoaded;

    private void LoadBannerFacebook()
    {
#if UNITY_ANDROID || UNITY_IOS
        bnFB = "bannerFB inited";
        AudienceNetwork.AdView adView = new AudienceNetwork.AdView(facebookID.banner, AudienceNetwork.AdSize.BANNER_HEIGHT_50);
        this.adViewFacebook = adView;
        this.adViewFacebook.Register(this.gameObject);
        this.adViewFacebook.AdViewDidLoad = (delegate ()
        {
            isBannerFacebookLoaded = true;
            bnFB = "bannerFB loaded";
            ShowBanner();
        });
        this.adViewFacebook.AdViewDidFailWithError = (delegate (string error)
        {
            isBannerFacebookLoaded = false;
            this.adViewFacebook.Dispose();
            bnFB = "bannerFB fail to load";
            LoadBannerFacebook();
        });
        adView.LoadAd();
#endif
    }
    private void LoadInterstitialFacebook()
    {
#if UNITY_ANDROID || UNITY_IOS
        viFB = "Interstitial FB inited";
        AudienceNetwork.InterstitialAd interstitialAd = new AudienceNetwork.InterstitialAd(facebookID.instertitial);
        this.interstitialAdFacebook = interstitialAd;
        this.interstitialAdFacebook.Register(this.gameObject);
        this.interstitialAdFacebook.InterstitialAdDidLoad = (delegate ()
        {
            this.isFacebookLoaded = true;
            viFB = "Interstitial FB loaded";
        });
        this.interstitialAdFacebook.InterstitialAdDidClose = (delegate ()
        {
            this.isFacebookLoaded = false;
            viFB = "Interstitial FB new load";
            this.interstitialAdFacebook.Dispose();
            this.LoadInterstitialFacebook();
        });
        this.interstitialAdFacebook.InterstitialAdDidFailWithError = (delegate (string error)
        {
            this.isFacebookLoaded = false;
            this.interstitialAdFacebook.Dispose();
            viFB = "Interstitial FB fail to load";
            this.LoadInterstitialFacebook();
        });
        this.interstitialAdFacebook.LoadAd();
#endif
    }
    private void ShowBannerFacebook()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (this.isBannerFacebookLoaded)
        {
            this.adViewFacebook.Show(0);
        }
#endif
    }
    private void ShowInterstitialFacebook()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (this.isFacebookLoaded)
        {
            this.interstitialAdFacebook.Show();
        }
        else
        {
            this.LoadInterstitialFacebook();
        }
#endif
    }
    private void InitFacebook()
    {
#if UNITY_ANDROID
        facebookID = androidFacebookID;
#elif UNITY_IOS
		facebookID = iosFacebookID;
#endif

#if UNITY_EDITOR
#else
		LoadInterstitialFacebook();
		LoadBannerFacebook();
#endif
    }

    //-------------------------------- ADMOB -----------------------------------//

    [System.Serializable]
    public class AdmobID
    {
        public string banner = "YOUR_PLACEMENT_ID";
        public string instertitial = "YOUR_PLACEMENT_ID";
        public string rewardVideo = "YOUR_PLACEMENT_ID";
    }

    public AdmobID admobAndroid;
    public AdmobID admobIOS;
    private AdmobID admob;
    private bool isBannerAdmobLoaded;
    private GoogleMobileAds.Api.BannerView bannerView;
    private GoogleMobileAds.Api.InterstitialAd interstitial;
    private GoogleMobileAds.Api.RewardBasedVideoAd rewardVideo;

    private void LoadBannerAdmob()
    {
        this.bannerView = new GoogleMobileAds.Api.BannerView(admob.banner, GoogleMobileAds.Api.AdSize.SmartBanner, AdPosition.Top);
        if (this.bannerView != null)
        {
            bn = "Banner inited";
        }
        this.bannerView.LoadAd(new AdRequest.Builder().Build());
        this.bannerView.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            this.isBannerAdmobLoaded = true;
            this.bannerView.Hide();
            this.ShowBanner();
            bn = "Banner Loaded";
        });
        this.bannerView.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            bn = "Banner fail to load";
            this.bannerView.LoadAd(new AdRequest.Builder().Build());
        });
    }
    private void LoadInterstitialAdmob()
    {
        this.interstitial = new GoogleMobileAds.Api.InterstitialAd(admob.instertitial);
        if (this.interstitial != null)
        {
            vi = "interstitial inited";
        }
        this.interstitial.OnAdClosed += (delegate (System.Object sender, EventArgs args) {
            vi = "interstitial video new load";
            this.interstitial.LoadAd(new AdRequest.Builder().Build());
        });
        this.interstitial.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            vi = "Video Loaded";
        });
        this.interstitial.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            vi = "Video fail to load";
            this.bannerView.LoadAd(new AdRequest.Builder().Build());
        });
        this.interstitial.LoadAd(new AdRequest.Builder().Build());
    }
    private void LoadRewardVideoAdmob()
    {
        this.rewardVideo = RewardBasedVideoAd.Instance;
        if (this.rewardVideo != null)
        {
            rv = "rewardVideo inited";
        }
        this.rewardVideo.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video Loaded";
        });
        this.rewardVideo.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            rv = "Reward video fail to load";
            this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
        });
        this.rewardVideo.OnAdClosed += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video closed. Shame";
            Time.timeScale = 1f;
            this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
            GameManager.Instance.GameOver();
        });
        this.rewardVideo.OnAdRewarded += (delegate (System.Object sender, Reward reward)
        {
            rv = "Reward video get rewared";
            Time.timeScale = 1f;
            GameManager.Instance.ContinueOnLose();
        });
        this.rewardVideo.OnAdLeavingApplication += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video leaving";
            Time.timeScale = 0f;
            //GameManager.Instance.GameOver();
        });

        this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
    }
    private void ShowBannerAdmob()
    {
        if (isBannerAdmobLoaded)
        {
            this.bannerView.Show();
        }
    }
    private void ShowInterstitialAdmob()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }
    private void ShowRewardVideoAdmob()
    {
        if (this.rewardVideo.IsLoaded())
        {
            this.rewardVideo.Show();
        }
    }
    private void InitAdmob()
    {
#if UNITY_ANDROID
        this.admob = this.admobAndroid;
#elif UNITY_IOS
		this.admob = this.admobIOS;
#else
		this.admob = this.admobAndroid;
#endif
        LoadBannerAdmob();
        LoadInterstitialAdmob();
        LoadRewardVideoAdmob();
    }

    //------------------------------ CHARTBOOST --------------------------------//

    private void ShowInterstitialChartboost()
    {
        if (Chartboost.hasInterstitial(CBLocation.Default))
        {
            Chartboost.showInterstitial(CBLocation.Default);
        }
        else
        {
            Chartboost.cacheInterstitial(CBLocation.Default);
        }
    }
    private void ShowRewardVideoChartboost()
    {
        if (Chartboost.hasRewardedVideo(CBLocation.Default))
        {
            Chartboost.showRewardedVideo(CBLocation.Default);
        }
        else
        {
            Chartboost.cacheRewardedVideo(CBLocation.Default);
        }
    }
    private void InitChartboost()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
        Chartboost.cacheRewardedVideo(CBLocation.Default);
        Chartboost.didCloseRewardedVideo += (delegate (CBLocation obj)
        {
            Time.timeScale = 1f;
        });
        Chartboost.didCompleteRewardedVideo += (delegate (CBLocation obj, int amount)
        {
            Time.timeScale = 1f;
        });
        Chartboost.didDismissRewardedVideo += (delegate (CBLocation obj)
        {
            Time.timeScale = 1f;
        });
        Chartboost.didClickRewardedVideo += (delegate (CBLocation obj)
        {
            Time.timeScale = 0f;
        });
    }

    //---------------------------- MAIN FUNCTION -------------------------------//

    public bool banner = true;
    public bool video = true;
    private bool alreadyShowBanner = false;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        this.InitFacebook();
        this.InitAdmob();
        this.InitChartboost();
    }

    public void ShowBanner()
    {
        if (!UserProfile.Instance.HasAds() || !banner || alreadyShowBanner)
        {
            return;
        }

        if (isBannerFacebookLoaded)
        {
            ShowBannerFacebook();
            alreadyShowBanner = true;
        }
        else if (isBannerAdmobLoaded)
        {
            ShowBannerAdmob();
            alreadyShowBanner = true;
        }
    }
    public void ShowInterstitial()
    {
        if (!UserProfile.Instance.HasAds() || !video)
        {
            return;
        }

        if (isFacebookLoaded)
        {
            ShowInterstitialFacebook();
        }
        else if (interstitial.IsLoaded())
        {
            ShowInterstitialAdmob();
        }
        else if (Chartboost.hasInterstitial(CBLocation.Default))
        {
            Chartboost.showInterstitial(CBLocation.Default);
        }
    }
    public void ShowRewardVideo()
    {
        if (rewardVideo.IsLoaded())
        {
            ShowRewardVideoAdmob();
        }
        else if (Chartboost.hasRewardedVideo(CBLocation.Default))
        {
            ShowRewardVideoChartboost();
        }
    }
    public void RemoveBanner()
    {
        if (this.adViewFacebook != null)
        {
            this.adViewFacebook.Dispose();
        }
        else if (this.bannerView != null)
        {
            this.bannerView.Hide();
            this.bannerView.Destroy();
        }
    }

    public bool IsRewardedVideoLoaded()
    {
        return rewardVideo.IsLoaded() || Chartboost.hasRewardedVideo(CBLocation.Default);
    }
   
    string bn = "";
    string vi = "";
    string rv = "";
    string bnFB = "";
    string viFB = "";
    public bool testMode = false;
    public void OnGUI()
    {
        if (!testMode)
        {
            return;
        }

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1f, 1.0f);
        GUI.Label(new Rect(0, h - 1.5f * h * 2 / 100, w, h * 2 / 100), rv, style);
        GUI.Label(new Rect(0, h - 2.5f * h * 2 / 100, w, h * 2 / 100), vi, style);
        GUI.Label(new Rect(0, h - 3.5f * h * 2 / 100, w, h * 2 / 100), bn, style);
        GUI.Label(new Rect(0, h - 4.5f * h * 2 / 100, w, h * 2 / 100), viFB, style);
        GUI.Label(new Rect(0, h - 5.5f * h * 2 / 100, w, h * 2 / 100), bnFB, style);
    }
}

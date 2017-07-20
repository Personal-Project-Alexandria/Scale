using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using ChartboostSDK;

public class AdManager : MonoSingleton<AdManager> {
	
	private string bannerId;
	private string videoId;
	private string rewardId;

	private BannerView bannerView;
	private InterstitialAd interstitial;
	private RewardBasedVideoAd rewardBasedVideo;

	private int rewardType = 0; // 0 = move, 1 = health 
	private const int RESPAWN_BANNER_TIME = 60;
	private float second;

	//#if UNITY_ANDROID || UNITY_IOS

	protected void Awake()
	{
		Initialize();
		LoadBanner();
		LoadVideo();
		LoadRewardedVideo();

		second = RESPAWN_BANNER_TIME;
	}

	protected void Update()
	{
		// Show ads every 5 minute = 300 seconds
		second += Time.deltaTime;
		if (second >= RESPAWN_BANNER_TIME)
		{
			ShowBanner();
			second = 0f;
		}
	}

	public void Initialize()
	{
		DontDestroyOnLoad(gameObject);

		Chartboost.Create();
		Chartboost.setAutoCacheAds(true);
		Chartboost.cacheInterstitial(CBLocation.HomeScreen);
		Chartboost.cacheRewardedVideo(CBLocation.HomeScreen);
		Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;

#if UNITY_ANDROID
		bannerId = "ca-app-pub-8138314746899986/1452225155";
		videoId = "ca-app-pub-8138314746899986/2928958354";
		rewardId = "ca-app-pub-8138314746899986/5882424756";

#elif UNITY_IOS
		bannerId = "ca-app-pub-8138314746899986/1591825952";
		videoId = "ca-app-pub-8138314746899986/4545292358";
		rewardId = "ca-app-pub-8138314746899986/6022025552";
#endif
	}

	private void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
	{
		if (this.rewardType == 0)
		{
			
		}
		else if (this.rewardType == 1)
		{
			
		}

		throw new System.NotImplementedException();
	}

	/* ------------------------------- BANNER ---------------------------------*/

	public void LoadBanner()
	{
		this.bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
		
		this.bannerView.LoadAd(new AdRequest.Builder().Build());

		this.bannerView.OnAdClosed += this.OnCloseBannerHandle;
	
		this.bannerView.Hide();
	}

	public void ShowBanner()
	{
		if (UserProfile.Instance.HasAds() && Application.internetReachability != NetworkReachability.NotReachable)
		{
			bannerView.Show();
		}
	}

	public void HideBanner()
	{
		this.bannerView.Hide();

		this.bannerView.LoadAd(new AdRequest.Builder().Build());
	}

	public void OnCloseBannerHandle(object sender, EventArgs args)
	{
		this.HideBanner();
	}

	/* ---------------------------- REWARD VIDEO ------------------------------*/

	private bool isRewarded = false;

	public void LoadRewardedVideo()
	{
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		this.rewardBasedVideo.OnAdClosed += this.OnCloseRewardVideoHandle;

		//this.rewardBasedVideo.OnAdRewarded += this.OnGetRewardHandle;

		this.rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), rewardId);
	}

	public void ShowRewardedVideo(int type = 0)
	{
		this.rewardType = type;

		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			if (this.rewardBasedVideo.IsLoaded())
			{
				if (this.rewardType == 0)
				{
					this.rewardBasedVideo.OnAdRewarded += this.OnGetRewardHandle;
				}
				else
				{
					this.rewardBasedVideo.OnAdRewarded += this.OnGetRewardHealthHandle;
				}

				this.rewardBasedVideo.Show();
			}
			else if (Chartboost.hasRewardedVideo(CBLocation.HomeScreen))
			{
				Chartboost.showRewardedVideo(CBLocation.HomeScreen);
			}
			else
			{
				Chartboost.cacheRewardedVideo(CBLocation.HomeScreen);
			}
		}
	}

	public void OnGetRewardHandle(object sender, EventArgs args)
	{
		isRewarded = true;
		GameManager.Instance.ContinueOnLose();
		this.rewardBasedVideo.OnAdRewarded -= this.OnGetRewardHandle;
	}

	public void OnGetRewardHealthHandle(object sender, EventArgs args)
	{
		this.rewardBasedVideo.OnAdRewarded -= this.OnGetRewardHealthHandle;
	}

	public void OnCloseRewardVideoHandle(object sender, EventArgs args)
	{
		if (!isRewarded)
		{
			GameManager.Instance.GameOver();
		}
		isRewarded = false;
		this.rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), rewardId);
	}

	public bool IsRewardedVideoLoaded()
	{
		return (this.rewardBasedVideo.IsLoaded() || Chartboost.hasRewardedVideo(CBLocation.HomeScreen)) 
			&& Application.internetReachability != NetworkReachability.NotReachable;
	}

	/* ---------------------------- INTERSTITIAL ------------------------------*/

	public void LoadVideo()
	{
		this.interstitial = new InterstitialAd(videoId);

		this.interstitial.OnAdClosed += OnCloseVideoHandle;

		this.interstitial.LoadAd(new AdRequest.Builder().Build());
	}

	public void ShowVideo()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable && UserProfile.Instance.HasAds())
		{
			if (this.interstitial.IsLoaded())
			{
				this.interstitial.Show();
			}
			else if (Chartboost.hasInterstitial(CBLocation.HomeScreen))
			{
				Chartboost.showInterstitial(CBLocation.HomeScreen);
			}
			else
			{
				Chartboost.cacheInterstitial(CBLocation.HomeScreen);
			}
		}
	}

	public void OnCloseVideoHandle(object sender, EventArgs args)
	{
		this.interstitial.LoadAd(new AdRequest.Builder().Build());
	}

	//#else

	//	protected void Start()
	//	{

	//	}

	//	public void Initialize()
	//	{

	//	}

	//	/* ------------------------------- BANNER ---------------------------------*/

	//	public void LoadBanner()
	//	{
	//		Debug.Log("NO");
	//	}

	//	public void ShowBanner()
	//	{
	//	}

	//	/* ---------------------------- REWARD VIDEO ------------------------------*/

	//	public void LoadRewardedVideo()
	//	{

	//	}

	//	public void ShowRewardedVideo()
	//	{

	//	}

	//	public void OnCloseRewardVideoHandle(object sender, EventArgs args)
	//	{

	//	}

	//public void OnGetRewardHealthHandle(object sender, EventArgs args)
	//{
	//	
	//}

	//	public bool IsRewardedVideoLoaded()
	//	{
	//		return false;
	//	}

	//	/* ---------------------------- INTERSTITIAL ------------------------------*/

	//	public void LoadVideo()
	//	{

	//	}

	//	public void ShowVideo()
	//	{

	//	}

	//	public void OnCloseVideoHandle(object sender, EventArgs args)
	//	{

	//	}

	//#endif
}
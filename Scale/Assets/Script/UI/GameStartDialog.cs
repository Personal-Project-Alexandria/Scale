using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartboostSDK;

public class GameStartDialog : BaseDialog {

	public Text highScore;
	public Button noAdsButton;
	public Button soundButton;
	
	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		highScore.text = UserProfile.Instance.GetHighScore().ToString();
		AdManager.Instance.ShowBanner();
		Setup();
	}

	public void Setup()
	{
		if (UserProfile.Instance.HasAds())
		{
			noAdsButton.GetComponent<Image>().sprite = UserProfile.Instance.hasAds;
			noAdsButton.interactable = true;
		}
		else
		{
			noAdsButton.GetComponent<Image>().sprite = UserProfile.Instance.noAds;
			noAdsButton.interactable = false;
		}

		if (SoundManager.Instance.IsBackgroundPlaying())
		{
			soundButton.GetComponent<Image>().sprite = UserProfile.Instance.hasSound;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = UserProfile.Instance.noSound;
		}
	}

	public void OnClickPlay()
    {
        GamePlayDialog dialog = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
        this.OnCloseDialog();
    }

    public void OnClickNoAds()
    {
		NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.NOADS, noAdsButton);
	}

    public void OnClickShare()
    {
		Debug.Log("HERE");
		if (Chartboost.hasInterstitial(CBLocation.Default))
		{
			Chartboost.showInterstitial(CBLocation.Default);
		}
		else
		{
			Chartboost.cacheInterstitial(CBLocation.Default);
		}
	}

    public void OnClickShop()
    {
        StoreDialog dialog = GUIManager.Instance.OnShowDialog<StoreDialog>("Store");
    }

    public void OnClickLeaderBoard()
	{
		Debug.Log("HERE 2");
		if (Chartboost.hasRewardedVideo(CBLocation.Default))
		{
			Chartboost.showRewardedVideo(CBLocation.Default);
		}
		else
		{
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}
	}

	public void OnClickSound()
    {
		SoundManager.Instance.ToggleMusic(!SoundManager.Instance.IsBackgroundPlaying());
		if (SoundManager.Instance.IsBackgroundPlaying())
		{
			soundButton.GetComponent<Image>().sprite = UserProfile.Instance.hasSound;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = UserProfile.Instance.noSound;
		}
	}
}
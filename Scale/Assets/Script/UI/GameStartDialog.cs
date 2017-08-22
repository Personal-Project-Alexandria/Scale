using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using DG.Tweening;

public class GameStartDialog : BaseDialog {
	
	public Text mode;
	public Text highScore;
	public Button noAdsButton;
	public Button soundButton;
	public Image playButton;
	public Image highScoreIcon; 

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		highScore.text = UserProfile.Instance.GetHighScore(GameManager.Instance.mode).ToString();
		if (GameManager.Instance.mode == 0)
		{
			mode.text = "classic";
		}
		else if (GameManager.Instance.mode == 1)
		{
			mode.text = "3 slices";
		}
		else
		{
			mode.text = "multiballs";
		}
		AdManager.Instance.ShowBanner();
		Setup();

		if (!(this is GameOverDialog))
		{
			highScoreIcon.transform.DOShakeRotation(5f, new Vector3(0, 0, 10f), 0).SetLoops(-1, LoopType.Restart);
		}
	}

	public void Setup()
	{
		if (!UserProfile.Instance.HasAds())
		{
			noAdsButton.gameObject.SetActive(false);
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
		FBManager.Instance.Login();
	}

    public void OnClickShop()
    {
        StoreDialog dialog = GUIManager.Instance.OnShowDialog<StoreDialog>("Store");
    }

    public void OnClickLeaderBoard()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			LeaderDialog leader = GUIManager.Instance.OnShowDialog<LeaderDialog>("Leader");
		}
	}

	public void OnClickCommit()
	{
		if (FB.IsLoggedIn)
		{
			LeaderBoard.Instance.UploadHighscore(GameManager.Instance.mode);
		}
		else
		{
			FBManager.Instance.Login();
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

	public void OnModeClick()
	{
		ModeDialog modeDialog = GUIManager.Instance.OnShowDialog<ModeDialog>("Mode");
	}
}
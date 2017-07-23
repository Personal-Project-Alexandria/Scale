using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDialog : BaseDialog {

	public Text bestScore;
	public Text score;
	public Text addedDiamond;
	public Text diamond;
	public Button soundButton;
	public Button noAdsButton;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		SetAllText();
		Setup();
        StartCoroutine( ShowRate());

    }

	public override void OnHide()
	{
		StopAllCoroutines();
		base.OnHide();
	}
    IEnumerator ShowRate()
    {
        yield return new WaitForSeconds(0.5f);
        int isShow = Random.Range(0, 0);
        if (isShow == 0)
        {
            RateDialog dialog = GUIManager.Instance.OnShowDialog<RateDialog>("Rate");
        }
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

	public void SetAllText()
	{
		bestScore.text = UserProfile.Instance.GetHighScore(GameManager.Instance.mode).ToString();
		score.text = GameManager.Instance.level.ToString();
		addedDiamond.text = "+" + (10 * GameManager.Instance.level).ToString();
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
		StartCoroutine(HideAddedDiamond(2f));
	}

	IEnumerator HideAddedDiamond(float time)
	{
		addedDiamond.gameObject.SetActive(true);
		yield return new WaitForSeconds(time);
		addedDiamond.gameObject.SetActive(false);
	}

	public void OnClickPlay()
	{
		this.OnHide();
		GamePlayDialog play = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
	}

	public void OnClickGetDiamond()
	{

	}

	public void OnClickStore()
	{
		StoreDialog store = GUIManager.Instance.OnShowDialog<StoreDialog>("Store");
	}

	public void OnClickNoAds()
	{
		NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.NOADS, noAdsButton);
	}

	public void OnClickLeaderBoard()
	{

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

	public void OnClickShare()
	{

	}

	
}

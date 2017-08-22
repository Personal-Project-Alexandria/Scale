using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLifeDialog : BaseDialog {

	private int diamondCost = 150;
	private bool close = false;

	public Text timeRemain;
	public Button adsButton;
	public Image adsButtonSprite;
	public Sprite available;
	public Sprite unavailable;

	public Button diamondButton;
	public Image diamondButtonImage;
	public Sprite d_available;
	public Sprite d_unavailable;

	private float time;
	
	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		time = 3f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (time <= 0f && !close)
		{
			OnClickClose();
			return;
		}

		time -= Time.deltaTime;
		timeRemain.text = Mathf.RoundToInt(time).ToString();

		if (AdManager.Instance.IsRewardedVideoLoaded())
		{
			adsButton.interactable = true;
			adsButtonSprite.sprite = available;
		}
		else
		{
			adsButton.interactable = false;
			adsButtonSprite.sprite = unavailable;
		}

		if (UserProfile.Instance.GetDiamond() >= diamondCost)
		{
			diamondButton.interactable = true;
			diamondButtonImage.sprite = d_available;
		}
		else
		{
			diamondButton.interactable = false;
			diamondButtonImage.sprite = d_unavailable;
		}
	}

	public void OnClickBuyLifeByDiamond()
	{
		if (UserProfile.Instance.ReduceDiamond(diamondCost))
		{
			GameManager.Instance.ContinueOnLose();
			this.OnCloseDialog();
		}
	}

	public void OnClickBuyLifeByAds()
	{
		AdManager.Instance.ShowRewardedVideo();
		this.OnCloseDialog();
	}

	public void OnClickClose()
	{
		this.OnCloseDialog();
		close = true;
		GameManager.Instance.GameOver();
	}
}

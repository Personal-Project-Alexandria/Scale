using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDialog : BaseDialog {

	public Text bestScore;
	public Text score;
	public Text addedDiamond;
	public Text diamond;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		SetAllText();
	}

	public override void OnHide()
	{
		StopAllCoroutines();
		base.OnHide();
	}

	public void SetAllText()
	{
		Debug.Log(UserProfile.Instance.GetHighScore());
		bestScore.text = UserProfile.Instance.GetHighScore().ToString();
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

	}

	public void OnClickLeaderBoard()
	{

	}

	public void OnClickSound()
	{

	}

	public void OnClickShare()
	{

	}
}

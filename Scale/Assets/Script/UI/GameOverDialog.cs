using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class GameOverDialog : GameStartDialog {

	public Text score;
	public Text addedDiamond;
	public Text diamond;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		SetAllText();
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
        int isShow = Random.Range(0, 10);
        if (isShow == 0)
        {
            RateDialog dialog = GUIManager.Instance.OnShowDialog<RateDialog>("Rate");
        }
    }

	public void SetAllText()
	{
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
		highScore.text = UserProfile.Instance.GetHighScore(GameManager.Instance.mode).ToString();
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
}

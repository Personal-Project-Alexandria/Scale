using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLifeDialog : BaseDialog {

	public Text timeRemain;
	private float time;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		time = 3f;
	}

	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		timeRemain.text = Mathf.RoundToInt(time).ToString();

		if (time <= 0f)
		{
			OnClickClose();
		}
	}

	public void OnClickBuyLifeByDiamond()
	{
		if (UserProfile.Instance.ReduceDiamond(0))
		{
			GameManager.Instance.ContinueOnLose();
			OnHide();
		}
	}

	public void OnClickBuyLifeByAds()
	{

	}

	public void OnClickClose()
	{
		GameManager.Instance.GameOver();
		this.OnHide();
	}
}

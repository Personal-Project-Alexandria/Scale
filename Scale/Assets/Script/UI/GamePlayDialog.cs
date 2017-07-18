﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayDialog : BaseDialog
{
	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);

		GameManager.Instance.StartGame();
	}

	public override void OnHide()
	{
		base.OnHide();

		GameManager.Instance.EndGame();
	}

	public void OnClickPause()
	{
		PauseDialog dialog = GUIManager.Instance.OnShowDialog<PauseDialog>("Pause");
	}

	// ------------------------- TOPBAR GUI ------------------------ //

	public Text life;
	public Text level;
	public Text diamond;
	public Slider slider;

	protected void Update()
	{
		life.text = GameManager.Instance.life.ToString();
		level.text = GameManager.Instance.level.ToString();
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
		slider.value = GameManager.Instance.percent;
	}
}


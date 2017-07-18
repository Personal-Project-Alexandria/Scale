using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDialog : BaseDialog {

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		GameManager.Instance.PauseGame();
	}

	public void OnClickRestart()
	{
		GameManager.Instance.RestartGame();
		this.OnHide();
	}

	public void OnClickSound()
	{

	}

	public void OnClickHelp()
	{

	}

	public void OnClickContinue()
	{
		GameManager.Instance.ContinueGame();
		this.OnHide();
	}
}

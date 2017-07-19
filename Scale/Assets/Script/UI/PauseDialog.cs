using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDialog : BaseDialog {

	public Button soundButton;
	public Sprite soundOff;
	public Sprite soundOn;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		GameManager.Instance.PauseGame();

		if (SoundManager.Instance.IsBackgroundPlaying())
		{
			soundButton.GetComponent<Image>().sprite = soundOn;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = soundOff;
		}
	}

	public void OnClickRestart()
	{
		GameManager.Instance.RestartGame();
		this.OnHide();
	}

	public void OnClickSound()
	{
		SoundManager.Instance.ToggleMusic(!SoundManager.Instance.IsBackgroundPlaying());
		if (SoundManager.Instance.IsBackgroundPlaying())
		{
			soundButton.GetComponent<Image>().sprite = soundOn;
		}
		else
		{
			soundButton.GetComponent<Image>().sprite = soundOff;
		}
	}

	public void OnClickHelp()
	{

	}

	public void OnClickContinue()
	{
		GameManager.Instance.ContinueGame();
		this.OnHide();
	}

	public void OnClickHome()
	{
		GameManager.Instance.QuitGame();
		this.OnHide();
	}
}

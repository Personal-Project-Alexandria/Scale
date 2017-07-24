using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeDialog : BaseDialog {

	public Text mode_1;
	public Text mode_2;
	public Text mode_3;
	public Text diamond;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		mode_1.text = UserProfile.Instance.GetHighScore(0).ToString();
		mode_2.text = UserProfile.Instance.GetHighScore(1).ToString();
		mode_3.text = UserProfile.Instance.GetHighScore(2).ToString();
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
	}

	public void OnClickModeNormal()
	{
		GUIManager.Instance.OnHideAllDialog();
		GameManager.Instance.mode = 0;
		GamePlayDialog play = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
	}

	public void OnClickMode3Slice()
	{
		GUIManager.Instance.OnHideAllDialog();
		GameManager.Instance.mode = 1;
		GamePlayDialog play = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
	}

	public void OnClickModeMultipleBalls()
	{
		GUIManager.Instance.OnHideAllDialog();
		GameManager.Instance.mode = 2;
		GamePlayDialog play = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
	}

	public void OnClickBack()
	{
		OnCloseDialog();
	}
}
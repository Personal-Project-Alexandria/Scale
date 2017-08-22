using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlayDialog : BaseDialog
{
    public Transform panelAnchor;
	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);

		GameManager.Instance.gamePlay = this;
		GameManager.Instance.StartGame();
	}
	public void OnClickPause()
	{
		PauseDialog dialog = GUIManager.Instance.OnShowDialog<PauseDialog>("Pause");
	}

	// ------------------------- TOPBAR GUI ------------------------ //
	public Image background;
	public Text life;
	public Text level;
	public Text diamond;
	public Slider slider;
	public Text rotateTip;
	public Image ballIcon;
	public List<Sprite> ballLife;

	public void ChangeBallIconByLife(int i)
	{
		if (ballLife != null && ballIcon != null && i < ballLife.Count && i >= 0)
		{
			ballIcon.sprite = ballLife[i];
		}
	}
	protected void Update()
	{
		life.text = GameManager.Instance.life.ToString();
		level.text = GameManager.Instance.level.ToString();
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
		slider.value = GameManager.Instance.percent;

		//if (Slicer.Instance.rotateTip.activeInHierarchy && Slicer.Instance.rotate)
		//{
		//	rotateTip.text = "TAP TO ROTATE";
		//}
		//else
		//{
		//	rotateTip.text = "";
		//}
	}
}


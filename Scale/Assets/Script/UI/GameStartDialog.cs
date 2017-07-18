using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartDialog : BaseDialog {

	public Text highScore;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		highScore.text = UserProfile.Instance.GetHighScore().ToString();
	}

	public void OnClickPlay()
    {
        GamePlayDialog dialog = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
        this.OnCloseDialog();
    }
    public void OnClickNoAds()
    { }

    public void OnClickShare()
    { }

    public void OnClickShop()
    {
        StoreDialog dialog = GUIManager.Instance.OnShowDialog<StoreDialog>("Store");
        this.OnCloseDialog();
    }

    public void OnClickLeaderBoard()
    { }

    public void OnClickSound()
    { }
}

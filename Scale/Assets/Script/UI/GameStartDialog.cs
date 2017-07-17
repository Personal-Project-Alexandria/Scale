using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartDialog : BaseDialog {

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
        GamePlayDialog dialog = GUIManager.Instance.OnShowDialog<GamePlayDialog>("Play");
        this.OnCloseDialog();
    }
    public void OnClickLeaderBoard()
    { }
    public void OnClickSound()
    { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayDialog : BaseDialog {

	public void OnClickPause()
    {
        PauseDialog dialog = GUIManager.Instance.OnShowDialog<PauseDialog>("Pause");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateDialog : BaseDialog {

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		gameName.text = "Do you like\n" + UserProfile.Instance.gameName;
	}

	public void OnClickRate()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.topfreepuzzle.tapshrink");
#elif UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/us/app/id1264453259");
#endif
    }
}

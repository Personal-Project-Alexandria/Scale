using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreDialog : BaseDialog {

	public Text diamond;

	protected void Update()
	{
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
	}

	public void OnClickIAP()
	{
		iAPDialog iap = GUIManager.Instance.OnShowDialog<iAPDialog>("iAP");
	}
}

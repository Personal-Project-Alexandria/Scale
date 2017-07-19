﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateDialog : BaseDialog {

	public void OnClickRate()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.quoclv.bestgame.scalecolor");
#elif UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/us/app/scale-color/id1261258088");
#endif
    }
}

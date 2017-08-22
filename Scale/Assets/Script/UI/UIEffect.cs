using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEffect : MonoSingleton<UIEffect> {

	public Text effectScore;

	public void MakeEffectScore(int score)
	{
		if (effectScore != null)
		{
			effectScore.text = score.ToString();
			effectScore.transform.localScale = Vector3.one * 0.75f;
			effectScore.transform.DOScale(1.5f, 0.75f);

			effectScore.color = new Color(1, 1, 1, 1);
			effectScore.DOColor(new Color(1, 1, 1, 0), 0.75f);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager> {

	public GameObject ballTouchLine;
	
	public void BallTouchLine(Vector3 position, float scale = 1)
	{
		GameObject effect = (GameObject)Instantiate(ballTouchLine, position, Quaternion.identity, transform);
		effect.transform.localScale = new Vector3(scale, scale, scale);
	}
}

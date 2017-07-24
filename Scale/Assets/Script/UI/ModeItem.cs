using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeItem : MonoBehaviour {

	public Text userName;
	public Text userScore;

	public void Setup(string name, string score)
	{
		userName.text = name;
		userScore.text = score;
	}
}

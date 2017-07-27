using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderDialog : BaseDialog {

	public GameObject grid;
	public GameObject itemPrefab;
	public Text modeName;

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		Setup();
	}

	public void Setup()
	{
		if (GameManager.Instance.mode == 0)
		{
			modeName.text = "classic";
		}
		else if (GameManager.Instance.mode == 1)
		{
			modeName.text = "3 slices";
		}
		else
		{
			modeName.text = "multiballs";
		}

		StartCoroutine(LoadScore());
		
	}

	IEnumerator LoadScore()
	{
		LeaderBoard.Instance.LoadScoreByMode(GameManager.Instance.mode);
		while (LeaderBoard.Instance.leaderBoards[GameManager.Instance.mode].load == true)
		{
			yield return null;
		}
		
		List<dreamloLeaderBoard.Score> scoreList = LeaderBoard.Instance.GetLeaderBoard(GameManager.Instance.mode);
		for (int i = 0; i < 10; i++)
		{
			if (i < scoreList.Count)
			{
				GameObject scoreBar = Instantiate(itemPrefab, grid.transform);
				scoreBar.transform.localScale = Vector3.one;
				scoreBar.GetComponent<ModeItem>().Setup(scoreList[i].playerName, scoreList[i].score.ToString());

				if (i == 0)
				{
					scoreBar.GetComponent<Image>().color = Palette.Translate(PColor.GOLD);
				}
				else if (i == 1)
				{
					scoreBar.GetComponent<Image>().color = Palette.Translate(PColor.SILVER);
				}
				else if (i == 2)
				{
					scoreBar.GetComponent<Image>().color = Palette.Translate(PColor.BRONZE);
				}
			}
		}
	}

	public void OnClickBack()
	{
		this.OnCloseDialog();
	}
}

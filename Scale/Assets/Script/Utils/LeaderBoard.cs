using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

[System.Serializable]
public class PairURL
{
	public string privateURL;
	public string publicURL;
	public PairURL(string pri, string pub)
	{
		privateURL = pri;
		publicURL = pub;
	}
} 

public class LeaderBoard : MonoSingleton<LeaderBoard> {

	public GameObject leaderBoardPrefab;
	public List<PairURL> pairs;
	public List<dreamloLeaderBoard> leaderBoards;

	public void Awake()
	{
		leaderBoards = new List<dreamloLeaderBoard>();

		for (int i = 0; i < pairs.Count; i++)
		{
			GameObject leaderBoard = (GameObject)Instantiate(leaderBoardPrefab, transform);
			leaderBoards.Add(leaderBoard.GetComponent<dreamloLeaderBoard>());
			leaderBoards[i].publicCode = pairs[i].publicURL;
			leaderBoards[i].privateCode = pairs[i].privateURL;
			leaderBoards[i].LoadScores();
		}
	}

	public void LoadScoreByMode(int mode)
	{
		leaderBoards[mode].LoadScores();
	} 

	public void UploadScore(int score, int mode)
	{
		if (!FB.IsLoggedIn || Application.internetReachability == NetworkReachability.NotReachable || mode >= leaderBoards.Count)
		{
			return;
		}

		StartCoroutine(AddScoreWithName(score, mode));
	}

	public void UploadHighscore(int mode)
	{
		UploadScore(UserProfile.Instance.GetHighScore(mode), mode);
	}

	public List<dreamloLeaderBoard.Score> GetLeaderBoard(int mode)
	{
		return leaderBoards[mode].ToListHighToLow();
	}

	IEnumerator AddScoreWithName(int score, int mode)
	{
		FBManager.Instance.SetupUserName();
		while (FBManager.Instance.GetUserName().Equals(""))
		{
			yield return null;
		}
		leaderBoards[mode].AddScore(FBManager.Instance.GetUserName(), score);
	}
}
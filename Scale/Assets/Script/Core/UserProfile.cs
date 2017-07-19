using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfile : MonoSingleton<UserProfile> {

	private string KEY_HIGH_SCORE = "KEY_HIGH_SCORE";
	private string KEY_DIAMOND = "KEY_DIAMOND";
	private string KEY_ADS = "KEY_ADS";

	private int highScore;
	private int diamond;
	private bool ads; // 0 = no ads, 1 = has ads

	private void Awake()
	{
		this.LoadProfile();
	}

	// High score function
	public bool IsHighScore(int newScore)
	{
		if (newScore > this.highScore)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void SetHighScore(int newScore)
	{
		if (IsHighScore(newScore))
		{
			this.highScore = newScore;
			PlayerPrefs.SetInt(KEY_HIGH_SCORE, this.highScore);
		}
	}
	public int GetHighScore()
	{
		return this.highScore;
	}

	// Diamond function
	public void AddDiamond(int addedDiamond)
	{
		this.diamond += addedDiamond;
		PlayerPrefs.SetInt(KEY_DIAMOND, this.diamond);
	}
	public bool ReduceDiamond(int reducedDiamond)
	{
		int temp = this.diamond - reducedDiamond;
		if (temp >= 0)
		{
			this.diamond -= reducedDiamond;
			PlayerPrefs.SetInt(KEY_DIAMOND, this.diamond);
			return true;
		}
		else
		{
			return false;
		}
	}
	public int GetDiamond()
	{
		return this.diamond;
	}
	[ContextMenu("Clear Diamond - test only")]
	public void ClearDiamond()
	{
		ReduceDiamond(GetDiamond());
	}

	// Ads function
	public void RemoveAds()
	{
		if (HasAds())
		{
			this.ads = false;
			PlayerPrefs.SetInt(KEY_ADS, HasAds() ? 1 : 0);
		}
	}
	public bool HasAds()
	{
		return this.ads;
	}

	// Save - load function
	public void LoadProfile()
	{
		// Init for first play
		this.highScore = 0;
		this.diamond = 0;
		this.ads = true;

		// Init for second, third, ... play
		if (PlayerPrefs.HasKey(KEY_HIGH_SCORE))
		{
			this.highScore = PlayerPrefs.GetInt(KEY_HIGH_SCORE);
		}
		if (PlayerPrefs.HasKey(KEY_DIAMOND))
		{
			this.diamond = PlayerPrefs.GetInt(KEY_DIAMOND);
		}
		if (PlayerPrefs.HasKey(KEY_ADS))
		{
			this.ads = PlayerPrefs.GetInt(KEY_ADS) == 1 ? true : false;
		}
	}
	public void SaveProfile()
	{
		PlayerPrefs.SetInt(KEY_HIGH_SCORE, this.highScore);
		PlayerPrefs.SetInt(KEY_DIAMOND, this.diamond);
		PlayerPrefs.SetInt(KEY_ADS, HasAds() ? 1 : 0);
	}
}

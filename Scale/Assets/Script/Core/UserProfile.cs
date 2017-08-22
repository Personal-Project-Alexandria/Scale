using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfile : MonoSingleton<UserProfile> {

	public string gameName;
	public List<Sprite> ballSprites;
	public Sprite noSound;
	public Sprite hasSound;

	private const int ITEM_COUNT = 15;

	private string KEY_HIGH_SCORE = "KEY_HIGH_SCORE";
	private string KEY_DIAMOND = "KEY_DIAMOND";
	private string KEY_ADS = "KEY_ADS";
	private string KEY_BALL = "KEY_BALL";

	private List<int> highScore;

	private int diamond;
	private bool ads; // 0 = no ads, 1 = has ads
	private List<bool> balls; // 0 = not, 1 = bought
	private int ballId;
	private Sprite ballSprite;

	private void Awake()
	{
		this.LoadProfile();
	}

	// High score function
	public bool IsHighScore(int newScore, int mode)
	{
		if (mode < highScore.Count && newScore > this.highScore[mode])
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void SetHighScore(int newScore, int mode)
	{
		if (IsHighScore(newScore, mode))
		{
			this.highScore[mode] = newScore;
			PlayerPrefs.SetInt(KEY_HIGH_SCORE + mode, this.highScore[mode]);
		}
	}
	public int GetHighScore(int mode)
	{
		if (mode < highScore.Count)
		{
			return this.highScore[mode];
		}
		else
		{
			return 0;
		}
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
	[ContextMenu("Clear Data - test only")] 
	public void ClearData()
	{
		PlayerPrefs.DeleteAll();
	}

	// Ads function
	public void RemoveAds()
	{
		if (HasAds())
		{
			this.ads = false;
			AdManager.Instance.HideBanner();
			PlayerPrefs.SetInt(KEY_ADS, HasAds() ? 1 : 0);
		}
	}
	public bool HasAds()
	{
		return this.ads;
	}

	// Ball function 
	public void BuyBall(int id)
	{
		balls[id] = true;
		PlayerPrefs.SetInt(KEY_BALL + id, 1);
	}
	public List<bool> GetBallList()
	{
		return this.balls;
	}
	public int GetActiveBall()
	{
		return this.ballId;
	}
	public void SetBallSprite(int id)
	{
		this.ballId = id;
		this.ballSprite = this.ballSprites[id];
		PlayerPrefs.SetInt(KEY_BALL + "ACTIVE", this.ballId);
	}
	public Sprite GetBallSprite()
	{
		return this.ballSprite;
	}

	// Save - load function
	public void LoadProfile()
	{
		// Init for first play
		this.highScore = new List<int>();
		for (int i = 0; i < GameManager.Instance.MODECOUNT; i++)
		{
			this.highScore.Add(0);
		}

		this.diamond = 0;
		this.ads = true;

		//Ball involves
		this.balls = new List<bool>();
		for (int i = 0; i < ITEM_COUNT; i++)
		{
			balls.Add(false);
		}
		this.balls[0] = true;
		this.ballId = 0;
		this.ballSprite = ballSprites[0];

		// Init for second, third, ... play
		for (int i = 0; i < GameManager.Instance.MODECOUNT; i++)
		{
			if (PlayerPrefs.HasKey(KEY_HIGH_SCORE + i))
			{
				this.highScore[i] = PlayerPrefs.GetInt(KEY_HIGH_SCORE + i);
			}
		}
		if (PlayerPrefs.HasKey(KEY_DIAMOND))
		{
			this.diamond = PlayerPrefs.GetInt(KEY_DIAMOND);
		}
		if (PlayerPrefs.HasKey(KEY_ADS))
		{
			this.ads = PlayerPrefs.GetInt(KEY_ADS) == 1 ? true : false;
		}
		for (int i = 0; i < balls.Count; i++)
		{
			if (PlayerPrefs.HasKey(KEY_BALL + i))
			{
				balls[i] = PlayerPrefs.GetInt(KEY_BALL + i) == 1 ? true : false;
			}
		} 
		if (PlayerPrefs.HasKey(KEY_BALL + "ACTIVE"))
		{
			this.ballId = PlayerPrefs.GetInt(KEY_BALL + "ACTIVE");
			this.ballSprite = ballSprites[this.ballId];
		}
	}
	public void SaveProfile()
	{
		for (int i = 0; i < GameManager.Instance.MODECOUNT; i++)
		{
			PlayerPrefs.SetInt(KEY_HIGH_SCORE + i, this.highScore[i]);
		}
		PlayerPrefs.SetInt(KEY_DIAMOND, this.diamond);
		PlayerPrefs.SetInt(KEY_ADS, HasAds() ? 1 : 0);
		for (int i = 0; i < balls.Count; i++)
		{
			PlayerPrefs.SetInt(KEY_BALL + i, balls[i] ? 1 : 0);
		}
		PlayerPrefs.SetInt(KEY_BALL + "ACTIVE", this.ballId);
	}
}

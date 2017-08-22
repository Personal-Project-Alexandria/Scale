using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoSingleton<GameManager> {

	public int MODECOUNT = 3;
	private const int DEFAULT_LIFE = 3;
	public bool inScale = false;

	public float goalPercent = 0.5f;
	public GameObject shapePrefab;
	[HideInInspector]
	public Slicer slicer;
	public Ball ball;
	[HideInInspector]
	public Shape shape;
	public int life;
	public int level;
	public float percent;
	[HideInInspector]
	public GamePlayDialog gamePlay;
	public BallManager ballManager;
	public SlicerManager slicerManager;
	public float area;
	public float destroyArea;
	public int mode;

    public void Update()
    {
        //if (gamePlay != null)
        //    if (this.gameAnchor.position.y != gamePlay.panelAnchor.position.y)
        //        this.gameAnchor.position = new Vector3(0, gamePlay.panelAnchor.position.y, 90);
    }

    public void StartGame()
	{
		AdManager.Instance.ShowBanner();

		life = DEFAULT_LIFE;
		level = 1;
		percent = 0f;
		if (shape != null)
		{
			Destroy(shape.gameObject);
		}
		shape = MakeShape(null, true);
		area = shape.Area();
		destroyArea = 0;
		
		if (mode == 0)
		{
			ballManager.Init(1);
			ballManager.OnStart();
			slicerManager.Init(1);
			slicerManager.OnStart();
		}
		else if (mode == 1)
		{
			ballManager.Init(1);
			ballManager.OnStart();
			slicerManager.Init(3);
			slicerManager.OnStart();
		}
		else if (mode == 2)
		{
			ballManager.Init(1);
			ballManager.OnStart();
			slicerManager.Init(1);
			slicerManager.OnStart();
		}

		gamePlay.background.color = Palette.Translate(PColor.PURPLE);
		shape.FillColor(Palette.Translate(PColor.PURPLE));
	}

	public void EndGame()
	{
		slicerManager.Clear();
		ballManager.Clear();
		Destroy(shape.gameObject);
		gamePlay.OnCloseDialog();
	}
	
	public void PauseGame()
	{
		ballManager.Pause();
		slicerManager.Pause();
	}

	public void ContinueGame()
	{
		AdManager.Instance.ShowBanner();
		slicerManager.Continue();
		ballManager.Continue();
	}

	public void RestartGame(bool onLose = false)
	{
		AdManager.Instance.ShowBanner();
		ballManager.Clear();
		slicerManager.Clear();
		StartGame();
	}

	public void NextLevel()
	{
		destroyArea = 0;
		area = shape.Area();
		percent = 0;
		level++;

		Color rand = Palette.RandomColorExcept(new List<PColor>() { PColor.WHITE, PColor.YELLOW,
			PColor.DARKYELLOW, PColor.GOLD, PColor.BRONZE });
		gamePlay.background.color = new Color(rand.r, rand.g, rand.b, 0.8f);
		shape.FillColor(rand);

		UIEffect.Instance.MakeEffectScore(level);

		if (mode == 1)
		{
			slicerManager.Restart();
		}
		else if (mode == 2)
		{
			// ADD MORE BALL HERE
			ballManager.AddBall();
		}
	}

	public void OnLose()
	{
		PauseGame();
		ExtraLifeDialog extraLifeDialog = GUIManager.Instance.OnShowDialog<ExtraLifeDialog>("ExtraLife");
	}

	public void GameOver(bool quit = false)
	{
		EndGame();

		UserProfile.Instance.SetHighScore(level, mode);

		UserProfile.Instance.AddDiamond(level * 10);

		if (this.level > 3)
		{
			AdManager.Instance.ShowVideo();
		}

		GameOverDialog gameOverDialog = GUIManager.Instance.OnShowDialog<GameOverDialog>("Over");
    }

	public void QuitGame()
	{
		EndGame();

		AdManager.Instance.ShowVideo();

		GameStartDialog start = GUIManager.Instance.OnShowDialog<GameStartDialog>("Start");
	}

	public void ContinueOnLose()
	{
		AdManager.Instance.ShowBanner();
		life = DEFAULT_LIFE;
		ballManager.Restart(true);
		slicerManager.Restart(true);
	}

	public Shape MakeShape(List<Vector3> points = null, bool scale = false)
	{
		GameObject shapeObject = Instantiate(shapePrefab, null) as GameObject;
		Shape shape = shapeObject.GetComponent<Shape>();

		if (points != null)
		{
			shape.points = points;
		}

		shape.InitLine(shape.points);

		if (scale == true)
		{
			shape.ScaleImmediate();
		}

		return shape;
	}

	public void CheckPercent()
	{
		percent = destroyArea / area;
		if (percent >= goalPercent)
		{
			shape.Scale();
		}
		else if (mode == 1)
		{
			StartCoroutine(CheckRemainSlicer());
		}
	}

	IEnumerator CheckRemainSlicer()
	{
		yield return new WaitForEndOfFrame();
		if (!slicerManager.RemainSlicer())
		{
			OnLose();
		}
	}

	// 3 balls test mode
	[ContextMenu("Start 3 balls")]
	public void StartMultipleBalls()
	{
		ballManager.Init(3);
		life = DEFAULT_LIFE;
		level = 1;
		percent = 0f;
		shape = MakeShape(null, true);
		ballManager.OnStart();
	}

	// 3 balls test mode
	[ContextMenu("Start 3 slicers")]
	public void StartMultipleSlicer()
	{
		slicerManager.Init(1);
		life = DEFAULT_LIFE;
		level = 1;
		percent = 0f;
		shape = MakeShape(null, true);
		area = shape.Area();
		ball.gameObject.SetActive(true);
		ball.OnStart();
		slicerManager.OnStart();
		destroyArea = 0;
	}
}

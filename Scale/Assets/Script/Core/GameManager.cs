using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoSingleton<GameManager> {

	private const int DEFAULT_LIFE = 3;
	public bool inScale = false; 

	public float goalPercent = 0.5f;
	public GameObject shapePrefab;
	public Slicer slicer;
	public Ball ball;
	public Shape shape;
	public int life;
	public int level;
	public float percent;
	public GamePlayDialog gamePlay;
    public Transform gameAnchor;
	public BallManager ballManager;
	public SlicerManager slicerManager;

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
		shape = MakeShape(null, true);
		slicer.gameObject.SetActive(true);
		ball.gameObject.SetActive(true);
		slicer.area = shape.Area();
		ball.OnStart();
		slicer.OnStart();
	}

	public void EndGame()
	{
		slicer.gameObject.SetActive(false);
		ball.gameObject.SetActive(false);
		Destroy(shape.gameObject);
		gamePlay.OnCloseDialog();
	}

	public void PauseGame()
	{
		ball.Pause();
		slicer.Pause();
	}

	public void ContinueGame()
	{
		AdManager.Instance.ShowBanner();
		slicer.Continue();
		ball.Continue();
	}

	public void RestartGame()
	{
		AdManager.Instance.ShowBanner();
		life = DEFAULT_LIFE;
		level = 1;
		percent = 0;
		Destroy(shape.gameObject);
		shape = MakeShape(null, true);
		slicer.area = shape.Area();
		ball.Restart();
		slicer.Restart();
	}

	public void NextLevel()
	{
		percent = 0;
		level++;
	}

	public void OnLose()
	{
		PauseGame();
		ExtraLifeDialog extraLifeDialog = GUIManager.Instance.OnShowDialog<ExtraLifeDialog>("ExtraLife");
	}

	public void GameOver(bool quit = false)
	{
		EndGame();

		UserProfile.Instance.SetHighScore(level);

		UserProfile.Instance.AddDiamond(level * 10);

		AdManager.Instance.ShowVideo();

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
		if (slicer.gameObject.activeInHierarchy && ball.gameObject.activeInHierarchy)
		{
			life = DEFAULT_LIFE;
			ball.Restart(true);
			slicer.Restart(true);
		}
	}

	public Shape MakeShape(List<Vector3> points = null, bool scale = false)
	{
		GameObject shapeObject = Instantiate(shapePrefab, null) as GameObject;
        //shapeObject.transform.position = new Vector3(0, this.gamePlay.panelAnchor.position.y, 0);
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
		slicerManager.Init(3);
		life = DEFAULT_LIFE;
		level = 1;
		percent = 0f;
		shape = MakeShape(null, true);
		ball.gameObject.SetActive(true);
		ball.OnStart();
		slicerManager.OnStart();
	}
}

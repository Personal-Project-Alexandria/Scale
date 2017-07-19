using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoSingleton<GameManager> {

	private const int DEFAULT_LIFE = 3;

	public float goalPercent = 0.5f;
	public GameObject shapePrefab;
	public Slicer slicer;
	public Ball ball;
	public Shape shape;
	public int life;
	public int level;
	public float percent;
	public GamePlayDialog gamePlay;

	public void StartGame()
	{
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
		slicer.Continue();
		ball.Continue();
	}

	public void RestartGame()
	{
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

	public void GameOver()
	{
		EndGame();

		UserProfile.Instance.SetHighScore(level);

		UserProfile.Instance.AddDiamond(level * 10);

		AdManager.Instance.ShowVideo();

		GameOverDialog gameOverDialog = GUIManager.Instance.OnShowDialog<GameOverDialog>("Over");
	}

	public void ContinueOnLose()
	{
		if (slicer.gameObject.activeInHierarchy && ball.gameObject.activeInHierarchy)
		{
			life = DEFAULT_LIFE;
			ball.Restart();
			slicer.Restart();
		}
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
}

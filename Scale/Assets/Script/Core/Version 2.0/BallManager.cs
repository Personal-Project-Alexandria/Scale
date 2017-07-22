﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
	public GameObject ballPrefabs;
	private List<BaseBall> balls;

	public void Init(int count)
	{
		if (balls == null)
		{
			balls = new List<BaseBall>(); 
		}
		else
		{
			balls.Clear();
		}

		for (int i = 0; i < count; i++)
		{
			GameObject ballObject = (GameObject)Instantiate(ballPrefabs, null);
			BaseBall ball = ballObject.GetComponent<BaseBall>();
			ball.ballManager = this;
			balls.Add(ball);
		}
 	}

	public void AddForce()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].AddForce(i % 4);
		}
	}

	public void StopForce()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].StopForce();
		}
	}

	public void OnStart()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].OnStart();
		}
	}

	public void Pause()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].Pause();
		}
	}

	public void Continue()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].Continue();
		}
	}

	public void Restart(bool onLose = false)
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].Restart();
		}
	}

	public void OnHit()
	{
		GameManager.Instance.life--;

		ScaleBall();

		if (GameManager.Instance.life <= 0)
		{
			OnLose();
		}
	}

	public void OnLose()
	{
		StopAllCoroutines();
		GameManager.Instance.OnLose();
	}

	public void ScaleBall()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].ScaleBall();
		}
	}

	public void Clear()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			Destroy(balls[i].gameObject);
		}
		balls.Clear();
	}

	public List<BaseBall> GetBallList()
	{
		return this.balls;
	}
}

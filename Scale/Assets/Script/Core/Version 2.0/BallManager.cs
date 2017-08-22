using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
	public GameObject ballPrefabs;
	private List<BaseBall> balls;
	private float recoverTime = 0.1f;

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
			GameObject ballObject = (GameObject)Instantiate(ballPrefabs, transform);
			BaseBall ball = ballObject.GetComponent<BaseBall>();
			ball.ballManager = this;
			balls.Add(ball);
		}
 	}

	public void AddForce()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].AddForce(i % 4);
			}
		}
	}

	public void StopForce()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].StopForce();
			}
		}
	}

	public void OnStart()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].OnStart();
			}
		}
	}

	public void Pause()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].Pause();
			}
		}
	}

	public void Continue()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].Continue();
			}
		}
	}

	public void Restart(bool onLose = false)
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].Restart(onLose);
			}
		}
	}

	public void OnHit()
	{
		if (recoverTime <= 0)
		{
			recoverTime = 0.1f;

			GameManager.Instance.life--;
			ScaleBall();
			if (GameManager.Instance.life == 0)
			{
				OnLose();
			}
		}
	}

	public void Update()
	{
		recoverTime -= Time.deltaTime;
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
			if (balls[i] != null)
			{
				balls[i].ScaleBall();
			}
		}
	}

	public void Clear()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				Destroy(balls[i].gameObject);
			}
		}
		balls.Clear();
	}

	public void AddBall()
	{
		GameObject ballObject = (GameObject)Instantiate(ballPrefabs, GetMainBall().transform.position, Quaternion.identity, transform);
		BaseBall ball = ballObject.GetComponent<BaseBall>();
		ball.ballManager = this;
		balls.Add(ball);
		ball.OnStart();
	}

	public BaseBall GetMainBall()
	{
		if (balls != null && balls.Count >= 1)
		{
			return balls[0];
		}
		return null;
	}

	public List<BaseBall> GetMinorBalls()
	{
		List<BaseBall> minor = new List<BaseBall>();
		for (int i = 1; i < balls.Count; i++)
		{
			minor.Add(balls[i]);
		}
		return minor;
	}

	public List<BaseBall> GetBallList()
	{
		return this.balls;
	}
}

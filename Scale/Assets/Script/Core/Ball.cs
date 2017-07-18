using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoSingleton<Ball> {

	protected Rigidbody2D body;
	protected Vector2 tempVelocity = Vector2.zero;
	protected bool hit = false; 

	protected void Start ()
	{
		ScaleBall();
		body = GetComponent<Rigidbody2D>();
		AddForce();
	}

	public void AddForce()
	{
		if (body != null)
		{
			body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 200);
		}
	}

	public void StopForce()
	{
		if (body != null)
		{
			body.velocity = Vector2.zero;
		}
	}

	public void Pause()
	{
		tempVelocity = body.velocity;
		body.velocity = Vector2.zero;
	}

	public void Continue()
	{
		body.velocity = tempVelocity;
		tempVelocity = Vector2.zero;
	}

	public void Restart()
	{
		ScaleBall();
		transform.position = Vector3.zero;
		tempVelocity = Vector2.zero;
		StopForce();
		AddForce();
	}

	public void OnHit()
	{
		StartCoroutine(Recover(0.5f));

		GameManager.Instance.life--;

		if (GameManager.Instance.life <= 0)
		{
			OnLose();
		}
		else
		{
			ScaleBall();
		}
	}

	public void OnLose()
	{
		StopAllCoroutines();
		GameManager.Instance.OnLose();
	}

	public void ScaleBall()
	{
		int life = GameManager.Instance.life;

		if (life == 3)
		{
			transform.localScale = new Vector3(0.8f, 0.8f);
		}
		else if (life == 2)
		{
			transform.localScale = new Vector3(0.65f, 0.65f);
		}
		else if (life == 1)
		{
			transform.localScale = new Vector3(0.5f, 0.5f);
		}
	}

	IEnumerator Recover(float time)
	{
		hit = true;
		yield return new WaitForSeconds(time);
		hit = false;
	}
}

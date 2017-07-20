using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoSingleton<Ball> {

	public SpriteRenderer sprite;
	protected Rigidbody2D body;
	protected Vector2 tempVelocity = Vector2.zero;
	protected bool hit = false; 

	protected void Awake ()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

	public void AddForce()
	{
		body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 200);
	}

	public void StopForce()
	{
		body.velocity = Vector2.zero;
	}

	public void OnStart()
	{
		ScaleBall();
		body = GetComponent<Rigidbody2D>();
		AddForce();
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

	public void Restart(bool onLose = false)
	{
		ScaleBall();
		if (!onLose)
		{
			transform.position = Vector3.zero;
			tempVelocity = Vector2.zero;
			StopForce();
			AddForce();
		}
		else
		{
			Continue();
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
		else
		{
			StartCoroutine(Recover(0.5f));
		}
	}

	public void OnLose()
	{
		StopAllCoroutines();
		GameManager.Instance.OnLose();
	}

	public void ScaleBall()
	{
		this.sprite.sprite = UserProfile.Instance.GetBallSprite();
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBall : MonoBehaviour {

	[HideInInspector]
	public BallManager ballManager = null;
	public SpriteRenderer sprite;
	protected Rigidbody2D body;
	protected Vector2 tempVelocity = Vector2.zero;

	protected void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

	public void AddForce()
	{
		body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 200);
	}

	public void AddForce(int part)
	{
		switch (part)
		{
		case 0: body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 200); break;
		case 1: body.AddForce(new Vector3(Random.Range(-1, -0.5f), Random.Range(0.5f, 1f)).normalized * 200); break;
		case 2: body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(-1, -0.5f)).normalized * 200); break;
		default: body.AddForce(new Vector3(Random.Range(-1, -0.5f), Random.Range(- 1, -0.5f)).normalized * 200); break;
		}
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
}

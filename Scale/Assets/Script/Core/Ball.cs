using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoSingleton<Ball> {

	protected Rigidbody2D body;

	protected void Start ()
	{
		body = GetComponent<Rigidbody2D>();
		AddForce();
	}

	public void AddForce()
	{
		body.AddForce(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 200);
	}

	public void StopForce()
	{
		body.velocity = Vector2.zero;
	}
}

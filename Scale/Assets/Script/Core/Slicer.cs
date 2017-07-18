﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SliceType { STRAIGHT, CORNER }

public class Slicer : MonoSingleton<Slicer> {

	public GameObject linePrefab;
	public SliceType type = SliceType.STRAIGHT;
	public SpriteRenderer sprite;
	public Sprite corner;
	public Sprite straigth;

	[HideInInspector]
	public float area;

	private SlicerLine first;
	private SlicerLine second;
	private float destroyArea;
	private bool paused = false;
	protected Shape shape_one;
	protected Shape shape_two;

	protected void Start()
	{
		//destroyArea = 0;
		//transform.position = start;
		//Reload();
	}

	public void Create()
	{
		this.type = (SliceType)Random.Range(0, 2);
		this.sprite.transform.rotation = Quaternion.identity;

		GameObject firstLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
		GameObject secondLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);

		first = firstLine.GetComponent<SlicerLine>();
		second = secondLine.GetComponent<SlicerLine>();

		if (this.type == SliceType.CORNER)
		{
			this.sprite.sprite = corner;
			first.Create(LineDirection.UP, this);
			second.Create(LineDirection.RIGHT, this);
		}
		else
		{
			this.sprite.sprite = straigth;
			first.Create(LineDirection.LEFT, this);
			second.Create(LineDirection.RIGHT, this);
		}
	}

	public void Reload()
	{
		this.Create();
		first.gameObject.SetActive(false);
		second.gameObject.SetActive(false);
	}

	public void Rotate()
	{
		first.Rotate();
		second.Rotate();

		sprite.transform.Rotate(new Vector3(0, 0, -90));
	}

	public void Grow(float v)
	{
		if (first != null && second != null)
		{
			first.gameObject.SetActive(true);
			second.gameObject.SetActive(true);

			first.Grow(v);
			second.Grow(v);
		}
	}

	public void Slice()
	{
		if (first.wait && second.wait)
		{
			first.MakeInfo();
			second.MakeInfo();

			List<Vector3> points = first.info.line.GetShape().points;
			List<Vector3> list_one = new List<Vector3>();
			List<Vector3> list_two = new List<Vector3>();

			list_one.Add(first.info.addedPoint);
			list_two.Add(second.info.addedPoint);
			
			if (this.type == SliceType.CORNER)
			{
				list_one.Add(first.start);
				list_two.Add(second.start);
			}

			list_one.Add(second.info.addedPoint);
			list_two.Add(first.info.addedPoint);

			AddPointsToList(points, list_one, second.info.line.index + 1, first.info.line.index);
			AddPointsToList(points, list_two, first.info.line.index + 1, second.info.line.index);

			shape_one = GameManager.Instance.MakeShape(list_one);
			shape_two = GameManager.Instance.MakeShape(list_two);

			shape_one.gameObject.name = "shape_one";
			shape_two.gameObject.name = "shape_two";

			if (shape_one.BallInShape(Ball.Instance.transform.position))
			{
				Clear(1);
			}
			else
			{
				Clear(0);
			}
		}
	}	

	void Clear(int index)
	{
		Destroy(first.info.line.GetShape().gameObject);
		Destroy(first.gameObject);
		Destroy(second.gameObject);

		if (index == 0)
		{
			destroyArea += shape_one.Area();
			Destroy(shape_one.gameObject);
			GameManager.Instance.shape = shape_two;
		}
		else
		{
			destroyArea += shape_two.Area();
			Destroy(shape_two.gameObject);
			GameManager.Instance.shape = shape_one;
		}

		this.grow = false;
		this.transform.position = start;
		this.Reload();

		GameManager.Instance.percent = destroyArea / area;

		if (GameManager.Instance.percent >= GameManager.Instance.goalPercent)
		{
			GameManager.Instance.shape.Scale();
			destroyArea = 0;
		}
	}

	public void ClearLine()
	{
		if (first != null)
		{
			Destroy(first.gameObject);
		}
		if (second != null)
		{
			Destroy(second.gameObject);
		}
		this.grow = false;
		this.transform.position = start;
		this.Reload();
	}

	public void AddPointsToList(List<Vector3> src, List<Vector3> des, int start, int end)
	{
		if (end < start)
		{
			end += src.Count;
		}

		for (int i = start; i <= end; i++)
		{
			des.Add(src[i % src.Count]);
		}
	}

	// ----------------------------------------- DRAG AND DROP ------------------------------------------//
	private Vector3 offset;
	public Vector3 start;
	protected const float DOWN_TIME = 0.2f;
	protected float downTime = 0f;
	[HideInInspector]
	public bool down = false;
	[HideInInspector]
	public bool grow = false;

	protected void Update()
	{
		// If game is pause then return
		if (paused)
		{
			return;
		}

		// Else process...
		if (this.down)
		{
			this.downTime += Time.deltaTime;
		}

		if (this.grow)
		{
			Grow(Time.deltaTime * 1.5f);
		} 
	}

	protected void OnMouseDown()
	{
		if (paused)
		{
			return;
		}

		this.down = true;

		offset = transform.position - Camera.main.ScreenToWorldPoint(
			new Vector3(Input.mousePosition.x, Input.mousePosition.y));
	}

	protected void OnMouseDrag()
	{
		if (paused)
		{
			return;
		}

		if (!this.grow)
		{
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
			this.transform.position = curPosition;
		}
	}

	protected virtual void OnMouseUp()
	{
		if (paused)
		{
			return;
		}

		if (down && !this.grow)
		{
			BoxCollider2D box = GetComponent<BoxCollider2D>();
			Shape shape = FindObjectOfType<Shape>(); // Cheat here

			float width = linePrefab.GetComponent<LineRenderer>().startWidth * 1.5f;
			Debug.Log(width);

			Vector3 tl, tr, bl, br;
			tl = transform.position + new Vector3(-width, width);
			tr = transform.position + new Vector3(width, width);
			bl = transform.position + new Vector3(-width, -width);
			br = transform.position + new Vector3(width, -width);

			if (shape.PointInShape(tl) && shape.PointInShape(tr) && shape.PointInShape(bl) && shape.PointInShape(br))
			{
				this.grow = true;
			}
			else
			{
				this.Rotate();
				this.transform.position = start;
			}

			this.down = false;
			this.downTime = 0f;
		}
	}

	// ------------------------------------- GAME STATE AFFECT THIS ---------------------------------------//

	public void OnStart()
	{
		this.Restart();
	}

	public void Pause()
	{
		this.paused = true;
	}

	public void Continue()
	{
		this.paused = false;
	}

	public void Restart()
	{
		destroyArea = 0;
		transform.position = start;
		ClearLine();
		paused = false;
	}
}

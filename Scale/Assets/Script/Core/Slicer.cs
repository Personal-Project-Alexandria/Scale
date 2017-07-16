using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SliceType { STRAIGHT, CORNER }

public class Slicer : MonoBehaviour {

	public GameObject linePrefab;
	public SliceType type = SliceType.STRAIGHT;
	public SpriteRenderer sprite;

	private SlicerLine first;
	private SlicerLine second;

	protected void Start()
	{
		this.Init();
	}

	protected void Update()
	{
		if (test)
		{
			Grow(Time.deltaTime * 2);
		}
	}

	public void Init()
	{
		GameObject firstLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
		GameObject secondLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);

		first = firstLine.GetComponent<SlicerLine>();
		second = secondLine.GetComponent<SlicerLine>();

		if (this.type == SliceType.CORNER)
		{
			first.Create(LineDirection.UP, this);
			second.Create(LineDirection.RIGHT, this);
		}
		else
		{
			first.Create(LineDirection.UP, this);
			second.Create(LineDirection.DOWN, this);
		}
	}

	[ContextMenu("Rotate")]
	public void Rotate()
	{
		first.Rotate();
		second.Rotate();

		sprite.transform.Rotate(new Vector3(0, 0, -90));
	}

	public void Grow(float v)
	{
		first.Grow(v);
		second.Grow(v);
	}

	bool test = false;
	[ContextMenu("Trigger")]
	public void TriggerTest()
	{
		test = !test;
	}

	public void Slice()
	{
		if (first.wait && second.wait && first.info.line.GetShape())
		{
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
				StartCoroutine(Clear(1));
			}
			else
			{
				StartCoroutine(Clear(0));
			}
			
		}
	}

	protected Shape shape_one, shape_two;

	IEnumerator Clear(int index)
	{
		yield return new WaitForEndOfFrame();

		Destroy(first.info.line.GetShape().gameObject);
		Destroy(gameObject);

		if (index == 0)
		{
			Destroy(shape_one.gameObject);
			shape_two.Scale();
		}
		else
		{
			Destroy(shape_two.gameObject);
			shape_one.Scale();
		}
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {

	public GameObject shapePrefab;

	public void Start()
	{
		Shape startShape = MakeShape(null, true);
		Slicer.Instance.area = startShape.Area();
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

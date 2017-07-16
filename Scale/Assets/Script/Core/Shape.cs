using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Shape include many points, can be scaled through center
public class Shape : MonoBehaviour {

	public GameObject linePrefab;
	public List<Vector3> points;    // List of points in shape
	//public float scale = 1;			// Scale of shape
	private List<Line> lines = new List<Line>();

	// Get center of shape
	private Vector3 Center
	{
		get
		{
			Vector3 sum = Vector3.zero;
			if (points != null && points.Count > 0)
			{
				int left = 0;
				int right = 0;
				int up = 0;
				int down = 0;

				for (int i = 0; i < points.Count; i++)
				{
					if (points[left].x > points[i].x)
					{
						left = i;
					}
					if (points[right].x < points[i].x)
					{
						right = i;
					}
					if (points[down].y > points[i].y)
					{
						down = i;
					}
					if (points[up].y < points[i].y)
					{
						up = i;
					}
				}

				return new Vector3((points[left].x + points[right].x) / 2, (points[up].y + points[down].y) / 2);
			}
			else
			{
				return Vector3.zero;
			}
		}
	}
	
	public void Scale()
	{
		StartCoroutine(ScaleInAction(this.Center, CalculateScale()));
	}

	public IEnumerator ScaleInAction(Vector3 center, float scale)
	{
		Ball.Instance.StopForce();

		float curScale = 1;
		float curPoint = 0;
		Vector3 ori = Ball.Instance.transform.position;

		while (curScale < scale)
		{
			points.Add(points[0]);

			for (int i = 0; i < lines.Count; i++)
			{
				lines[i].UpdateLine((points[i] - center * curPoint) * curScale, (points[i + 1] - center * curPoint) * curScale, false);
			}

			points.RemoveAt(points.Count - 1);

			Ball.Instance.transform.position = (ori - center * curPoint) * curScale;

			yield return new WaitForFixedUpdate();

			if (curScale < scale)
			{
				curScale += Time.fixedDeltaTime * 0.5f;
			}

			if (curPoint < 1)
			{
				
			}
			curPoint = (curScale - 1) / (scale - 1);
		}

		// After effect

		curScale = scale;
		curPoint = 1;

		points.Add(points[0]);

		for (int i = 0; i < points.Count; i++)
		{
			points[i] = (points[i] - center) * curScale;
		}

		for (int i = 0; i < lines.Count; i++)
		{
			lines[i].UpdateLine(points[i], points[i + 1]);
		}

		points.RemoveAt(points.Count - 1);

		Ball.Instance.transform.position = (ori - center * curPoint) * curScale;

		Ball.Instance.AddForce();
	}

	public void ScaleImmediate()
	{
		// Get center of shape
		Vector3 center = this.Center;

		float scale = CalculateScale();

		points.Add(points[0]);

		for (int i = 0; i < points.Count; i++)
		{
			points[i] = (points[i] - center) * scale;
		}

		for (int i = 0; i < lines.Count; i++)
		{
			lines[i].UpdateLine(points[i], points[i + 1]);
		}

		points.RemoveAt(points.Count - 1);
	}

	public float CalculateScale()
	{
		Vector3 center = this.Center;
		float max = 0;
		int index = -1;

		for (int i = 0; i < points.Count; i++)
		{
			float distance = Vector3.Distance(center, points[i]);
			if (distance > max)
			{
				max = distance;
				index = i;
			}
		}

		float scaleX = 2f / Mathf.Abs(points[index].x - center.x);
		float scaleY = 2f / Mathf.Abs(points[index].y - center.y);

		return scaleX < scaleY ? scaleX : scaleY;
	}

	public bool BallInShape(Vector3 position)
	{
		return PointInPolygon(position, points);
	}

	public void InitLine(List<Vector3> points)
	{
		points.Add(points[0]);
		for (int i = 0; i < points.Count - 1; i++)
		{
			GameObject lineObject = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
			lineObject.GetComponent<Line>().Create(points[i], points[i + 1], i);
			lineObject.GetComponent<Line>().BelongTo(this);
			lines.Add(lineObject.GetComponent<Line>());
		}
		points.RemoveAt(points.Count - 1);
	}

	static bool PointInPolygon(Vector3 p, List<Vector3> poly)
	{
		Vector3 p1, p2;

		bool inside = false;

		if (poly.Count < 3)
		{
			return inside;
		}

		Vector3 oldPoint = new Vector3(
		poly[poly.Count - 1].x, poly[poly.Count - 1].y);

		for (int i = 0; i < poly.Count; i++)
		{
			Vector3 newPoint = new Vector3(poly[i].x, poly[i].y);

			if (newPoint.x > oldPoint.x)
			{
				p1 = oldPoint;
				p2 = newPoint;
			}
			else
			{
				p1 = newPoint;
				p2 = oldPoint;
			}

			if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
			&& ((p.y - p1.y) * (p2.x - p1.x)
			 < (p2.y - p1.y) * (p.x - p1.x)))
			{
				inside = !inside;
			}

			oldPoint = newPoint;
		}

		return inside;
	}
}

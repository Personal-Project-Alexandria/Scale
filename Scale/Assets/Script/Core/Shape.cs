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
		GameManager.Instance.inScale = true;
		GameManager.Instance.ballManager.StopForce();

		float curScale = 1;
		float curPoint = 0;

		List<Vector3> oris = new List<Vector3>();
		List<BaseBall> balls = new List<BaseBall>();

		balls = GameManager.Instance.ballManager.GetBallList();
		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				oris.Add(balls[i].transform.position);
			}
			else
			{
				oris.Add(Vector3.zero);
			}
		}

		while (curScale < scale || curPoint < 1f)
		{
			List<Vector3> points3 = new List<Vector3>();

			for (int i = 0; i < points.Count; i++)
			{
				points3.Add((points[i] - center * curPoint) * curScale);
			}

			points.Add(points[0]);

			for (int i = 0; i < lines.Count; i++)
			{
				lines[i].UpdateLine((points[i] - center * curPoint) * curScale, (points[i + 1] - center * curPoint) * curScale, false);
			}

			points.RemoveAt(points.Count - 1);

		
			for (int i = 0; i < balls.Count; i++)
			{
				if (balls[i] != null)
				{
					balls[i].transform.position = (oris[i] - center * curPoint) * curScale;
				}
			}

			RenderMesh(points3);

			yield return new WaitForFixedUpdate();

			if (curScale < scale)
			{
				curScale += Time.deltaTime * 1.5f;
			}

			if (curPoint < 1)
			{
				curPoint += Time.deltaTime * 1.5f;
			}
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

		this.RenderMesh(points);

		for (int i = 0; i < balls.Count; i++)
		{
			if (balls[i] != null)
			{
				balls[i].transform.position = (oris[i] - center * curPoint) * curScale;
				balls[i].AddForce(Random.Range(0, 4));
			}
		}

		GameManager.Instance.area = this.Area();
		GameManager.Instance.NextLevel();
		GameManager.Instance.inScale = false;
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
		RenderMesh(points);
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

		float scaleX = 2.5f / Mathf.Abs(points[index].x - center.x);
		float scaleY = 2.5f / Mathf.Abs(points[index].y - center.y);

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
		RenderMesh(points);
	}

	public static bool PointInPolygon(Vector3 p, List<Vector3> poly)
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

	// ------------------------------ CHECK POLYGON --------------------------------- //

	public bool PointInShape(Vector3 p)
	{
		Vector3 p1, p2;

		bool inside = false;

		if (points.Count < 3)
		{
			return inside;
		}

		Vector3 oldPoint = new Vector3(
		points[points.Count - 1].x, points[points.Count - 1].y);

		for (int i = 0; i < points.Count; i++)
		{
			Vector3 newPoint = new Vector3(points[i].x, points[i].y);

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

	public bool IsPointInPolygon(Vector3 point/*, List<Vector3> polygon*/)
	{
		int polygonLength = points.Count, i = 0;
		bool inside = false;
		// x, y for tested point.
		float pointX = point.x, pointY = point.y;
		// start / end point for the current polygon segment.
		float startX, startY, endX, endY;
		Vector3 endPoint = points[polygonLength - 1];
		endX = endPoint.x;
		endY = endPoint.y;
		while (i < polygonLength)
		{
			startX = endX; startY = endY;
			endPoint = points[i++];
			endX = endPoint.x; endY = endPoint.y;
			//
			inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
					  && /* if so, test if it is under the segment */
					  ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
		}
		return inside;
	}

	// Return True if the point is in the polygon.
	public bool PointInPolygon(Vector3 p)
	{
		// Get the angle between the point and the
		// first and last vertices.
		int max_point = points.Count - 1;
		float total_angle = GetAngle(
			points[max_point].x, points[max_point].y,
			p.x, p.y,
			points[0].x, points[0].y);

		// Add the angles from the point
		// to each other pair of vertices.
		for (int i = 0; i < max_point; i++)
		{
			total_angle += GetAngle(
				points[i].x, points[i].y,
				p.x, p.y,
				points[i + 1].x, points[i + 1].y);
		}

		// The total angle should be 2 * PI or -2 * PI if
		// the point is in the polygon and close to zero
		// if the point is outside the polygon.
		return (System.Math.Abs(total_angle) > 0.000001);
	}

	public static float GetAngle(float Ax, float Ay,
	float Bx, float By, float Cx, float Cy)
	{
		// Get the dot product.
		float dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy);

		// Get the cross product.
		float cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);

		// Calculate the angle.
		return (float)System.Math.Atan2(cross_product, dot_product);
	}

	private static float DotProduct(float Ax, float Ay,
	float Bx, float By, float Cx, float Cy)
	{
		// Get the vectors' coordinates.
		float BAx = Ax - Bx;
		float BAy = Ay - By;
		float BCx = Cx - Bx;
		float BCy = Cy - By;

		// Calculate the dot product.
		return (BAx * BCx + BAy * BCy);
	}

	public static float CrossProductLength(float Ax, float Ay,
	float Bx, float By, float Cx, float Cy)
	{
		// Get the vectors' coordinates.
		float BAx = Ax - Bx;
		float BAy = Ay - By;
		float BCx = Cx - Bx;
		float BCy = Cy - By;

		// Calculate the Z coordinate of the cross product.
		return (BAx * BCy - BAy * BCx);
	}

	// --------------------------------------------------------------------------------//
	// 0 1 2 3 4 Count = 5 (4 + 1) = 5 p[4] != p[0]
	public void RenderMesh(List<Vector3> points3)
	{
		List<Vector2> points2 = new List<Vector2>();
		for (int i = 0; i < points3.Count; i++)
		{
			Vector2 point = new Vector2(points3[i].x, points3[i].y);
			if (!point.Equals(points3[(i + 1) % points3.Count])) 
			{
				points2.Add(point);
			}
		}
		Vector2[] points = points2.ToArray();

		int pointCount = points.Length;
		MeshFilter mf = GetComponent<MeshFilter>();
		MeshRenderer mr = GetComponent<MeshRenderer>();
		PolygonCollider2D pc = GetComponent<PolygonCollider2D>();
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[pointCount];
		Vector2[] uv = new Vector2[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			Vector3 actual = points[i];
			vertices[i] = new Vector3(actual.x, actual.y);
			uv[i] = actual;
		}
		Triangulator tr = new Triangulator(points);
		int[] triangles = tr.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mf.mesh = mesh;
		mr.material.color = Shape.fill;
		pc.SetPath(0, points);
	}

	private static Color fill = Palette.Translate(PColor.PURPLE);

	public void FillColor(Color color)
	{
		Shape.fill = color;
		GetComponent<MeshRenderer>().material.color = Shape.fill;
	}

	public float Area()
	{
		int i, j;
		float area = 0;

		for (i = 0; i < points.Count; i++)
		{
			j = (i + 1) % points.Count;

			area += points[i].x * points[j].y;
			area -= points[i].y * points[j].x;
		}

		area /= 2;
		return (area < 0 ? -area : area);
	}

	public Line FindLineByStart(Vector3 start)
	{
		foreach (Line line in lines)
		{
			if (start == line.start)
			{
				//GameObject dup = Instantiate(line.gameObject);
				return line;
			}
		}
		
		return null;
	}

	public Line FindLineByEnd(Vector3 end)
	{
		foreach (Line line in lines)
		{
			if (end == line.end)
			{
				return line;
			}
		}
		return null;
	}

	public void Clear()
	{
		Destroy(gameObject);
		//StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		for (int i = 0; i < lines.Count; i++)
		{
			Destroy(lines[i].gameObject);
		}
		Color color = mr.material.color;
		float time = 0f;
		while (time < 1 && !GameManager.Instance.inScale)
		{
			yield return null;
			time += Time.deltaTime;
			mr.material.color = Color.Lerp(mr.material.color, new Color(0, 0, 0, 0), time);
		}
		Destroy(gameObject);
	}
}

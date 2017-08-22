using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clock-wise direction of line
public enum LineDirection
{
	UP, RIGHT, DOWN, LEFT, UNKNOWNED
}

// Line contains two point only
public class Line : MonoBehaviour {

	[HideInInspector]
	public LineRenderer lineRender;         // Line renderer of this Line
	protected BoxCollider2D boxCollider;    // Collider of this line, detect collision
	public LineDirection direction;			// Direction of line, detect scale and slice
	private Shape shape;                    // Parent shape [Line Only]
	[HideInInspector]
	public int index;                       // Index of line in shape's point
	public CircleCollider2D corner;			// Start corner

	protected virtual void Init()
	{
		gameObject.tag = "Line";

		lineRender = GetComponent<LineRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();

		// Initialize lineRenderer
		if (lineRender == null)
		{
			gameObject.AddComponent<LineRenderer>();	
		}
		lineRender.positionCount = 2;
		lineRender.SetPosition(0, Vector3.zero);
		lineRender.SetPosition(1, Vector3.zero);
		lineRender.useWorldSpace = false;

		// Initialize boxCollider
		if (boxCollider == null)
		{
			gameObject.AddComponent<BoxCollider2D>();
		}
		boxCollider.size = new Vector2(lineRender.startWidth, lineRender.endWidth);
		boxCollider.offset = Vector2.zero;

		direction = LineDirection.UNKNOWNED;
	}

	// Create line from two point
	public void Create(Vector3 start, Vector3 end, int index, bool autoScale = true)
	{
		this.Init();

		lineRender.SetPosition(0, start);
		lineRender.SetPosition(1, end);

		this.index = index;

		DetectDirection();

		//corner.radius = lineRender.startWidth;
		corner.transform.localPosition = start;
		corner.GetComponent<SpriteRenderer>().color = lineRender.startColor;

		if (autoScale)
		{
			ScaleCollider();
		}
	}

	public void UpdateLine(Vector3 start, Vector3 end, bool checkCollider = true)
	{
		this.start = start;
		this.end = end;

		if (checkCollider)
		{
			ScaleCollider();
		}
	}

	// Scale box collider of line
	protected void ScaleCollider()
	{
		Vector3 diff = end - start;

		if (diff.x < 0 || diff.y < 0)
		{
			diff = -diff;
		}

		if (direction == LineDirection.UP)
		{
			boxCollider.size = new Vector3(lineRender.startWidth, diff.y);
			boxCollider.offset = new Vector3(start.x, start.y + diff.y / 2);
		}
		else if (direction == LineDirection.DOWN)
		{
			boxCollider.size = new Vector3(lineRender.startWidth, diff.y);
			boxCollider.offset = new Vector3(start.x, start.y - diff.y / 2);
		}
		else if (direction == LineDirection.LEFT)
		{
			boxCollider.size = new Vector3(diff.x, lineRender.startWidth);
			boxCollider.offset = new Vector3(start.x - diff.x / 2, start.y);
		}
		else if (direction == LineDirection.RIGHT)
		{
			boxCollider.size = new Vector3(diff.x, lineRender.startWidth);
			boxCollider.offset = new Vector3(start.x + diff.x / 2, start.y);
		}
		else
		{
			Debug.Log("ERROR. UNKNOWNED DIRECTION");
			boxCollider.size = diff;
			boxCollider.offset = Vector3.zero;
		}
	}

	// Belong to which shape
	public void BelongTo(Shape shape)
	{
		this.shape = shape;
	}

	public Shape GetShape()
	{
		return this.shape;
	}

	// Detect direction of line
	protected void DetectDirection()
	{
		if (start.x == end.x)
		{
			this.direction = (start.y > end.y) ? LineDirection.DOWN : LineDirection.UP;
		}
		else if (start.y == end.y)
		{
			this.direction = (start.x > end.x) ? LineDirection.LEFT : LineDirection.RIGHT;
		}
		else
		{
			this.direction = LineDirection.UNKNOWNED;
		}
	}

	public void Scale(float scale)
	{
		start *= scale;
		end *= scale;
	}

	// Start and end point of line
	public Vector3 start
	{
		set
		{
			if (lineRender != null)
			{
				lineRender.SetPosition(0, value);
				corner.transform.position = value;
			}
		}
		get
		{
			if (lineRender != null)
			{
				return lineRender.GetPosition(0);
			}
			else
			{
				Debug.Log("Error. LineRender not found");
				return Vector3.zero;
			}
		}
	}
	public Vector3 end
	{
		set
		{
			if (lineRender != null)
			{
				lineRender.SetPosition(1, value);
			}
		}
		get
		{
			if (lineRender != null)
			{
				return lineRender.GetPosition(1);
			}
			else
			{
				Debug.Log("Error. LineRender not found");
				return Vector3.zero;
			}
		}
	}
}

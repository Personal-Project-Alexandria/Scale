using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SlicerLine break rule of direction -- BEWARE TO USE
public class SlicerLine : Line {

	// After sliced info
	public class Info
	{
		public Line line;
		public Vector3 addedPoint;
	}
	
	[HideInInspector]
	public bool wait = false; // Stop doing 
	[HideInInspector]
	public Info info = null; // Store info after sliced
	[HideInInspector]
	public bool crash = false;

	private Line line; // Collider line


	protected Slicer slicer; // The slicer who create this sliceLine

	protected override void Init()
	{
		base.Init();
		gameObject.tag = "Slicer";
		wait = false;
	}

	// Create specify for slicerLine
	public void Create(LineDirection dir, Slicer slicer = null)
	{
		this.Init();
		this.direction = dir;
		this.slicer = slicer;
	}

	// Slice start growing
	public void Grow(float v)
	{
		if (wait == true)
		{
			return;
		}

		Vector3 velocity = Vector3.zero;

		switch (this.direction)
		{
		case LineDirection.UP:
			velocity = Vector3.up * v;
			break;

		case LineDirection.DOWN:
			velocity = Vector3.down * v;
			break;

		case LineDirection.LEFT:
			velocity = Vector3.left * v;
			break;

		case LineDirection.RIGHT:
			velocity = Vector3.right * v;
			break;

		default:
			return;
		}

		lineRender.SetPosition(1, end + velocity);
		ScaleCollider();
		//boxCollider.size += new Vector2(velocity.x, velocity.y);
		//boxCollider.offset += new Vector2(velocity.x, velocity.y) / 2;
	}

	// Clock-wise rotate
	public void Rotate()
	{
		int i = (int)direction + 1;

		// Because has 4 direction
		if (i > 3)
		{
			i = 0;
		}

		this.direction = (LineDirection)i;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Line") && !wait)
		{
			wait = true;
			line = col.GetComponent<Line>();
			Slicer.Instance.Slice();
		}
	}

	public void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Ball"))
		{
			Debug.Log(col.name);
			Slicer.Instance.ClearLine();
		}
	}

	public void MakeInfo()
	{
		if (info == null)
		{
			info = new Info();
		}

		info.line = line;

		transform.parent = line.GetShape().transform;
		lineRender.SetPosition(0, start + transform.localPosition);
		lineRender.SetPosition(1, end + transform.localPosition);
		transform.localPosition = Vector3.zero;
	
		info.addedPoint = PointInLine(this, info.line);
	}

	// Check point exactly
	public Vector3 PointInLine(SlicerLine slicerLine, Line normalLine)
	{
		Vector3 result = Vector3.zero;

		if (slicerLine.direction == LineDirection.UP || slicerLine.direction == LineDirection.DOWN)
		{
			result.x = slicerLine.start.x;
			result.y = normalLine.start.y;
		}
		else
		{
			result.x = normalLine.start.x;
			result.y = slicerLine.start.y;
		}

		return result;
	}

}

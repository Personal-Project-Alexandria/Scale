using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollider : MonoBehaviour
{
	public BaseSlicer slicer;
	public BoxCollider2D col;
	public PolygonCollider2D other;

	public void Start()
	{
		other = GameManager.Instance.shape.GetComponent<PolygonCollider2D>();
	}

	protected void Update()
	{
		
		if ((other = GameManager.Instance.shape.GetComponent<PolygonCollider2D>()) == null)
		{
			return;
		}

		slicer.available = CheckAllInWithRaycast(other);
	} 

	private List<Vector3> GetVertices(Collider2D col)
	{
		List<Vector3> result = new List<Vector3>();
		float size = col.bounds.size.x / 2;
		Vector3 center = col.bounds.center;
		result.Add(center + new Vector3(size, size));
		result.Add(center + new Vector3(size, -size));
		result.Add(center + new Vector3(-size, size));
		result.Add(center + new Vector3(-size, -size));
		result.Add(center);
		return result;
	}

	public bool CheckAllIn(Collider2D other)
	{
		List<Vector3> vertices = GetVertices(this.col);
		for (int i = 0; i < vertices.Count; i++)
		{
			if (!other.bounds.Contains(vertices[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckAllInWithRaycast(Collider2D other)
	{
		List<Vector3> vertices = GetVertices(this.col);
		for (int i = 0; i < vertices.Count; i++)
		{
			RaycastHit2D hit = Physics2D.Raycast(vertices[i], Vector3.zero, 0, 1 << LayerMask.NameToLayer("Shape"));
			if (hit.collider == null)
			{
				return false;
			}
		}
		return true;
	}
}
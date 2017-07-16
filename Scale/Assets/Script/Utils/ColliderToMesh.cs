using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ColliderToMesh : MonoBehaviour
{
	PolygonCollider2D pc2;
	void Start()
	{
		pc2 = gameObject.GetComponent<PolygonCollider2D>();
	}

	//[ContextMenu("Commit")]
	public void Commit(List<Vector3> points3)
	{
		//int pointCount = 0;
		//pointCount = pc2.GetTotalPointCount();
		//MeshFilter mf = GetComponent<MeshFilter>();
		//Mesh mesh = new Mesh();
		//Vector2[] points = pc2.points;
		//Vector3[] vertices = new Vector3[pointCount];
		//Vector2[] uv = new Vector2[pointCount];
		//for (int j = 0; j < pointCount; j++)
		//{
		//	Vector2 actual = points[j];
		//	vertices[j] = new Vector3(actual.x, actual.y, 0);
		//	uv[j] = actual;
		//}
		//Triangulator tr = new Triangulator(points);
		//int[] triangles = tr.Triangulate();
		//mesh.vertices = vertices;
		//mesh.triangles = triangles;
		//mesh.uv = uv;
		//mf.mesh = mesh;

		Vector2[] points = new Vector2[points3.Count];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new Vector2(points3[i].x, points3[i].y);
		}

		int pointCount = points.Length;
		MeshFilter mf = GetComponent<MeshFilter>();
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
	}
}
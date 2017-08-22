using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerManager : MonoBehaviour {

	public GameObject slicerPrefab;
	private List<BaseSlicer> slicers;
	[HideInInspector]
	public bool down = false;
	[HideInInspector]
	public float downTime = 0f;
	[HideInInspector]
	public bool growing = false;

	public float sliceSpeed = 3f;

	protected void Update()
	{
		if (down)
		{
			downTime += Time.deltaTime;
		}
	}

	public void Init(int count)
	{
		if (count <= 0)
		{
			return;
		}

		if (slicers == null)
		{
			slicers = new List<BaseSlicer>();
		}
		else
		{
			slicers.Clear();
		}

		float dis = 1.5f;
		float half = dis * (count - 1) / 2;
		for (int i = 0; i < count; i++)
		{
			GameObject slicerObject = (GameObject)Instantiate(slicerPrefab, transform);
			slicers.Add(slicerObject.GetComponent<BaseSlicer>());
			slicers[i].start = new Vector3(dis * i - half, -3.75f, 0);
			slicers[i].slicerManager = this;
		}
	}

	public void OnStart()
	{
		if (slicers == null)
		{
			return;
		}

		for (int i = 0; i < slicers.Count; i++)
		{
			slicers[i].OnStart();
		}
	}

	public void Pause()
	{
		if (slicers == null)
		{
			return;
		}

		for (int i = 0; i < slicers.Count; i++)
		{
			slicers[i].Pause();
		}
	}

	public void Continue()
	{
		if (slicers == null)
		{
			return;
		}

		for (int i = 0; i < slicers.Count; i++)
		{
			slicers[i].Continue();
		}
	}

	public void Restart(bool onLose = false)
	{
		if (slicers == null)
		{
			return;
		}

		for (int i = 0; i < slicers.Count; i++)
		{
			slicers[i].gameObject.SetActive(true);
			slicers[i].Restart(onLose);
		}
	}

	public void Clear()
	{
		foreach (BaseSlicer slicer in slicers)
		{
			Destroy(slicer.gameObject);
		}
		slicers.Clear();
	}

	public bool RemainSlicer()
	{
		for (int i = 0; i < slicers.Count; i++)
		{
			if (slicers[i].gameObject.activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerManager : MonoBehaviour {

	public GameObject slicerPrefab;
	private List<BaseSlicer> slicers;

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

		float partWidth = Screen.width / count;
		float partWidthCenter = partWidth / 2;
		float halfScreenWidth = Screen.width / 2;
		 
		for (int i = 0; i < count; i++)
		{
			GameObject slicerObject = (GameObject)Instantiate(slicerPrefab, transform);
			slicers.Add(slicerObject.GetComponent<BaseSlicer>());
			float worldX = Camera.main.ScreenToWorldPoint(new Vector3(partWidth * i + partWidthCenter, 0)).x;
			slicers[i].start = new Vector3(worldX, -3.5f, 0);
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
			slicers[i].Restart(onLose);
		}
	}
}

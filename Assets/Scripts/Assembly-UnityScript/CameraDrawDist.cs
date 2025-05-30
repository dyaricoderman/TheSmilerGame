using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CameraDrawDist : MonoBehaviour
{
	public float[] DrawDistances;

	public float TrackDrawDist;

	public float BuildingDrawDist;

	public float DetailDrawDist;

	public float TerrainDrawDist;

	public virtual void Start()
	{
		GetComponent<Camera>().farClipPlane = DrawDistances[Performance.GamePerformance];
		float[] array = new float[32];
		array[9] = TrackDrawDist * GetComponent<Camera>().farClipPlane;
		array[10] = BuildingDrawDist * GetComponent<Camera>().farClipPlane;
		array[11] = DetailDrawDist * GetComponent<Camera>().farClipPlane;
		array[12] = TerrainDrawDist * GetComponent<Camera>().farClipPlane;
		GetComponent<Camera>().layerCullDistances = array;
		for (int i = 0; i < Extensions.get_length((System.Array)array); i++)
		{
			if (!(GetComponent<Camera>().farClipPlane >= array[i]))
			{
				GetComponent<Camera>().farClipPlane = array[i];
			}
		}
	}

	public virtual void Main()
	{
	}
}

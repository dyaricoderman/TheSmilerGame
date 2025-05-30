using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetBGCameraLayerBehaviour : MonoBehaviour
{
	public int CameraLayer;

	private void Awake()
	{
		ApplyCameraLayerRecursive(base.gameObject);
		GetComponent<Camera>().cullingMask |= 1 << CameraLayer;
	}

	private void ApplyCameraLayerRecursive(GameObject go)
	{
		go.layer = CameraLayer;
		for (int i = 0; i < go.transform.childCount; i++)
		{
			ApplyCameraLayerRecursive(go.transform.GetChild(i).gameObject);
		}
	}
}

using UnityEngine;

public class ScreenPosition3D : MonoBehaviour
{
	public Camera cam;

	public Vector2 screenPosition;

	public float dispZPos = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = cam.ViewportToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, dispZPos));
	}
}

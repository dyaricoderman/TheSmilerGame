using UnityEngine;

public class BlueprintPoint : MonoBehaviour
{
	public Transform viewportGO;

	public Vector2 viewportPos;

	public bool display = true;

	public float opacity;

	public UnityEngine.UI.Image tex;

	private void Start()
	{
	}

	public void SetOpacity(float newOpacity)
	{
		opacity = newOpacity;
	}

	public void UpdateDisplay()
	{
		if (display)
		{
			viewportPos = Camera.main.WorldToViewportPoint(base.transform.position);
			tex.color = new Color(0.5f, 0.5f, 0.5f, opacity);
			viewportGO.position = viewportPos;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 10f);
	}
}

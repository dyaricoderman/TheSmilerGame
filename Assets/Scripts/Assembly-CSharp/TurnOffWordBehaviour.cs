using UnityEngine;

public class TurnOffWordBehaviour : MonoBehaviour
{
	private void Awake()
	{
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			Object.Destroy(component);
			Transform transform = base.transform.Find("Text");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
		}
	}
}

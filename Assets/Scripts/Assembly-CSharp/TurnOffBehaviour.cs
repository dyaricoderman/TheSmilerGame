using UnityEngine;

public class TurnOffBehaviour : MonoBehaviour
{
	private void Awake()
	{
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			Object.Destroy(component);
			MeshFilter component2 = GetComponent<MeshFilter>();
			Object.Destroy(component2);
		}
	}
}

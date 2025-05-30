using UnityEngine;

public class MaskOutBehaviour : MonoBehaviour
{
	public Material maskMaterial;

	private void Start()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return;
		}
		int num = base.GetComponent<Renderer>().materials.Length;
		if (num == 1)
		{
			base.GetComponent<Renderer>().sharedMaterial = maskMaterial;
			return;
		}
		Material[] array = new Material[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = maskMaterial;
		}
		base.GetComponent<Renderer>().sharedMaterials = array;
	}
}

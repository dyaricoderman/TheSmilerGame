using System;
using UnityEngine;

[Serializable]
public class ShaderSwap : MonoBehaviour
{
	public Material[] SwapMaterials;

	public virtual void Start()
	{
		if (SwapMaterials.Length != 3)
		{
			MonoBehaviour.print("SwapMaterials has wrong number of materials on " + gameObject.name);
		}
		GetComponent<Renderer>().material = SwapMaterials[Performance.GamePerformance];
	}

	public virtual void Main()
	{
	}
}

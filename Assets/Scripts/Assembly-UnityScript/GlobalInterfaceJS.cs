using System;
using UnityEngine;

[Serializable]
public class GlobalInterfaceJS : MonoBehaviour
{
	[NonSerialized]
	public static GameObject GIgameObject;

	public virtual void Awake()
	{
		if (!GIgameObject)
		{
			GIgameObject = gameObject;
		}
	}

	public virtual void Die()
	{
		UnityEngine.Object.Destroy(gameObject);
	}

	public virtual void Main()
	{
	}
}

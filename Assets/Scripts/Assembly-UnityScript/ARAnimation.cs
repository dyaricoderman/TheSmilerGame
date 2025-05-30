using System;
using UnityEngine;

[Serializable]
public class ARAnimation : MonoBehaviour
{
	public Animation Blackout;

	public Animation Logo;

	public virtual void StartAnimation()
	{
		Blackout.Stop();
		Logo.Stop();
		Logo.Rewind("flipperLoopAnim");
		Logo.Rewind("flipperAppearAnim");
		Blackout.Play();
		Logo.Play("flipperAppearAnim");
		Logo.PlayQueued("flipperLoopAnim", QueueMode.CompleteOthers);
	}

	public virtual void StopAnimation()
	{
	}

	public virtual void Main()
	{
	}
}

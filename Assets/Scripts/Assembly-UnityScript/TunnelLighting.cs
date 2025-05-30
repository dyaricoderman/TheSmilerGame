using System;
using UnityEngine;

[Serializable]
public class TunnelLighting : MonoBehaviour
{
	public tunnelLight Mode;

	public AnimationCurve tunnelEnterCurve;

	public AnimationCurve tunnelExitCurve;

	public float AmbiantLightLevel;

	public TunnelLighting()
	{
		Mode = tunnelLight.Both;
		tunnelEnterCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.2f, 0.6f), new Keyframe(1f, 0.2f));
		tunnelExitCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.2f, 0.3f), new Keyframe(0.7f, 0f));
		AmbiantLightLevel = -0.5f;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (Mode == tunnelLight.Enter || Mode == tunnelLight.Both)
		{
			Enter();
		}
		if (Mode == tunnelLight.Exit)
		{
			Exit();
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (Mode == tunnelLight.Both)
		{
			Exit();
		}
	}

	public virtual void Exit()
	{
		CartCam.inst.SetAmbiantLight(0f);
		CartCam.inst.ApplyExposureChange(tunnelExitCurve);
	}

	public virtual void Enter()
	{
		CartCam.inst.SetAmbiantLight(AmbiantLightLevel);
		CartCam.inst.ApplyExposureChange(tunnelEnterCurve);
	}

	public virtual void Main()
	{
	}
}

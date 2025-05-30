using System;
using UnityEngine;

[Serializable]
public class BoostGUIControl : MonoBehaviour
{
	public Texture2D ButtonOn;

	public Texture2D ButtonCharged;

	public Texture2D ButtonOff;

	public Material butMat;

	public Material barMat;

	public int animationFrames;

	public Material SpeedBarMat;

	public Camera OrthoCam;

	private float FillAmount;

	private Vector3 startpos;

	private int Frame;

	public BoostGUIControl()
	{
		animationFrames = 5;
	}

	public virtual void SetSpeed(float sped)
	{
		SpeedBarMat.SetTextureOffset("_MainTex", new Vector2(0f, 1f - sped));
	}

	public virtual void SetRightLeft(bool right)
	{
		if (right)
		{
			transform.position = OrthoCam.transform.TransformPoint(new Vector3(OrthoCam.aspect * -5f, -4.2f, 10f));
			int num = 1;
			Vector3 localScale = transform.localScale;
			float num2 = (localScale.z = num);
			Vector3 vector = (transform.localScale = localScale);
		}
		else
		{
			transform.position = OrthoCam.transform.TransformPoint(new Vector3(OrthoCam.aspect * 5f, -4.2f, 10f));
			int num3 = -1;
			Vector3 localScale2 = transform.localScale;
			float num4 = (localScale2.z = num3);
			Vector3 vector3 = (transform.localScale = localScale2);
		}
	}

	public virtual void SetFill(float fill)
	{
		FillAmount = 1f - fill;
		if (!(fill > 0f))
		{
			butMat.SetTexture("_MainTex", ButtonOff);
		}
		else if (Cart.inst.Boosting)
		{
			butMat.SetTexture("_MainTex", ButtonOn);
		}
		else
		{
			butMat.SetTexture("_MainTex", ButtonCharged);
		}
	}

	public virtual void Start()
	{
		startpos = OrthoCam.transform.InverseTransformPoint(transform.position);
		barMat.mainTextureScale = new Vector2(1f / (float)animationFrames, 1f);
		SetFill(0f);
	}

	public virtual void Update()
	{
		Frame++;
		Frame = (int)Mathf.Repeat(Frame, animationFrames);
		barMat.SetTextureOffset("_MainTex", new Vector2(((float)Frame + 0f) / (float)animationFrames, FillAmount));
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}

	public virtual void Main()
	{
	}
}

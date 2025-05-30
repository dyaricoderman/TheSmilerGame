using UnityEngine;

public class Blueprint : MonoBehaviour
{
	public bool display = true;

	public float displayRotation;

	public float angleDiff;

	public float rotationSize = 45f;

	public BlueprintPoint point1;

	public BlueprintPoint point2;

	public LineRenderer line;

	public AnimationCurve popupCurveY;

	public AnimationCurve opacityCurve;

	private void Start()
	{
		line = base.gameObject.GetComponent<LineRenderer>();
		popupCurveY = new AnimationCurve();
		opacityCurve = new AnimationCurve();
		Keyframe key = new Keyframe(0f, point2.transform.localPosition.y);
		float inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		popupCurveY.AddKey(key);
		key = new Keyframe(rotationSize * 0.25f, point2.transform.localPosition.y);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		popupCurveY.AddKey(key);
		key = new Keyframe(rotationSize * 0.75f, 0f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		popupCurveY.AddKey(key);
		key = new Keyframe(rotationSize, 0f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		popupCurveY.AddKey(key);
		key = new Keyframe(0f, 0.5f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		opacityCurve.AddKey(key);
		key = new Keyframe(rotationSize * 0.25f, 0.5f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		opacityCurve.AddKey(key);
		key = new Keyframe(rotationSize * 0.75f, 0f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		opacityCurve.AddKey(key);
		key = new Keyframe(rotationSize, 0f);
		inTangent = (key.outTangent = 0f);
		key.inTangent = inTangent;
		opacityCurve.AddKey(key);
	}

	private void Update()
	{
		angleDiff = Mathf.Abs(Mathf.DeltaAngle(displayRotation, Camera.main.transform.rotation.eulerAngles.y));
		if ((double)angleDiff < (double)rotationSize * 0.5)
		{
			SetDisplaying(true);
		}
		else
		{
			SetDisplaying(false);
		}
		if (display)
		{
			point1.transform.position = base.transform.position;
			point1.SetOpacity(opacityCurve.Evaluate(angleDiff * 2f));
			point1.UpdateDisplay();
			point2.transform.position = new Vector3(point1.transform.position.x, point1.transform.position.y + popupCurveY.Evaluate(angleDiff * 2f), point1.transform.position.z);
			point2.SetOpacity(opacityCurve.Evaluate(angleDiff * 2f));
			point2.UpdateDisplay();
			line.SetPosition(0, point1.transform.position);
			line.SetPosition(1, point2.transform.position);
			line.SetColors(new Color(0f, 0f, 0f, opacityCurve.Evaluate(angleDiff * 2f)), new Color(0f, 0f, 0f, opacityCurve.Evaluate(angleDiff * 2f)));
		}
	}

	private void SetDisplaying(bool state)
	{
		if (state != display)
		{
			display = state;
			if (display)
			{
				line.enabled = true;
				point1.gameObject.SetActiveRecursively(true);
				point2.gameObject.SetActiveRecursively(true);
			}
			else
			{
				line.enabled = false;
				point1.gameObject.SetActiveRecursively(false);
				point2.gameObject.SetActiveRecursively(false);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
	}
}

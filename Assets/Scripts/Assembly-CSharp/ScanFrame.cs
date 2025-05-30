using UnityEngine;

public class ScanFrame : MonoBehaviour
{
	public UnityEngine.UI.Image GUIFrame;

	public Color colorStart = new Color(0.5f, 0.5f, 0.5f);

	public Color colorEnd = new Color(0.2f, 0.2f, 0.2f);

	public float duration = 1f;

	private void Update()
	{
		float t = Mathf.PingPong(Time.time, duration) / duration;
		if ((bool)GUIFrame)
		{
			GUIFrame.color = Color.Lerp(colorStart, colorEnd, t);
		}
	}
}

using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
	private Achievement[] Achievements;

	private GUISkin guiSkin;

	public Texture footerTex;

	public Texture headerTex;

	public Texture headerBGTex;

	public Texture unlockedTex;

	public Texture lockedTex;

	private float ScrollOffset;

	private float ScrollStart;

	private float ScrollSpeed;

	private void Awake()
	{
		base.enabled = false;
	}

	private void Start()
	{
		guiSkin = FontResize.ResizedSkin;
	}

	private void AchievementsDisplay()
	{
		Achievements = AchievementManager.GetAchievements();
		base.enabled = true;
	}

	private void OnGUI()
	{
		GUI.skin = guiSkin;
		for (int i = 0; i < Achievements.Length; i++)
		{
			Rect rect = new Rect(((float)i * 0.29f - ScrollOffset) * (float)Screen.width, 0.22f * (float)Screen.height, 0.29f * (float)Screen.width, 0.67f * (float)Screen.height);
			rect.x = Mathf.Floor(rect.x);
			DrawCell(i, rect);
		}
		DrawHeader(new Rect(0f, 0f, 1f * (float)Screen.width, 0.275f * (float)Screen.height));
		DrawFooter(new Rect(0f, 0.85f * (float)Screen.height, 1f * (float)Screen.width, 0.15f * (float)Screen.height));
	}

	public void DrawCell(int i, Rect rect)
	{
		if (Achievements[i].Earned)
		{
			GUI.DrawTexture(FluidRect(0f, 0f, 1.025f, 1f, rect), unlockedTex);
			GUI.color = Color.black;
			GUI.DrawTexture(FluidRect(0.1f, 0.25f, 0.8f, 0.5f, rect), Achievements[i].Icon, ScaleMode.ScaleToFit);
		}
		else
		{
			GUI.color = Color.white;
			GUI.DrawTexture(FluidRect(0f, 0f, 1.025f, 1f, rect), lockedTex);
			GUI.DrawTexture(FluidRect(0.1f, 0.25f, 0.8f, 0.5f, rect), Achievements[i].Icon, ScaleMode.ScaleToFit);
		}
		GUI.Label(FluidRect(0.05f, 0.1f, 0.9f, 0.2f, rect), Achievements[i].DisplayName, "WhiteTitle");
		GUI.Label(FluidRect(0.05f, 0.725f, 0.9f, 0.225f, rect), Achievements[i].Description, "WhiteLabel");
		GUI.color = Color.white;
	}

	public void DrawHeader(Rect rect)
	{
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1f, rect), headerBGTex);
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 0.5f, rect), headerTex);
		GUI.Label(FluidRect(0.1f, 0f, 0.8f, 0.4f, rect), "YOUR TROPHIES", "WhiteTitle");
		GUI.color = Color.black;
		GUI.Label(FluidRect(0.1f, 0.3f, 0.8f, 0.7f, rect), "Complete actions in the game to claim trophies.", "WhiteLabel");
		GUI.color = Color.white;
	}

	private Rect FluidRect(float x, float y, float width, float height, Rect cell)
	{
		return new Rect(cell.x + x * cell.width, cell.y + y * cell.height, cell.width * width, cell.height * height);
	}

	public void DrawFooter(Rect rect)
	{
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1f, rect), footerTex);
		GUI.color = Color.black;
		if (GUI.Button(FluidRect(0f, 0.15f, 0.35f, 1f, rect), "Back", "WhiteTitle"))
		{
			base.enabled = false;
			base.gameObject.SendMessage("MainMenuDisplay", true);
		}
		GUI.color = Color.white;
	}

	private Vector2 GetAverageTouch()
	{
		Vector2 vector = new Vector2(0f, 0f);
		for (int i = 0; i < Input.touches.Length; i++)
		{
			if (Input.touches[i].fingerId < 5)
			{
				vector += Input.touches[i].position;
			}
		}
		return vector / Input.touchCount;
	}

	public void Update()
	{
		if (Input.touchCount > 0)
		{
			Vector2 averageTouch = GetAverageTouch();
			if ((double)averageTouch.y > (double)Screen.height * 0.2 && (double)averageTouch.y < 0.8 * (double)Screen.height)
			{
				if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
				{
					ScrollStart = averageTouch.x;
				}
				if (Input.touchCount == 1 && Input.touches[0].phase != TouchPhase.Ended)
				{
					ScrollSpeed = (averageTouch.x - ScrollStart) / (float)Screen.width;
				}
				ScrollStart = averageTouch.x;
			}
		}
		ScrollOffset -= ScrollSpeed;
		ScrollOffset = ClampScroll(ScrollOffset, 0.29f * (float)Achievements.Length);
		ScrollSpeed += Mathf.Clamp(0f - ScrollSpeed, -0.4f, 0.4f) * Time.deltaTime;
	}

	private void FixedUpdate()
	{
		ScrollSpeed -= Time.deltaTime * ScrollSpeed;
	}

	private float ClampScroll(float scroll, float ScrollDist)
	{
		return Mathf.Clamp(scroll, 0f, Mathf.Max(0f, ScrollDist - 1f));
	}
}

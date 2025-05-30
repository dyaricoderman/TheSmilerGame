using UnityEngine;

public class AchievementsDisplay : MonoBehaviour
{
	private Achievement[] Achievements;

	private int listeningTouchID;

	public float friction = 0.99f;

	public float bounceBack = 0.95f;

	private Vector2 swipeAcceleration = Vector2.zero;

	private Vector2 position = Vector2.zero;

	public float containerWidth = 0.8f;

	public float containerHeight = 0.8f;

	public Vector2 containerMargin = new Vector2(0.2f, 0.2f);

	public float columns = 4f;

	public int rows = 2;

	public Vector2 separation = new Vector2(0.2f, 0.2f);

	public float width = 0.8f;

	public float height = 0.8f;

	private Vector2 mousePosDelta = Vector2.zero;

	private Vector2 lastMousePos = Vector2.zero;

	public float totalWidth;

	private float xPos;

	private float yPos;

	private float actualWidth;

	private float actualHeight;

	private Vector2 actualSeparation;

	private void Start()
	{
		Achievements = AchievementManager.GetAchievements();
	}

	private void Update()
	{
		Vector2 vector = Input.mousePosition;
		mousePosDelta = vector - lastMousePos;
		lastMousePos = vector;
		if (Input.GetMouseButton(0))
		{
			swipeAcceleration = Vector2.zero;
			mousePosDelta.x /= Screen.width;
			mousePosDelta.y /= Screen.height;
			swipeAcceleration += mousePosDelta;
		}
		else
		{
			swipeAcceleration.x *= friction;
			swipeAcceleration.y *= friction;
		}
		position.x -= swipeAcceleration.x;
		if (position.x < 0f)
		{
			float num = 0f - position.x;
			position.x += num * bounceBack;
			if ((double)position.x > -0.001)
			{
				position.x = 0f;
			}
		}
		if (position.x > totalWidth)
		{
			float num2 = totalWidth - position.x;
			position.x += num2 * bounceBack;
			if ((double)position.x < (double)totalWidth + 0.001)
			{
				position.x = totalWidth;
			}
		}
	}

	private void AchevementsDisplay(bool state)
	{
		base.enabled = state;
	}

	private void OnGUI()
	{
		if (rows <= 0)
		{
			rows = 1;
		}
		if (columns <= 0f)
		{
			columns = 1f;
		}
		actualWidth = (width - separation.x) / columns;
		actualHeight = (height - separation.y) / (float)rows;
		actualSeparation = new Vector2(separation.x / (columns + 1f), separation.y / (float)(rows + 1));
		GUI.Box(FluidRect(0f, 0.025f, 1f, 0.1f), "-- Achievements --");
		for (int i = 0; i < Achievements.Length; i++)
		{
			xPos = actualSeparation.x - position.x;
			yPos = actualSeparation.y - position.y;
			xPos += (float)i * (actualWidth + actualSeparation.x) / (float)rows;
			xPos -= (actualWidth + actualSeparation.x) / (float)rows * (float)(i % rows);
			yPos += (float)(i % rows) * (actualHeight + actualSeparation.y);
			Rect container = new Rect(xPos, yPos, actualWidth, actualHeight);
			GUI.Box(RelativeFluidRect(0f, 0f, 1f, 1f, container), string.Empty);
			GUI.DrawTexture(RelativeFluidRect(0.1f, 0.025f, 0.8f, 0.55f, container), Achievements[i].Icon, ScaleMode.ScaleToFit);
			GUI.Box(RelativeFluidRect(0.1f, 0.6f, 0.8f, 0.3f, container), Achievements[i].Name);
			GUI.Box(RelativeFluidRect(0.1f, 0.775f, 0.8f, 0.3f, container), Achievements[i].Description);
			if (GUI.Button(FluidRect(0.025f, 0.9f, 0.3f, 0.09f), "BACK"))
			{
				base.enabled = false;
				base.gameObject.SendMessage("MainMenuDisplay", true);
			}
		}
		totalWidth = (actualWidth + actualSeparation.x) * (float)(Achievements.Length / rows) + actualSeparation.x - 1f;
	}

	public Rect RelativeFluidRect(float x, float y, float width, float height, Rect container)
	{
		return ContainerRelativeRect(container.x + x * container.width, container.y + y * container.height, container.width * width, container.height * height);
	}

	public Rect ContainerRelativeRect(float x, float y, float width, float height)
	{
		return new Rect(containerMargin.x * (float)Screen.width + x * containerWidth * (float)Screen.width, containerMargin.y * (float)Screen.height + y * containerHeight * (float)Screen.height, width * containerWidth * (float)Screen.width, height * containerHeight * (float)Screen.height);
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}

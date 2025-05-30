using UnityEngine;

public class WinningsDisplay : MonoBehaviour
{
	private Competition competition;

	private CompetitionLogic competitionLogic;

	public GUISkin guiSkin;

	private float gridTopMargin = 0.1f;

	private float gridBottomMargin = 0.12f;

	private float gridSize;

	public Vector2 cellSize = new Vector2(1f, 0.3f);

	public Texture footerTex;

	public Texture headerTex;

	public Texture prizeBGTex;

	public Texture blackBGTex;

	public float ScrollOffset;

	public float ScrollStart;

	public float ScrollStartStart;

	public float ScrollSpeed;

	public bool canClick = true;

	public int gridLength
	{
		get
		{
			return competitionLogic.wins.Count;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		guiSkin = FontResize.ResizedSkin;
		competition = base.gameObject.GetComponent<Competition>();
		competitionLogic = base.gameObject.GetComponent<CompetitionLogic>();
		base.enabled = false;
		gridSize = 1f - gridTopMargin - gridBottomMargin;
	}

	private void OnGUI()
	{
		GUI.skin = guiSkin;
		DrawBackground(new Rect(0f, 0f, 1f * (float)Screen.width, 1 * Screen.height));
		DrawFooter(new Rect(0f, 0.85f * (float)Screen.height, 1f * (float)Screen.width, 0.15f * (float)Screen.height));
		for (int i = 0; i < gridLength; i++)
		{
			Rect rect = new Rect(0f, ((float)i * cellSize.y - ScrollOffset + gridTopMargin) * (float)Screen.height, cellSize.x * (float)Screen.width, cellSize.y * (float)Screen.height);
			rect.x = Mathf.Floor(rect.x);
			DrawCell(i, rect);
		}
		DrawHeader(new Rect(0f, 0f, 1f * (float)Screen.width, 0.15f * (float)Screen.height));
		DrawFooter(new Rect(0f, 0.85f * (float)Screen.height, 1f * (float)Screen.width, 0.15f * (float)Screen.height));
	}

	public void DrawBackground(Rect rect)
	{
		GUI.skin = guiSkin;
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1f, rect), blackBGTex, ScaleMode.StretchToFill);
	}

	public void DrawCell(int i, Rect rect)
	{
		if (GUI.Button(FluidRect(0f, 0f, 1f, 1f, rect), string.Empty))
		{
			if (!canClick)
			{
				return;
			}
			ScrollSpeed = 0f;
			base.gameObject.SendMessage("ShowDetail", competitionLogic.wins[i]);
			base.enabled = false;
		}
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1.1f, rect), prizeBGTex);
		GUI.color = Color.black;
		GUI.DrawTexture(FluidRect(0.05f, 0.15f, 0.175f, 0.75f, rect), competitionLogic.prizeIcons[competitionLogic.wins[i].Prize.PrizeDescription.IconID], ScaleMode.ScaleToFit);
		GUI.Box(FluidRect(0.225f, 0.25f, 0.375f, 0.15f, rect), competitionLogic.wins[i].Prize.PrizeDescription.Name, "WhiteTitle");
		GUI.Box(FluidRect(0.225f, 0.45f, 0.375f, 0.55f, rect), competitionLogic.wins[i].Prize.PrizeDescription.ShortDescription, "WhiteLabel");
		GUI.Box(FluidRect(0.6f, 0.15f, 0.35f, 0.75f, rect), competitionLogic.wins[i].Prize.Code, "WhiteLabel");
		GUI.color = Color.white;
	}

	public void DrawHeader(Rect rect)
	{
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1f, rect), headerTex);
		GUI.color = Color.yellow;
		GUI.Label(FluidRect(0.1f, 0f, 0.8f, 0.6f, rect), "WINNINGS", "WhiteTitle");
		GUI.color = Color.white;
	}

	public void DrawFooter(Rect rect)
	{
		GUI.DrawTexture(FluidRect(0f, 0f, 1f, 1f, rect), footerTex);
		GUI.color = Color.black;
		if (GUI.Button(FluidRect(0f, 0.15f, 0.35f, 1f, rect), "BACK", "WhiteTitle"))
		{
			base.enabled = false;
			competition.ShowSpinner();
		}
		if (GUI.Button(FluidRect(0.65f, 0.15f, 0.35f, 1f, rect), "SUPPORT", "WhiteTitle"))
		{
			base.enabled = false;
			competition.ShowSupport();
			competition.StartCoroutine(competition.LoadSupportScreen(0.5f));
		}
		if (GUI.Button(FluidRect(0.35f, 0.15f, 0.3f, 1f, rect), string.Empty, "WhiteTitle"))
		{
			Debug.Log("DO NOTHING");
		}
		GUI.color = Color.white;
	}

	private Rect FluidRect(float x, float y, float width, float height, Rect cell)
	{
		return new Rect(cell.x + x * cell.width, cell.y + y * cell.height, cell.width * width, cell.height * height);
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
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
					ScrollStart = averageTouch.y;
					canClick = true;
					ScrollStartStart = averageTouch.y;
				}
				if (Input.touchCount == 1 && Input.touches[0].phase != TouchPhase.Ended)
				{
					ScrollSpeed = (averageTouch.y - ScrollStart) / (float)Screen.height;
				}
				ScrollStart = averageTouch.y;
			}
		}
		if (canClick)
		{
			if ((double)Mathf.Abs(ScrollStart - ScrollStartStart) > (double)Screen.height * 0.025)
			{
				canClick = false;
			}
			if ((double)Mathf.Abs(ScrollSpeed) > 0.02)
			{
				canClick = false;
			}
		}
		ScrollOffset += ScrollSpeed;
		ScrollOffset = ClampScroll(ScrollOffset, cellSize.y * (float)gridLength);
		ScrollSpeed += Mathf.Clamp(0f - ScrollSpeed, -0.4f, 0.4f) * Time.deltaTime;
	}

	private void FixedUpdate()
	{
		ScrollSpeed -= Time.deltaTime * ScrollSpeed;
	}

	private float ClampScroll(float scroll, float ScrollDist)
	{
		return Mathf.Clamp(scroll, 0f, Mathf.Max(0f, ScrollDist - gridSize));
	}
}

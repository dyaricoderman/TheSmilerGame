using UnityEngine;

public class GridList : MonoBehaviour
{
	public bool display;

	private int listeningTouchID;

	public bool horizontalScroll;

	public bool verticalScroll = true;

	public DisplayStyle displayStyle;

	public float friction = 3.5f;

	public float bounceBack = 25f;

	public Vector2 mouseDownPosition = Vector2.zero;

	private Vector2 currentMousePos = Vector2.zero;

	public Vector2 lastMousePos = Vector2.zero;

	public Vector2 mousePosDelta = Vector2.zero;

	private Vector2 mouseMoveDist = Vector2.zero;

	private Vector2 swipeAcceleration = Vector2.zero;

	public Vector2 position = Vector2.zero;

	public Rect container = new Rect(0.1f, 0.1f, 0.8f, 0.8f);

	public Vector2 cellSize = new Vector2(0.8f, 0.8f);

	public Vector2 cellMargin = new Vector2(0.1f, 0.1f);

	public Rect header = new Rect(0f, 0f, 1f, 1f);

	public Rect footer = new Rect(0f, 0f, 1f, 1f);

	public int numRows;

	public int numColumns;

	public float totalWidth;

	public float totalHeight;

	public GridListInterface customInterface;

	public IItemTemplate itemTemplate;

	public bool canClick = true;

	private float xPos;

	private float yPos;

	private void Start()
	{
		if (customInterface != null)
		{
			itemTemplate = customInterface as IItemTemplate;
		}
		numRows = ((numRows < 1) ? 1 : numRows);
		numColumns = ((numColumns < 1) ? 1 : numColumns);
		container.width = ((!(container.width < 0f)) ? container.width : 0f);
		container.height = ((!(container.height < 0f)) ? container.height : 0f);
		container.x = ((!(container.x < 0f)) ? container.x : 0f);
		container.y = ((!(container.y < 0f)) ? container.y : 0f);
		container.width = ((!(container.width < 0f)) ? container.width : 0f);
		container.height = ((!(container.height < 0f)) ? container.height : 0f);
		header.width = ((!(header.width < 0f)) ? header.width : 0f);
		header.width = ((!(header.width > 1f)) ? header.width : 1f);
		header.height = ((!(header.height < 0f)) ? header.height : 0f);
		header.height = ((!(header.height > 1f)) ? header.height : 1f);
		footer.width = ((!(footer.width < 0f)) ? footer.width : 0f);
		footer.width = ((!(footer.width > 1f)) ? footer.width : 1f);
		footer.height = ((!(footer.height < 0f)) ? footer.height : 0f);
		footer.height = ((!(footer.height > 1f)) ? footer.height : 1f);
		cellSize.x = ((!(cellSize.x < 0f)) ? cellSize.x : 0f);
		cellSize.y = ((!(cellSize.y < 0f)) ? cellSize.y : 0f);
		cellMargin.x = ((!(cellMargin.x > 0.5f)) ? cellMargin.x : 0.5f);
		cellMargin.y = ((!(cellMargin.y > 0.5f)) ? cellMargin.y : 0.5f);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mouseMoveDist = Vector2.zero;
			canClick = true;
		}
		if (!Input.GetMouseButtonUp(0))
		{
		}
	}

	private void FixedUpdate()
	{
		if (!display)
		{
			return;
		}
		if (displayStyle == DisplayStyle.horizontal || displayStyle == DisplayStyle.vertical)
		{
		}
		if (Input.touchCount > 0)
		{
			currentMousePos = Input.touches[0].position;
		}
		mousePosDelta = Vector2.zero;
		if (horizontalScroll)
		{
			mousePosDelta.x += currentMousePos.x - lastMousePos.x;
		}
		if (verticalScroll)
		{
			mousePosDelta.y += currentMousePos.y - lastMousePos.y;
		}
		mouseMoveDist.y += mousePosDelta.x;
		mouseMoveDist.x += mousePosDelta.y;
		if (canClick)
		{
			if ((double)Mathf.Abs(mousePosDelta.x) > (double)Screen.height * 0.05 || (double)Mathf.Abs(mousePosDelta.y) > (double)Screen.height * 0.05)
			{
				canClick = false;
			}
			if ((double)(Mathf.Abs(mouseMoveDist.x) + Mathf.Abs(mouseMoveDist.y)) > (double)Screen.height * 0.1)
			{
				canClick = false;
			}
		}
		lastMousePos = currentMousePos;
		if (Input.touchCount > 0)
		{
			swipeAcceleration = Vector2.zero;
			mousePosDelta.x /= Screen.width;
			mousePosDelta.y /= Screen.height;
			swipeAcceleration += mousePosDelta;
		}
		else
		{
			swipeAcceleration.x -= Time.deltaTime * swipeAcceleration.x * friction;
			swipeAcceleration.y -= Time.deltaTime * swipeAcceleration.y * friction;
		}
		if (horizontalScroll)
		{
			position.x -= swipeAcceleration.x;
		}
		if (verticalScroll)
		{
			position.y += swipeAcceleration.y;
		}
		totalWidth = (cellSize.x + cellMargin.x) * (float)numColumns * (container.width - container.x) - 1f;
		totalHeight = (cellSize.y + cellMargin.y) * (float)numRows * (container.height + container.y) - 1f;
		if (position.x < 0f)
		{
			float num = 0f - position.x;
			position.x += num * bounceBack * Time.deltaTime;
			if ((double)position.x > -0.001)
			{
				position.x = 0f;
			}
		}
		if (position.x > container.width)
		{
			float num2 = container.width - position.x;
			position.x += num2 * bounceBack * Time.deltaTime;
			if ((double)position.x < (double)container.width + 0.001)
			{
				position.x = container.width;
			}
		}
		if (position.y < 0f)
		{
			float num3 = 0f - position.y;
			position.y += num3 * bounceBack * Time.deltaTime;
			if ((double)position.y > -0.001)
			{
				position.y = 0f;
			}
		}
		if (position.y > container.height)
		{
			float num4 = container.height - position.y;
			position.y += num4 * bounceBack * Time.deltaTime;
			if ((double)position.y < (double)container.height + 0.001)
			{
				position.y = container.height;
			}
		}
	}

	public void ResetGrid()
	{
		FreezeMovement();
		position.x = 0f;
		position.y = 0f;
	}

	public void FreezeMovement()
	{
		swipeAcceleration.x = 0f;
		swipeAcceleration.y = 0f;
	}

	private void OnGUI()
	{
		if (!display)
		{
			return;
		}
		itemTemplate.DrawBackground(FluidRect(0f, 0f, Screen.width, Screen.height));
		itemTemplate.DrawContainer(FluidRect(container.x, container.y, container.width, container.height));
		itemTemplate.DrawFooter(FluidRect(footer.x, footer.y, footer.width, footer.height));
		for (int i = 0; i < itemTemplate.gridLength; i++)
		{
			xPos = container.x + cellMargin.x;
			yPos = container.y + cellMargin.y;
			if (displayStyle == DisplayStyle.horizontal)
			{
				xPos += (cellSize.x + cellMargin.x) * (float)(i % numColumns);
				yPos += (cellSize.y + cellMargin.y) * Mathf.Floor(i / numColumns);
			}
			else if (displayStyle == DisplayStyle.vertical)
			{
				xPos += (float)i * (cellSize.x + cellMargin.x) / (float)numRows;
				xPos -= (cellSize.x + cellMargin.x) / (float)numRows * (float)(i % numRows);
				yPos += (float)(i % numRows) * (cellSize.y + cellMargin.y);
			}
			xPos -= position.x;
			yPos -= position.y;
			itemTemplate.DrawCell(i, FluidRect(xPos, yPos, cellSize.x, cellSize.y));
		}
		itemTemplate.DrawHeader(FluidRect(header.x, header.y, header.width, header.height));
		itemTemplate.DrawFooter(FluidRect(footer.x, footer.y, footer.width, footer.height));
		itemTemplate.DrawGUI(FluidRect(0f, 0f, Screen.width, Screen.height));
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}

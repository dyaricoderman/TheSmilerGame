using UnityEngine;

[RequireComponent(typeof(GridList))]
public class GridListInterface : MonoBehaviour
{
	public GridList gridList;

	private void Awake()
	{
		if (!gridList)
		{
			gridList = base.gameObject.GetComponent<GridList>();
		}
	}

	private void SetupGridList()
	{
	}

	public void SetActive(bool state)
	{
		gridList.display = state;
	}

	public Rect RelativeFluidRect(float x, float y, float width, float height, Rect cell)
	{
		return new Rect(cell.x + x * cell.width, cell.y + y * cell.height, cell.width * width, cell.height * height);
	}
}

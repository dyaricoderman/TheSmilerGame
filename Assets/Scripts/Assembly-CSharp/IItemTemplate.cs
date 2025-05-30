using UnityEngine;

public interface IItemTemplate
{
	int gridLength { get; }

	void DrawBackground(Rect rect);

	void DrawContainer(Rect rect);

	void DrawCell(int i, Rect rect);

	void DrawHeader(Rect rect);

	void DrawFooter(Rect rect);

	void DrawGUI(Rect rect);
}

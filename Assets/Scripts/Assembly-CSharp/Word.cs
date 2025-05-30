using UnityEngine;

public interface Word : Trackable
{
	string StringValue { get; }

	Vector2 Size { get; }

	Image GetLetterMask();

	RectangleData[] GetLetterBoundingBoxes();
}

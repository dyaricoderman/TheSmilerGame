using UnityEngine;

public struct OrientedBoundingBox
{
	public Vector2 Center { get; private set; }

	public Vector2 HalfExtents { get; private set; }

	public float Rotation { get; private set; }

	public OrientedBoundingBox(Vector2 center, Vector2 halfExtents, float rotation)
	{
		Center = center;
		HalfExtents = halfExtents;
		Rotation = rotation;
	}
}

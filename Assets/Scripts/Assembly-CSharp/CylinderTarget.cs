public interface CylinderTarget : Trackable
{
	float GetSideLength();

	float GetTopDiameter();

	float GetBottomDiameter();

	bool SetSideLength(float sideLength);

	bool SetTopDiameter(float topDiameter);

	bool SetBottomDiameter(float bottomDiameter);
}

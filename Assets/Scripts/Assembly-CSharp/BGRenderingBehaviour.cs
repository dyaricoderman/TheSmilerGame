using UnityEngine;

public class BGRenderingBehaviour : MonoBehaviour
{
	public Camera Camera;

	private QCARRenderer.VideoTextureInfo mTextureInfo;

	private ScreenOrientation mScreenOrientation;

	private int mScreenWidth;

	private int mScreenHeight;

	private bool mFlipHorizontally;

	public void CheckAndSetActive(bool isActive)
	{
		if (Camera.gameObject.active != isActive)
		{
			Camera.gameObject.SetActiveRecursively(isActive);
		}
	}

	public void SetTexture(Texture texture)
	{
		base.GetComponent<Renderer>().material.mainTexture = texture;
	}

	public void SetFlipHorizontally(bool flip)
	{
		mFlipHorizontally = flip;
	}

	private void Start()
	{
		if (Camera == null)
		{
			Camera = Camera.main;
		}
	}

	private void Update()
	{
		if (!QCARRenderer.Instance.IsVideoBackgroundInfoAvailable())
		{
			return;
		}
		QCARRenderer.VideoTextureInfo videoTextureInfo = QCARRenderer.Instance.GetVideoTextureInfo();
		if (!mTextureInfo.imageSize.Equals(videoTextureInfo.imageSize) || !mTextureInfo.textureSize.Equals(videoTextureInfo.textureSize))
		{
			mTextureInfo = videoTextureInfo;
			Debug.Log("VideoTextureInfo " + videoTextureInfo.textureSize.x + " " + videoTextureInfo.textureSize.y + " " + videoTextureInfo.imageSize.x + " " + videoTextureInfo.imageSize.y);
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			meshFilter.mesh = CreateVideoMesh();
			PositionVideoMesh();
		}
		else if (mScreenOrientation != QCARRuntimeUtilities.ScreenOrientation || mScreenWidth != Screen.width || mScreenHeight != Screen.height)
		{
			PositionVideoMesh();
		}
	}

	private Mesh CreateVideoMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4];
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				float num = (float)j / 1f - 0.5f;
				float num2 = 1f - (float)i / 1f - 0.5f;
				vertices[i * 2 + j].x = num * 2f;
				vertices[i * 2 + j].y = 0f;
				vertices[i * 2 + j].z = num2 * 2f;
			}
		}
		mesh.vertices = vertices;
		mesh.triangles = new int[24];
		int num3 = 0;
		float num4 = (float)mTextureInfo.imageSize.x / (float)mTextureInfo.textureSize.x;
		float num5 = (float)mTextureInfo.imageSize.y / (float)mTextureInfo.textureSize.y;
		mesh.uv = new Vector2[4];
		int[] triangles = mesh.triangles;
		Vector2[] uv = mesh.uv;
		for (int k = 0; k < 1; k++)
		{
			for (int l = 0; l < 1; l++)
			{
				int num6 = k * 2 + l;
				int num7 = k * 2 + l + 2 + 1;
				int num8 = k * 2 + l + 2;
				int num9 = k * 2 + l + 1;
				triangles[num3++] = num6;
				triangles[num3++] = num7;
				triangles[num3++] = num8;
				triangles[num3++] = num7;
				triangles[num3++] = num6;
				triangles[num3++] = num9;
				uv[num6] = new Vector2((float)l / 1f * num4, (float)k / 1f * num5);
				uv[num7] = new Vector2((float)(l + 1) / 1f * num4, (float)(k + 1) / 1f * num5);
				uv[num8] = new Vector2((float)l / 1f * num4, (float)(k + 1) / 1f * num5);
				uv[num9] = new Vector2((float)(l + 1) / 1f * num4, (float)k / 1f * num5);
				if (mFlipHorizontally)
				{
					uv[num6].x = 1f - uv[num6].x;
					uv[num7].x = 1f - uv[num7].x;
					uv[num8].x = 1f - uv[num8].x;
					uv[num9].x = 1f - uv[num9].x;
				}
			}
		}
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.normals = new Vector3[mesh.vertices.Length];
		mesh.RecalculateNormals();
		return mesh;
	}

	private void PositionVideoMesh()
	{
		mScreenOrientation = QCARRuntimeUtilities.ScreenOrientation;
		mScreenWidth = Screen.width;
		mScreenHeight = Screen.height;
		base.gameObject.transform.localRotation = Quaternion.AngleAxis(270f, Vector3.right);
		if (mScreenOrientation == ScreenOrientation.LandscapeLeft)
		{
			base.gameObject.transform.localRotation *= Quaternion.identity;
		}
		else if (mScreenOrientation == ScreenOrientation.Portrait)
		{
			base.gameObject.transform.localRotation *= Quaternion.AngleAxis(90f, Vector3.up);
		}
		else if (mScreenOrientation == ScreenOrientation.LandscapeRight)
		{
			base.gameObject.transform.localRotation *= Quaternion.AngleAxis(180f, Vector3.up);
		}
		else if (mScreenOrientation == ScreenOrientation.PortraitUpsideDown)
		{
			base.gameObject.transform.localRotation *= Quaternion.AngleAxis(270f, Vector3.up);
		}
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f * (float)mTextureInfo.imageSize.y / (float)mTextureInfo.imageSize.x);
		Camera.orthographic = true;
		float orthographicSize = (ShouldFitWidth() ? ((!QCARRuntimeUtilities.IsPortraitOrientation) ? ((float)mScreenHeight / (float)mScreenWidth) : ((float)mTextureInfo.imageSize.y / (float)mTextureInfo.imageSize.x * ((float)mScreenHeight / (float)mScreenWidth))) : ((!QCARRuntimeUtilities.IsPortraitOrientation) ? ((float)mTextureInfo.imageSize.y / (float)mTextureInfo.imageSize.x) : 1f));
		Camera.orthographicSize = orthographicSize;
	}

	private bool ShouldFitWidth()
	{
		float num = (float)mScreenWidth / (float)mScreenHeight;
		float num2 = ((!QCARRuntimeUtilities.IsPortraitOrientation) ? ((float)mTextureInfo.imageSize.x / (float)mTextureInfo.imageSize.y) : ((float)mTextureInfo.imageSize.y / (float)mTextureInfo.imageSize.x));
		return num >= num2;
	}
}

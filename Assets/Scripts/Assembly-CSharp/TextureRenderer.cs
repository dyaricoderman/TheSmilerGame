using UnityEngine;

public class TextureRenderer
{
	private Camera mTextureBufferCamera;

	private int mTextureWidth;

	private int mTextureHeight;

	public int Width
	{
		get
		{
			return mTextureWidth;
		}
	}

	public int Height
	{
		get
		{
			return mTextureHeight;
		}
	}

	public TextureRenderer(Texture textureToRender, int renderTextureLayer, QCARRenderer.Vec2I requestedTextureSize)
	{
		if (renderTextureLayer > 31)
		{
			Debug.LogError("WebCamBehaviour.SetupTextureBufferCamera: configured layer > 31 is not supported by Unity!");
			return;
		}
		mTextureWidth = requestedTextureSize.x;
		mTextureHeight = requestedTextureSize.y;
		float num = (float)mTextureHeight / (float)mTextureWidth * 0.5f;
		GameObject gameObject = new GameObject("TextureBufferCamera");
		mTextureBufferCamera = gameObject.AddComponent<Camera>();
		mTextureBufferCamera.orthographic = true;
		mTextureBufferCamera.orthographicSize = num;
		mTextureBufferCamera.aspect = (float)mTextureWidth / (float)mTextureHeight;
		mTextureBufferCamera.nearClipPlane = 0.5f;
		mTextureBufferCamera.farClipPlane = 1.5f;
		mTextureBufferCamera.cullingMask = 1 << renderTextureLayer;
		mTextureBufferCamera.enabled = false;
		if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepARCameraAlive)
		{
			Object.DontDestroyOnLoad(gameObject);
		}
		GameObject gameObject2 = new GameObject("TextureBufferMesh", typeof(MeshFilter), typeof(MeshRenderer));
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.layer = renderTextureLayer;
		Mesh mesh = new Mesh
		{
			vertices = new Vector3[4]
			{
				new Vector3(-0.5f, num, 1f),
				new Vector3(0.5f, num, 1f),
				new Vector3(-0.5f, 0f - num, 1f),
				new Vector3(0.5f, 0f - num, 1f)
			},
			uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			},
			triangles = new int[6] { 0, 1, 2, 2, 1, 3 }
		};
		MeshRenderer component = gameObject2.GetComponent<MeshRenderer>();
		component.material = new Material(Shader.Find("Unlit/Texture"));
		component.material.mainTexture = textureToRender;
		MeshFilter component2 = gameObject2.GetComponent<MeshFilter>();
		component2.mesh = mesh;
	}

	public RenderTexture Render()
	{
		RenderTexture temporary = RenderTexture.GetTemporary(mTextureWidth, mTextureHeight);
		mTextureBufferCamera.targetTexture = temporary;
		mTextureBufferCamera.Render();
		return temporary;
	}

	public void Destroy()
	{
		if (mTextureBufferCamera != null)
		{
			Object.Destroy(mTextureBufferCamera.gameObject);
		}
	}
}

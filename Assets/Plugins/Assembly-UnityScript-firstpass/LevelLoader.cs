using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class LevelLoader : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024LoadLevel_002497 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal int _0024level_002498;

			internal LevelLoader _0024self__002499;

			public _0024(int level, LevelLoader self_)
			{
				_0024level_002498 = level;
				_0024self__002499 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__002499.enabled = true;
					AudioListener.volume = 0f;
					result = (YieldDefault(2) ? 1 : 0);
					break;
				case 2:
					Application.LoadLevel(_0024level_002498);
					result = (YieldDefault(3) ? 1 : 0);
					break;
				case 3:
					AudioListener.volume = 1f;
					_0024self__002499.enabled = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal int _0024level_0024100;

		internal LevelLoader _0024self__0024101;

		public _0024LoadLevel_002497(int level, LevelLoader self_)
		{
			_0024level_0024100 = level;
			_0024self__0024101 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024level_0024100, _0024self__0024101);
		}
	}

	public Texture2D MainLogo;

	public Texture2D Background;

	public Texture2D Loading;

	[NonSerialized]
	public static LevelLoader inst;

	private GUISkin skin;

	public virtual void Awake()
	{
		enabled = false;
		if ((bool)inst)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		inst = this;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
	}

	public virtual void Start()
	{
		skin = FontResize.ResizedSkin;
	}

	public virtual void LoadLevelC(int level)
	{
		MonoBehaviour.print(123);
		StartCoroutine(LoadLevel(level));
	}

	public virtual IEnumerator LoadLevel(int level)
	{
		return new _0024LoadLevel_002497(level, this).GetEnumerator();
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void OnGUI()
	{
		GUI.skin = skin;
		GUI.depth = -50;
		GUI.DrawTexture(fluidRect(0f, 0f, 1f, 1f), Background, ScaleMode.StretchToFill, true);
		GUI.DrawTexture(Global.AspectRect(-0.1f, -0.1f, 0.75f, 0.75f, PositionType.TopRight), MainLogo, ScaleMode.StretchToFill, true);
		GUI.DrawTexture(Global.AspectRect(0f, 0f, 0.7f, 0.4f, PositionType.BottemLeft), Loading, ScaleMode.StretchToFill, true);
	}

	public virtual void Main()
	{
	}
}

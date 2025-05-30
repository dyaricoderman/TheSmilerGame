using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class SharingControl : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024FacebookSharePost_0024274 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024shareString_0024275;

			internal string _0024facebookAppLink_0024276;

			internal string _0024linkName_0024277;

			internal string _0024iconURL_0024278;

			internal string _0024iconCaption_0024279;

			internal SharingControl _0024self__0024280;

			public _0024(string shareString, string facebookAppLink, string linkName, string iconURL, string iconCaption, SharingControl self_)
			{
				_0024shareString_0024275 = shareString;
				_0024facebookAppLink_0024276 = facebookAppLink;
				_0024linkName_0024277 = linkName;
				_0024iconURL_0024278 = iconURL;
				_0024iconCaption_0024279 = iconCaption;
				_0024self__0024280 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024280.loggingIntoSocialNetwork = false;
					result = (Yield(2, new WaitForSeconds(0.1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024280.loggingIntoSocialNetwork = true;
					_0024self__0024280.StartCoroutine(_0024self__0024280.FacebookPost(_0024shareString_0024275, _0024facebookAppLink_0024276, _0024linkName_0024277, _0024iconURL_0024278, _0024iconCaption_0024279));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024shareString_0024281;

		internal string _0024facebookAppLink_0024282;

		internal string _0024linkName_0024283;

		internal string _0024iconURL_0024284;

		internal string _0024iconCaption_0024285;

		internal SharingControl _0024self__0024286;

		public _0024FacebookSharePost_0024274(string shareString, string facebookAppLink, string linkName, string iconURL, string iconCaption, SharingControl self_)
		{
			_0024shareString_0024281 = shareString;
			_0024facebookAppLink_0024282 = facebookAppLink;
			_0024linkName_0024283 = linkName;
			_0024iconURL_0024284 = iconURL;
			_0024iconCaption_0024285 = iconCaption;
			_0024self__0024286 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024shareString_0024281, _0024facebookAppLink_0024282, _0024linkName_0024283, _0024iconURL_0024284, _0024iconCaption_0024285, _0024self__0024286);
		}
	}

	[Serializable]
	internal sealed class _0024FacebookShareImage_0024287 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Texture2D _0024shareImage_0024288;

			internal string _0024shareString_0024289;

			internal SharingControl _0024self__0024290;

			public _0024(Texture2D shareImage, string shareString, SharingControl self_)
			{
				_0024shareImage_0024288 = shareImage;
				_0024shareString_0024289 = shareString;
				_0024self__0024290 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024290.loggingIntoSocialNetwork = false;
					result = (Yield(2, new WaitForSeconds(0.1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024290.loggingIntoSocialNetwork = true;
					_0024self__0024290.StartCoroutine(_0024self__0024290.FacebookPostImage(_0024shareImage_0024288.EncodeToPNG(), _0024shareString_0024289));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Texture2D _0024shareImage_0024291;

		internal string _0024shareString_0024292;

		internal SharingControl _0024self__0024293;

		public _0024FacebookShareImage_0024287(Texture2D shareImage, string shareString, SharingControl self_)
		{
			_0024shareImage_0024291 = shareImage;
			_0024shareString_0024292 = shareString;
			_0024self__0024293 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024shareImage_0024291, _0024shareString_0024292, _0024self__0024293);
		}
	}

	[Serializable]
	internal sealed class _0024FacebookPost_0024294 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024shareString_0024295;

			internal string _0024facebookAppLink_0024296;

			internal string _0024linkName_0024297;

			internal string _0024imageURL_0024298;

			internal string _0024imageCaption_0024299;

			internal SharingControl _0024self__0024300;

			public _0024(string shareString, string facebookAppLink, string linkName, string imageURL, string imageCaption, SharingControl self_)
			{
				_0024shareString_0024295 = shareString;
				_0024facebookAppLink_0024296 = facebookAppLink;
				_0024linkName_0024297 = linkName;
				_0024imageURL_0024298 = imageURL;
				_0024imageCaption_0024299 = imageCaption;
				_0024self__0024300 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024300.TryFacebookLogin();
					if (!FacebookAndroid.isSessionValid())
					{
						result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
						break;
					}
					goto case 2;
				case 2:
					_0024self__0024300.loggingIntoSocialNetwork = true;
					goto case 3;
				case 3:
					if (!FacebookAndroid.isSessionValid() && _0024self__0024300.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					if (FacebookAndroid.isSessionValid())
					{
						if (FacebookAndroid.getSessionPermissions().Count == 0)
						{
							FacebookAndroid.reauthorizeWithPublishPermissions(_0024self__0024300.GetFacebookPermissions("publish"), FacebookSessionDefaultAudience.EVERYONE);
						}
						goto case 4;
					}
					goto IL_013e;
				case 4:
					if (FacebookAndroid.getSessionPermissions().Count == 0 && _0024self__0024300.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(4) ? 1 : 0);
						break;
					}
					if (_0024self__0024300.loggingIntoSocialNetwork)
					{
						_0024self__0024300.loggingIntoSocialNetwork = false;
						Facebook.instance.postMessageWithLinkAndLinkToImage(_0024shareString_0024295, _0024facebookAppLink_0024296, _0024linkName_0024297, _0024imageURL_0024298, _0024imageCaption_0024299, _0024self__0024300.completionHandler);
						_0024self__0024300.SocialNetworkPostComplete();
					}
					goto IL_013e;
				case 1:
					{
						result = 0;
						break;
					}
					IL_013e:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024shareString_0024301;

		internal string _0024facebookAppLink_0024302;

		internal string _0024linkName_0024303;

		internal string _0024imageURL_0024304;

		internal string _0024imageCaption_0024305;

		internal SharingControl _0024self__0024306;

		public _0024FacebookPost_0024294(string shareString, string facebookAppLink, string linkName, string imageURL, string imageCaption, SharingControl self_)
		{
			_0024shareString_0024301 = shareString;
			_0024facebookAppLink_0024302 = facebookAppLink;
			_0024linkName_0024303 = linkName;
			_0024imageURL_0024304 = imageURL;
			_0024imageCaption_0024305 = imageCaption;
			_0024self__0024306 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024shareString_0024301, _0024facebookAppLink_0024302, _0024linkName_0024303, _0024imageURL_0024304, _0024imageCaption_0024305, _0024self__0024306);
		}
	}

	[Serializable]
	internal sealed class _0024FacebookPostImage_0024307 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal object _0024bytes_0024308;

			internal string _0024postDescription_0024309;

			internal SharingControl _0024self__0024310;

			public _0024(object bytes, string postDescription, SharingControl self_)
			{
				_0024bytes_0024308 = bytes;
				_0024postDescription_0024309 = postDescription;
				_0024self__0024310 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024310.TryFacebookLogin();
					if (!FacebookAndroid.isSessionValid())
					{
						result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
						break;
					}
					goto case 2;
				case 2:
					_0024self__0024310.loggingIntoSocialNetwork = true;
					goto case 3;
				case 3:
					if (!FacebookAndroid.isSessionValid() && _0024self__0024310.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					if (FacebookAndroid.isSessionValid())
					{
						if (FacebookAndroid.getSessionPermissions().Count == 0)
						{
							FacebookAndroid.reauthorizeWithPublishPermissions(_0024self__0024310.GetFacebookPermissions("publish"), FacebookSessionDefaultAudience.EVERYONE);
						}
						goto case 4;
					}
					goto IL_014b;
				case 4:
					if (FacebookAndroid.getSessionPermissions().Count == 0 && _0024self__0024310.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(4) ? 1 : 0);
						break;
					}
					if (_0024self__0024310.loggingIntoSocialNetwork)
					{
						_0024self__0024310.loggingIntoSocialNetwork = false;
						Facebook instance = Facebook.instance;
						object obj = _0024bytes_0024308;
						if (!(obj is byte[]))
						{
							obj = RuntimeServices.Coerce(obj, typeof(byte[]));
						}
						instance.postImage((byte[])obj, _0024postDescription_0024309, _0024self__0024310.completionHandler);
						_0024self__0024310.SocialNetworkPostComplete();
					}
					goto IL_014b;
				case 1:
					{
						result = 0;
						break;
					}
					IL_014b:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal object _0024bytes_0024311;

		internal string _0024postDescription_0024312;

		internal SharingControl _0024self__0024313;

		public _0024FacebookPostImage_0024307(object bytes, string postDescription, SharingControl self_)
		{
			_0024bytes_0024311 = bytes;
			_0024postDescription_0024312 = postDescription;
			_0024self__0024313 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024bytes_0024311, _0024postDescription_0024312, _0024self__0024313);
		}
	}

	[Serializable]
	internal sealed class _0024TwitterSharePost_0024314 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024shareString_0024315;

			internal SharingControl _0024self__0024316;

			public _0024(string shareString, SharingControl self_)
			{
				_0024shareString_0024315 = shareString;
				_0024self__0024316 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024316.loggingIntoSocialNetwork = false;
					result = (Yield(2, new WaitForSeconds(0.1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024316.loggingIntoSocialNetwork = true;
					_0024self__0024316.StartCoroutine(_0024self__0024316.TwitterPost(_0024shareString_0024315));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024shareString_0024317;

		internal SharingControl _0024self__0024318;

		public _0024TwitterSharePost_0024314(string shareString, SharingControl self_)
		{
			_0024shareString_0024317 = shareString;
			_0024self__0024318 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024shareString_0024317, _0024self__0024318);
		}
	}

	[Serializable]
	internal sealed class _0024TwitterShareImage_0024319 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024shareString_0024320;

			internal string _0024imageLocation_0024321;

			internal Texture2D _0024shareImage_0024322;

			internal SharingControl _0024self__0024323;

			public _0024(string shareString, string imageLocation, Texture2D shareImage, SharingControl self_)
			{
				_0024shareString_0024320 = shareString;
				_0024imageLocation_0024321 = imageLocation;
				_0024shareImage_0024322 = shareImage;
				_0024self__0024323 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024323.loggingIntoSocialNetwork = false;
					result = (Yield(2, new WaitForSeconds(0.1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024323.loggingIntoSocialNetwork = true;
					_0024self__0024323.StartCoroutine(_0024self__0024323.TwitterImage(_0024shareString_0024320, _0024imageLocation_0024321, _0024shareImage_0024322.EncodeToPNG()));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024shareString_0024324;

		internal string _0024imageLocation_0024325;

		internal Texture2D _0024shareImage_0024326;

		internal SharingControl _0024self__0024327;

		public _0024TwitterShareImage_0024319(string shareString, string imageLocation, Texture2D shareImage, SharingControl self_)
		{
			_0024shareString_0024324 = shareString;
			_0024imageLocation_0024325 = imageLocation;
			_0024shareImage_0024326 = shareImage;
			_0024self__0024327 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024shareString_0024324, _0024imageLocation_0024325, _0024shareImage_0024326, _0024self__0024327);
		}
	}

	[Serializable]
	internal sealed class _0024TwitterPost_0024328 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024postDescription_0024329;

			internal SharingControl _0024self__0024330;

			public _0024(string postDescription, SharingControl self_)
			{
				_0024postDescription_0024329 = postDescription;
				_0024self__0024330 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024330.TryTwitterLogin();
					if (!TwitterAndroid.isLoggedIn())
					{
						result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
						break;
					}
					goto case 2;
				case 2:
				case 3:
					if (!TwitterAndroid.isLoggedIn() && _0024self__0024330.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					if (TwitterAndroid.isLoggedIn())
					{
						TwitterAndroid.postUpdate(_0024postDescription_0024329);
						_0024self__0024330.SocialNetworkPostComplete();
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024postDescription_0024331;

		internal SharingControl _0024self__0024332;

		public _0024TwitterPost_0024328(string postDescription, SharingControl self_)
		{
			_0024postDescription_0024331 = postDescription;
			_0024self__0024332 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024postDescription_0024331, _0024self__0024332);
		}
	}

	[Serializable]
	internal sealed class _0024TwitterImage_0024333 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024postDescription_0024334;

			internal object _0024bytes_0024335;

			internal SharingControl _0024self__0024336;

			public _0024(string postDescription, object bytes, SharingControl self_)
			{
				_0024postDescription_0024334 = postDescription;
				_0024bytes_0024335 = bytes;
				_0024self__0024336 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024336.TryTwitterLogin();
					if (!TwitterAndroid.isLoggedIn())
					{
						result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
						break;
					}
					goto case 2;
				case 2:
				case 3:
				{
					if (!TwitterAndroid.isLoggedIn() && _0024self__0024336.loggingIntoSocialNetwork)
					{
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					string update = _0024postDescription_0024334;
					object obj = _0024bytes_0024335;
					if (!(obj is byte[]))
					{
						obj = RuntimeServices.Coerce(obj, typeof(byte[]));
					}
					TwitterAndroid.postUpdateWithImage(update, (byte[])obj);
					_0024self__0024336.SocialNetworkPostComplete();
					YieldDefault(1);
					goto case 1;
				}
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024postDescription_0024337;

		internal object _0024bytes_0024338;

		internal SharingControl _0024self__0024339;

		public _0024TwitterImage_0024333(string postDescription, object bytes, SharingControl self_)
		{
			_0024postDescription_0024337 = postDescription;
			_0024bytes_0024338 = bytes;
			_0024self__0024339 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024postDescription_0024337, _0024bytes_0024338, _0024self__0024339);
		}
	}

	[Serializable]
	internal sealed class _0024ReinitialiseSocialNetworking_0024340 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal SharingControl _0024self__0024341;

			public _0024(SharingControl self_)
			{
				_0024self__0024341 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024341.InitialiseSocialNetworking();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal SharingControl _0024self__0024342;

		public _0024ReinitialiseSocialNetworking_0024340(SharingControl self_)
		{
			_0024self__0024342 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024342);
		}
	}

	[NonSerialized]
	public static SharingControl inst;

	public string twitterKey;

	public string twitterSecret;

	public MonoBehaviour postCompleteHandler;

	public string postCompleteMessage;

	public string loginFailedMessage;

	private float androidLoginTimeout;

	private bool loggingIntoSocialNetwork;

	public SharingControl()
	{
		loginFailedMessage = "LoginFailed";
		androidLoginTimeout = 10f;
	}

	public virtual void Awake()
	{
		inst = this;
	}

	public virtual void Start()
	{
		InitialiseSocialNetworking();
	}

	private void InitialiseSocialNetworking()
	{
		FacebookAndroid.init();
		TwitterAndroid.init(twitterKey, twitterSecret);
	}

	public virtual void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
	}

	private void TryFacebookLogin()
	{
		bool flag = default(bool);
		if (!FacebookAndroid.isSessionValid())
		{
			FacebookAndroid.login();
		}
	}

	private string[] GetFacebookPermissions(string permissionType)
	{
		string[] result = null;
		if (permissionType == "read")
		{
			result = new string[0];
		}
		if (permissionType == "publish")
		{
			result = new string[1] { "publish_actions" };
		}
		return result;
	}

	public virtual IEnumerator FacebookSharePost(string shareString, string facebookAppLink, string linkName, string iconURL, string iconCaption)
	{
		return new _0024FacebookSharePost_0024274(shareString, facebookAppLink, linkName, iconURL, iconCaption, this).GetEnumerator();
	}

	public virtual IEnumerator FacebookShareImage(Texture2D shareImage, string shareString)
	{
		return new _0024FacebookShareImage_0024287(shareImage, shareString, this).GetEnumerator();
	}

	private IEnumerator FacebookPost(string shareString, string facebookAppLink, string linkName, string imageURL, string imageCaption)
	{
		return new _0024FacebookPost_0024294(shareString, facebookAppLink, linkName, imageURL, imageCaption, this).GetEnumerator();
	}

	private IEnumerator FacebookPostImage(object bytes, string postDescription)
	{
		return new _0024FacebookPostImage_0024307(bytes, postDescription, this).GetEnumerator();
	}

	private void TryTwitterLogin()
	{
		if (!TwitterAndroid.isLoggedIn())
		{
			TwitterAndroid.showLoginDialog();
		}
	}

	public virtual IEnumerator TwitterSharePost(string shareString)
	{
		return new _0024TwitterSharePost_0024314(shareString, this).GetEnumerator();
	}

	public virtual IEnumerator TwitterShareImage(string shareString, string imageLocation, Texture2D shareImage)
	{
		return new _0024TwitterShareImage_0024319(shareString, imageLocation, shareImage, this).GetEnumerator();
	}

	public virtual IEnumerator TwitterPost(string postDescription)
	{
		return new _0024TwitterPost_0024328(postDescription, this).GetEnumerator();
	}

	public virtual IEnumerator TwitterImage(string postDescription, string imageLocation, object bytes)
	{
		return new _0024TwitterImage_0024333(postDescription, bytes, this).GetEnumerator();
	}

	public virtual void SocialNetworkPostComplete()
	{
		if ((bool)postCompleteHandler)
		{
			postCompleteHandler.Invoke(postCompleteMessage, 0f);
		}
	}

	public virtual void SocialNetworksLogout()
	{
		if (FacebookAndroid.isSessionValid())
		{
			FacebookAndroid.logout();
		}
		if (TwitterAndroid.isLoggedIn())
		{
			TwitterAndroid.logout();
		}
		StartCoroutine(ReinitialiseSocialNetworking());
	}

	private IEnumerator ReinitialiseSocialNetworking()
	{
		return new _0024ReinitialiseSocialNetworking_0024340(this).GetEnumerator();
	}

	public virtual void SocialNetworkLoginFailed()
	{
		loggingIntoSocialNetwork = false;
		if ((bool)postCompleteHandler)
		{
			postCompleteHandler.Invoke(loginFailedMessage, 0f);
		}
	}

	public virtual void Main()
	{
	}
}

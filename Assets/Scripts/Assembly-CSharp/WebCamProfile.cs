using System.Collections.Generic;

public class WebCamProfile
{
	public struct ProfileData
	{
		public QCARRenderer.Vec2I RequestedTextureSize;

		public QCARRenderer.Vec2I ResampledTextureSize;

		public int RequestedFPS;
	}

	private ProfileData mDefaultProfile = default(ProfileData);

	private readonly Dictionary<string, ProfileData> mProfiles = new Dictionary<string, ProfileData>();

	public ProfileData Default
	{
		get
		{
			return mDefaultProfile;
		}
	}

	public WebCamProfile()
	{
		LoadAndParseProfiles();
	}

	public ProfileData GetProfile(string webcamName)
	{
		ProfileData value;
		if (mProfiles.TryGetValue(webcamName.ToLower(), out value))
		{
			return value;
		}
		return mDefaultProfile;
	}

	public bool ProfileAvailable(string webcamName)
	{
		return mProfiles.ContainsKey(webcamName.ToLower());
	}

	private void LoadAndParseProfiles()
	{
	}
}

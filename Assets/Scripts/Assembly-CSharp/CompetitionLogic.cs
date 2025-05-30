using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class CompetitionLogic : MonoBehaviour
{
	private const string phoneKey = "phone";

	private const string prizesKey = "prizes";

	private const string optinKey = "optin";

	private const string acceptedRulesKey = "rules";

	private const string spinAmountKey = "spinAmount";

	private const string spinsTodayKey = "spinsToday";

	private const string remainingAvailiableSpinsKey = "remainingAvailiableSpins";

	private const string dateKey = "date";

	private SpinWheel spinner;

	private Competition competition;

	private TextMesh counter;

	public Texture[] prizeIcons;

	public string competitionURL = "http://localhost:50191/Competition/";

	public string nameKey = "name";

	public string emailKey = "email";

	public string email = string.Empty;

	public new string name = string.Empty;

	public string phone = string.Empty;

	public bool optin;

	public bool canSpin;

	public int maxDailySpins = 10;

	public int spinAmount;

	public int spinsToday;

	public int remainingAvailiableSpins = 10;

	public int numCredits;

	public int spinCost = 10;

	public bool contactingServer;

	private WWW www;

	public string wormdo = "j9gf8573ujtk39f48t";

	public string mandont = "289h37ygj945438tujfk389h";

	public bool cancelled;

	public bool showAsyncCancelBtn;

	private float stopConnectionTimeout = 1.5f;

	private string prizesJson { get; set; }

	public List<Winner> wins { get; set; }

	private void Awake()
	{
		spinner = GameObject.Find("SpinWheel").GetComponent<SpinWheel>();
		counter = GameObject.Find("Counter").GetComponent<TextMesh>();
		competition = base.gameObject.GetComponent<Competition>();
	}

	public void Start()
	{
		ReadPrizes();
		spinAmount = PlayerPrefs.GetInt("spinAmount", 0);
		remainingAvailiableSpins = PlayerPrefs.GetInt("remainingAvailiableSpins", 10);
		PlayerPrefs.SetInt("spinAmount", spinAmount);
		PlayerPrefs.SetInt("remainingAvailiableSpins", remainingAvailiableSpins);
		if (PlayerPrefs.HasKey("date"))
		{
			if (IsNewDay())
			{
				spinsToday = 0;
				if (spinAmount < maxDailySpins)
				{
					AddSpins(1);
					CheckWheelLock();
				}
				remainingAvailiableSpins = maxDailySpins - spinAmount;
				PlayerPrefs.SetInt("remainingAvailiableSpins", remainingAvailiableSpins);
				PlayerPrefs.SetInt("spinsToday", spinsToday);
				PlayerPrefs.SetString("date", DateTime.Today.ToString("dd") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy"));
			}
		}
		else
		{
			PlayerPrefs.SetString("date", DateTime.Today.ToString("dd") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy"));
			if (remainingAvailiableSpins > 0)
			{
				AddSpins(1);
				CheckWheelLock();
			}
		}
		CheckWheelLock();
		counter.text = spinAmount.ToString();
		if (HasAcceptedRules())
		{
			if (PlayerPrefs.HasKey(emailKey))
			{
				GetUserDetails();
				return;
			}
			competition.StartCoroutine(competition.LoadDetailsScreen(0.5f));
			canSpin = false;
		}
		else
		{
			competition.StartCoroutine(competition.LoadRulesScreen(0.5f));
			canSpin = false;
		}
	}

	private bool IsNewDay()
	{
		string text = DateTime.Today.ToString("dd") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy");
		if (text == PlayerPrefs.GetString("date"))
		{
			return false;
		}
		return true;
	}

	public void AcceptRules(bool state)
	{
		PlayerPrefsX.SetBool("rules", state);
		if (state)
		{
			if (PlayerPrefs.HasKey(emailKey))
			{
				GetUserDetails();
				return;
			}
			competition.StartCoroutine(competition.LoadDetailsScreen(0.5f));
			canSpin = false;
		}
	}

	public bool HasAcceptedRules()
	{
		return PlayerPrefsX.GetBool("rules");
	}

	public void BuySpins(int numToAdd)
	{
		TrackUpgrade.SpendCredits(numToAdd * spinCost);
		AddSpins(numToAdd);
		CheckWheelLock();
	}

	public void AddSpins(int numToAdd)
	{
		spinAmount += numToAdd;
		counter.text = spinAmount.ToString();
		if (numToAdd > 0)
		{
			remainingAvailiableSpins -= numToAdd;
		}
		else
		{
			spinsToday++;
		}
		PlayerPrefs.SetInt("spinAmount", spinAmount);
		PlayerPrefs.SetInt("remainingAvailiableSpins", remainingAvailiableSpins);
		PlayerPrefs.SetInt("spinsToday", spinsToday);
	}

	public void CheckWheelLock()
	{
		if (spinAmount > 0)
		{
			if (!canSpin)
			{
				spinner.Unlock();
				canSpin = true;
			}
			else
			{
				spinner.playerControl = true;
			}
		}
		else if (canSpin)
		{
			spinner.Lock();
			canSpin = false;
		}
		else
		{
			spinner.playerControl = false;
		}
		SetSpinnerErrorMessage();
	}

	public void SetSpinnerErrorMessage()
	{
		competition.spinnerErrorContent.text = string.Empty;
		if (spinAmount < 1)
		{
			competition.spinnerErrorContent.text = "You need more spins!";
			if (remainingAvailiableSpins < 1)
			{
				competition.spinnerErrorContent.text = "You are out of spins, come back tomorrow!";
			}
		}
	}

	public string GetDetailMessage(Winner prize)
	{
		MonoBehaviour.print(prize.Address);
		if (prize.Address.Addressee == null)
		{
			return "To redeem your prize please check your email ";
		}
		if (!prize.Dispatched.HasValue)
		{
			return "Prize awaiting dispatch";
		}
		return "Prize dispatched on " + prize.Dispatched.Value.Date.ToString("d");
	}

	public IEnumerator Spin()
	{
		GameObject.Find("Tracking").SendMessage("SentFromCLog", "Wheel spun");
		contactingServer = true;
		competition.ShowAsync();
		JsonData postModel = new JsonData();
		postModel["uuid"] = SystemInfo.deviceUniqueIdentifier;
		postModel["email"] = email;
		postModel["name"] = name;
		postModel["phone"] = phone;
		postModel["optin"] = (optin ? 1 : 0);
		WWWForm form = new WWWForm();
		form.AddField("e", Encryption.EncryptString(postModel.ToJson(), wormdo, mandont));
		www = new WWW(competitionURL + "Spin", form);
		Debug.Log("Spin!");
		yield return www;
		if (!cancelled)
		{
			if (www.error == null)
			{
				string data = www.text;
				data = Encryption.DecryptString(data, wormdo, mandont);
				Debug.Log("Spin: " + data);
				contactingServer = false;
				JsonData json = JsonMapper.ToObject(data);
				if ((bool)json["win"])
				{
					Prize prize = new Prize(json["prize"]);
					GameObject.Find("Tracking").SendMessage("SentFromCLog", "Won prize:" + prize.PrizeDescription.Name);
					Debug.Log(prize.PrizeDescription.Name);
					AchievementManager.SetProgressToAchievement("Go for a Spin", 1f);
					AddSpins(-1);
					StartCoroutine(Sync());
					spinner.StopWheel(true, true);
					competition.win = prize;
				}
				else
				{
					GameObject.Find("Tracking").SendMessage("SentFromCLog", "Didn't win prize");
					AchievementManager.SetProgressToAchievement("Go for a Spin", 1f);
					AddSpins(-1);
					spinner.StopWheel(false, true);
				}
			}
			else
			{
				GameObject.Find("Tracking").SendMessage("SentFromCLog", "No connection for spin");
				contactingServer = false;
				spinner.StopWheel(false, false);
				competition.StartCoroutine(competition.LoadErrorScreen(0.5f));
			}
		}
		StopCoroutine("SpinTimeoutHandler");
	}

	public IEnumerator SpinTimeoutHandler()
	{
		showAsyncCancelBtn = false;
		yield return new WaitForSeconds(stopConnectionTimeout);
		showAsyncCancelBtn = true;
	}

	public void CancelSpin()
	{
		www.Dispose();
		cancelled = true;
		Debug.Log("DISPOSE SPIN");
	}

	public void SetUserDetails(string tmpName, string tmpEmail, string tmpPhone, bool tmpOptin)
	{
		name = tmpName;
		email = tmpEmail;
		phone = tmpPhone;
		optin = tmpOptin;
		if (!PlayerPrefs.HasKey(emailKey) || PlayerPrefs.GetString(emailKey) != email)
		{
		}
		PlayerPrefs.SetString(nameKey, tmpName);
		PlayerPrefs.SetString(emailKey, tmpEmail);
		PlayerPrefs.SetString("phone", tmpPhone);
		PlayerPrefsX.SetBool("optin", tmpOptin);
		StartCoroutine(Sync());
	}

	public void GetUserDetails()
	{
		name = PlayerPrefs.GetString(nameKey, string.Empty);
		email = PlayerPrefs.GetString(emailKey, string.Empty);
		phone = PlayerPrefs.GetString("phone", string.Empty);
		optin = PlayerPrefsX.GetBool("optin", false);
		StartCoroutine(Sync());
	}

	public void InitBuyScreen()
	{
		numCredits = TrackUpgrade.GetCredits();
		if (remainingAvailiableSpins < 1 || numCredits < spinCost)
		{
			competition.numSpinsToBuy = 0;
		}
		else
		{
			competition.numSpinsToBuy = 1;
		}
		SetShopMessage();
	}

	public void SetShopMessage()
	{
		competition.buySpinsErrorContent.text = string.Empty;
		MonoBehaviour.print(numCredits - spinCost * competition.numSpinsToBuy);
		if (spinCost * competition.numSpinsToBuy >= numCredits - spinCost)
		{
		}
		if (remainingAvailiableSpins <= competition.numSpinsToBuy)
		{
			competition.buySpinsErrorContent.text = "You can only buy 10 spins every day.";
		}
		else if (numCredits < spinCost)
		{
			competition.buySpinsErrorContent.text = "You need " + spinCost + " or more credits to buy extra spins. Keep playing!";
		}
	}

	public void SetPrizes(string data)
	{
		PlayerPrefs.SetString("prizes", data);
	}

	public void ReadPrizes()
	{
		wins = new List<Winner>();
		if (PlayerPrefs.HasKey("prizes"))
		{
			prizesJson = PlayerPrefs.GetString("prizes");
			JsonData jsonData = JsonMapper.ToObject(prizesJson);
			if (jsonData != null && jsonData.IsArray && jsonData.Count > 0)
			{
				foreach (JsonData item in jsonData.inst_array)
				{
					wins.Add(new Winner(item));
				}
			}
		}
		MonoBehaviour.print("CL Read Prizes: " + wins.Count);
	}

	private IEnumerator Sync()
	{
		JsonData postModel = new JsonData();
		postModel["uuid"] = SystemInfo.deviceUniqueIdentifier;
		postModel["email"] = email;
		postModel["name"] = name;
		postModel["phone"] = phone;
		postModel["optin"] = (optin ? 1 : 0);
		WWWForm form = new WWWForm();
		form.AddField("e", Encryption.EncryptString(postModel.ToJson(), wormdo, mandont));
		using (WWW www = new WWW(competitionURL + "Sync", form))
		{
			Debug.Log("Sync!");
			yield return www;
			if (www.error == null)
			{
				string data = www.text;
				data = Encryption.DecryptString(data, wormdo, mandont);
				Debug.Log("Sync: " + data);
				SetPrizes(data);
				ReadPrizes();
			}
			else
			{
				Debug.Log(www.error);
				Debug.Log(www.text);
			}
		}
	}
}

using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class Competition : MonoBehaviour
{
	public enum State
	{
		Email = 0,
		Spinner = 1,
		Async = 2,
		Win = 3,
		Lose = 4,
		Previous = 5,
		Detail = 6,
		Broken = 7,
		BuySpins = 8,
		Rules = 9,
		TC = 10,
		Prizes = 11,
		MaxSpins = 12,
		Support = 13,
		Empty = 14
	}

	private GUISkin skin;

	public bool canSpin = true;

	private SpinWheel spinner;

	private CompetitionLogic competitionLogic;

	private WinningsDisplay winnings;

	private PrizeDisplay prizes;

	public AudioSource winSound;

	public AudioSource loseSound;

	public string backBtnLevel;

	public Texture footerTex;

	public Texture buySpinsIncreaseTex;

	public Texture buySpinsDecreaseTex;

	public Texture buySpinsIncreaseGreyTex;

	public Texture buySpinsDecreaseGreyTex;

	public Texture rulesUpTex;

	public Texture rulesDownTex;

	public Texture rulesUpGreyTex;

	public Texture rulesDownGreyTex;

	public State state;

	private bool emailValidating;

	public Rect nameLabelRect = new Rect(0.2f, 0.45f, 0.2f, 0.1f);

	public GUIContent nameLabelContent = new GUIContent("Name");

	public Rect nameFieldRect = new Rect(0.35f, 0.45f, 0.3f, 0.1f);

	public GUIContent nameFieldContent = new GUIContent("Name");

	public string nameFieldText = string.Empty;

	public Rect emailLabelRect = new Rect(0.2f, 0.6f, 0.2f, 0.1f);

	public GUIContent emailLabelContent = new GUIContent("Email");

	public Rect emailFieldRect = new Rect(0.35f, 0.6f, 0.3f, 0.1f);

	public GUIContent emailFieldContent = new GUIContent("Email");

	public string emailFieldText = string.Empty;

	public Rect phoneLabelRect = new Rect(0.2f, 0.45f, 0.2f, 0.1f);

	public GUIContent phoneLabelContent = new GUIContent("phone");

	public Rect phoneFieldRect = new Rect(0.35f, 0.45f, 0.3f, 0.1f);

	public GUIContent phoneFieldContent = new GUIContent("phone");

	public string phoneFieldText = string.Empty;

	public Rect emailCancelRect = new Rect(0.3f, 0.85f, 0.15f, 0.1f);

	public GUIContent emailCancelContent = new GUIContent("Cancel");

	public Rect emailSubmitRect = new Rect(0.475f, 0.85f, 0.25f, 0.1f);

	public GUIContent emailSubmitContent = new GUIContent("Submit");

	public bool toggleStatus;

	public Rect toggleLabelRect = new Rect(0.475f, 0.85f, 0.25f, 0.1f);

	public GUIContent toggleLabelContent = new GUIContent("We would like to keep you informed about Alton Towers Resort and other Merlin Entertainments Group attractions. Please tick here if you wish to receive offers and information from the Merlin Entertainments Group.");

	public Rect toggleSubmitRect = new Rect(0.475f, 0.85f, 0.25f, 0.1f);

	public GUIContent toggleSubmitContent = new GUIContent();

	public Rect spinnerBackRect = new Rect(0.1f, 0.8f, 0.3f, 0.1f);

	public GUIContent spinnerBackContent = new GUIContent("BACK");

	public Rect spinnerPreviousRect = new Rect(0.3f, 0.7f, 0.2f, 0.1f);

	public GUIContent spinnerPreviousContent = new GUIContent("WINNINGS");

	public Rect spinnerUserRect = new Rect(0.825f, 0.025f, 0.15f, 0.2f);

	public Rect spinnerBuySpinsRect = new Rect(0.01f, 0.025f, 0.15f, 0.1f);

	public Texture spinnerBuySpinsContent;

	public Rect spinnerEmailRect = new Rect(0.825f, 0.025f, 0.15f, 0.1f);

	public GUIContent spinnerEmailContent = new GUIContent();

	public Rect spinnerTutRect = new Rect(0.075f, 0.55f, 0.325f, 0.09f);

	public GUIContent spinnerTutContent = new GUIContent("DRAG THE WHEEL AND SPIN TO WIN ONE OF 8 ALTON TOWERS PRIZES");

	public Rect spinnerDescRect = new Rect(0.075f, 0.55f, 0.325f, 0.09f);

	public GUIContent spinnerDescContent = new GUIContent("YOU COULD WIN ANYTHING FROM A NIGHT AT ALTON TOWERS RESORT HOTEL TO FREE FASTTRACK TICKETS!");

	public Rect spinnerMessageRect = new Rect(0.1f, 0.5f, 0.3f, 0.1f);

	public GUIContent spinnerMessageContent = new GUIContent("YOU WILL RECIEVE ONE FREE SPIN EVERY DAY");

	public Rect spinnerErrorRect = new Rect(0.1f, 0.5f, 0.3f, 0.1f);

	public GUIContent spinnerErrorContent = new GUIContent("ERROR MESSAGE");

	public Rect spinnerRulesRect = new Rect(0.075f, 0.45f, 0.325f, 0.09f);

	public GUIContent spinnerRulesContent = new GUIContent();

	public Rect spinnerTCRect = new Rect(0.075f, 0.55f, 0.325f, 0.09f);

	public GUIContent spinnerTCContent = new GUIContent();

	public Rect spinnerPrizesRect = new Rect(0.075f, 0.65f, 0.325f, 0.09f);

	public GUIContent spinnerPrizesContent = new GUIContent();

	public Rect asyncDescRect = new Rect(0.3f, 0.75f, 0.25f, 0.1f);

	public GUIContent asyncDescContent = new GUIContent("Waiting for server...");

	public Rect asyncCancelRect = new Rect(0.3f, 0.85f, 0.15f, 0.1f);

	public GUIContent asyncCancelContent = new GUIContent("Cancel");

	public Rect winImageRect = new Rect(0.175f, 0.4f, 0.275f, 0.35f);

	public GUIContent winImageContent = new GUIContent();

	public Rect winCodeLabelRect = new Rect(0.25f, 0.5f, 0.5f, 0.1f);

	public GUIContent winCodeLabelContent = new GUIContent("Code");

	public Rect winCodeRect = new Rect(0.25f, 0.5f, 0.5f, 0.1f);

	public GUIContent winCodeContent = new GUIContent("BBA1-CCH5");

	public Rect winNameRect = new Rect(0.25f, 0.4f, 0.5f, 0.1f);

	public GUIContent winNameContent = new GUIContent("Win Name");

	public Rect winDescriptionRect = new Rect(0.1f, 0.5f, 0.8f, 0.2f);

	public GUIContent winDescriptionContent = new GUIContent("Win Description");

	public Rect winExplainationRect = new Rect(0.1f, 0.5f, 0.8f, 0.2f);

	public GUIContent winExplainationContent = new GUIContent("Win Description");

	public Rect winContinueRect = new Rect(0.2f, 0.8f, 0.8f, 0.1f);

	public GUIContent winContinueContent = new GUIContent("Continue");

	public Rect loseParaRect = new Rect(0.1f, 0.1f, 0.8f, 0.2f);

	public GUIContent loseParaContent = new GUIContent("Lose Paragraph");

	public Rect loseTipRect = new Rect(0.1f, 0.1f, 0.8f, 0.2f);

	public GUIContent loseTipContent = new GUIContent("Hint Paragraph");

	public Rect loseContinueRect = new Rect(0.2f, 0.8f, 0.8f, 0.1f);

	public GUIContent loseContinueContent = new GUIContent("Continue");

	public Rect detailImageRect = new Rect(0.175f, 0.4f, 0.275f, 0.35f);

	public GUIContent detailImageContent = new GUIContent();

	public Rect detailMessageRect = new Rect(0.25f, 0.5f, 0.5f, 0.1f);

	public GUIContent detailMessageContent = new GUIContent("Code");

	public Rect detailCodeRect = new Rect(0.25f, 0.5f, 0.5f, 0.1f);

	public GUIContent detailCodeContent = new GUIContent("BBA1-CCH5");

	public Rect detailNameRect = new Rect(0.25f, 0.4f, 0.5f, 0.1f);

	public GUIContent detailNameContent = new GUIContent("Win Name");

	public Rect detailDescriptionRect = new Rect(0.1f, 0.5f, 0.8f, 0.2f);

	public GUIContent detailDescriptionContent = new GUIContent("Win Description");

	public Rect detailContinueRect = new Rect(0.2f, 0.8f, 0.8f, 0.1f);

	public GUIContent detailContinueContent = new GUIContent("Continue");

	public Rect brokenIconRect = new Rect(0.175f, 0.4f, 0.275f, 0.35f);

	public GUIContent brokenIconContent = new GUIContent();

	public Rect brokenParaRect = new Rect(0.4f, 0.4f, 0.4f, 0.2f);

	public GUIContent brokenParaContent = new GUIContent("There has been an error");

	public Rect brokenSolutionRect = new Rect(0.4f, 0.6f, 0.4f, 0.2f);

	public GUIContent brokenSolutionContent = new GUIContent("There has been an error");

	public Rect brokenContinueRect = new Rect(0.2f, 0.8f, 0.8f, 0.1f);

	public GUIContent brokenContinueContent = new GUIContent("Continue");

	public Rect buySpinsDescRect = new Rect(0.25f, 0.65f, 0.5f, 0.15f);

	public GUIContent buySpinsDescContent = new GUIContent("Oat cake cake. Candy canes jujubes drag??e pie chocolate jelly-o cupcake toffee.");

	public Rect buySpinsErrorRect = new Rect(0.25f, 0.8f, 0.5f, 0.15f);

	public GUIContent buySpinsErrorContent = new GUIContent(string.Empty);

	public Rect buySpinsBalanceRect = new Rect(0.25f, 0.25f, 275f, 0.275f);

	public GUIContent buySpinsBalanceContent = new GUIContent();

	public Rect buySpinsCurrentCostRect = new Rect(0.25f, 0.25f, 275f, 0.275f);

	public GUIContent buySpinsCurrentCostContent = new GUIContent();

	public Rect buySpinsNumberRect = new Rect(0.4f, 0.5f, 0.2f, 0.1f);

	public GUIContent buySpinsNumberContent = new GUIContent("0");

	public Rect buySpinsDecreaseRect = new Rect(0.25f, 0.5f, 0.075f, 0.1f);

	public Rect buySpinsIncreaseRect = new Rect(0.2f, 0.5f, 0.075f, 0.1f);

	public Rect buySpinsCancelRect = new Rect(0.3f, 0.85f, 0.15f, 0.1f);

	public GUIContent buySpinsCancelContent = new GUIContent("Cancel");

	public Rect buySpinsPurchaseRect = new Rect(0.475f, 0.85f, 0.25f, 0.1f);

	public GUIContent buySpinsPurchaseContent = new GUIContent("Buy");

	public Rect tcDescRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent tcDescContent = new GUIContent("Terms and Conditions");

	public Rect tcBtnRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent tcBtnContent = new GUIContent("Terms and Conditions");

	public Rect tcBackRect = new Rect(0.325f, 0.85f, 0.35f, 0.15f);

	public GUIContent tcBackContent = new GUIContent("Back");

	public GameObject rules;

	public float rulesScrollPos;

	public float rulesScrollSpeed;

	public Rect rulesUpBtnRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent rulesUpBtnContent = new GUIContent();

	public Rect rulesDownBtnRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent rulesDownBtnContent = new GUIContent();

	public Rect rulesContinueRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent rulesContinueContent = new GUIContent("Back");

	public Rect rulesDeclineRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent rulesDeclineContent = new GUIContent("Decline");

	public Rect rulesAcceptRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent rulesAcceptContent = new GUIContent("Terms and Conditions");

	public Rect maxSpinsIconRect = new Rect(0.175f, 0.4f, 0.275f, 0.35f);

	public GUIContent maxSpinsIconContent = new GUIContent();

	public Rect maxSpinsParaRect = new Rect(0.1f, 0.1f, 0.8f, 0.2f);

	public GUIContent maxSpinsParaContent = new GUIContent("Max Spins Paragraph");

	public Rect maxSpinsTipRect = new Rect(0.1f, 0.1f, 0.8f, 0.2f);

	public GUIContent maxSpinsTipContent = new GUIContent("Hint Paragraph");

	public Rect maxSpinsContinueRect = new Rect(0.475f, 0.85f, 0.25f, 0.1f);

	public GUIContent maxSpinsContinueContent = new GUIContent("Continue");

	public Rect supportDescRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent supportDescContent = new GUIContent("This link will take you to the Alton Towers website");

	public Rect supportBtnRect = new Rect(0.3f, 0.45f, 0.4f, 0.2f);

	public GUIContent supportBtnContent = new GUIContent("Go to Website");

	public Rect supportBackRect = new Rect(0.325f, 0.85f, 0.35f, 0.15f);

	public GUIContent supportBackContent = new GUIContent("Back");

	public Renderer[] DispearableObjects;

	public int numSpinsToBuy = 1;

	private bool WaitForTutoral;

	public Prize win { get; set; }

	private Winner detail { get; set; }

	private GameObject cbObject { get; set; }

	private string cbMethod { get; set; }

	private bool drawing { get; set; }

	public void SetDrawing(bool state)
	{
		drawing = state;
		for (int i = 0; i < DispearableObjects.Length; i++)
		{
			DispearableObjects[i].enabled = state;
		}
	}

	public void Awake()
	{
		spinner = GameObject.Find("SpinWheel").GetComponent<SpinWheel>();
		competitionLogic = base.gameObject.GetComponent<CompetitionLogic>();
		winnings = base.gameObject.GetComponent<WinningsDisplay>();
		prizes = base.gameObject.GetComponent<PrizeDisplay>();
		prizes.enabled = false;
	}

	public void Start()
	{
		SetDrawing(true);
		skin = FontResize.ResizedSkin;
		state = State.Spinner;
	}

	public void End()
	{
		SetDrawing(false);
		Application.LoadLevel(backBtnLevel);
	}

	public void ShowSpinner()
	{
		state = State.Spinner;
		competitionLogic.CheckWheelLock();
	}

	public void ShowDetail(Winner prize)
	{
		detail = prize;
		detailMessageContent.text = competitionLogic.GetDetailMessage(prize);
		StartCoroutine(LoadDetailScreen(0.5f));
	}

	public void ShowAsync()
	{
		state = State.Async;
	}

	public void ShowSupport()
	{
	}

	public void OnGUI()
	{
		GUI.skin = skin;
		if (drawing)
		{
			switch (state)
			{
			case State.Email:
				EmailGUI();
				break;
			case State.Spinner:
				SpinnerGUI();
				break;
			case State.Async:
				AsyncGUI();
				break;
			case State.Win:
				WinGUI();
				break;
			case State.Lose:
				LoseGUI();
				break;
			case State.Previous:
				PreviousGUI();
				break;
			case State.Detail:
				DetailGUI();
				break;
			case State.Broken:
				BrokenGUI();
				break;
			case State.BuySpins:
				BuySpinsGUI();
				break;
			case State.Rules:
				RulesGUI();
				break;
			case State.TC:
				TCGUI();
				break;
			case State.Prizes:
				break;
			case State.MaxSpins:
				MaxSpinsGUI();
				break;
			case State.Support:
				SupportGUI();
				break;
			case State.Empty:
				break;
			}
		}
	}

	private void EmailGUI()
	{
		GUI.Label(AspectfluidRect(nameLabelRect, 1.6f), nameLabelContent, "WhiteLabel");
		nameFieldText = GUI.TextField(AspectfluidRect(nameFieldRect, 1.6f), nameFieldText, (!Validation.IsName(nameFieldText)) ? "TextFieldError" : "TextField");
		GUI.Label(AspectfluidRect(emailLabelRect, 1.6f), emailLabelContent, "WhiteLabel");
		emailFieldText = GUI.TextField(AspectfluidRect(emailFieldRect, 1.6f), emailFieldText, (!Validation.IsEmail(emailFieldText)) ? "TextFieldError" : "TextField");
		GUI.Label(AspectfluidRect(phoneLabelRect, 1.6f), phoneLabelContent, "WhiteLabel");
		phoneFieldText = GUI.TextField(AspectfluidRect(phoneFieldRect, 1.6f), phoneFieldText, (!Validation.IsPhone(phoneFieldText)) ? "TextFieldError" : "TextField");
		GUI.DrawTexture(AspectfluidRect(toggleLabelRect, 1.6f), toggleLabelContent.image, ScaleMode.ScaleToFit);
		toggleStatus = GUI.Toggle(AspectfluidRect(toggleSubmitRect, 1.6f), toggleStatus, toggleSubmitContent, "CustomToggle");
		if (Validation.IsEmail(emailFieldText) && Validation.IsName(nameFieldText) && Validation.IsPhone(phoneFieldText))
		{
			if (GUI.Button(AspectfluidRect(emailSubmitRect, 1.6f), emailSubmitContent, "ContinueButton"))
			{
				if (Validation.IsEmail(emailFieldText) && Validation.IsName(nameFieldText) && Validation.IsPhone(phoneFieldText))
				{
					emailValidating = false;
					competitionLogic.SetUserDetails(nameFieldText, emailFieldText, Regex.Replace(phoneFieldText, "[^.0-9]", string.Empty), toggleStatus);
					StartCoroutine(HidePopup(0.2f, State.Spinner));
				}
				else
				{
					emailValidating = true;
				}
			}
		}
		else if (!GUI.Button(AspectfluidRect(emailSubmitRect, 1.6f), emailSubmitContent, "ContinueGreyButton"))
		{
		}
		if (GUI.Button(AspectfluidRect(emailCancelRect, 1.6f), emailCancelContent, "CancelButton"))
		{
			emailValidating = false;
			if (PlayerPrefs.HasKey(competitionLogic.emailKey))
			{
				GameObject.Find("Tracking").SendMessage("SentFromCLog", "Signed up for competition");
				StartCoroutine(HidePopup(0.2f, State.Spinner));
			}
			else
			{
				GameObject.Find("Tracking").SendMessage("SentFromCLog", "Didn't enter comp details");
				End();
			}
		}
		if (emailValidating && Validation.IsEmail(emailFieldText) && Validation.IsName(nameFieldText))
		{
			emailValidating = false;
		}
	}

	private void SpinnerGUI()
	{
		GUI.Label(FluidRect(spinnerUserRect), competitionLogic.email, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.color = Color.white;
		GUI.color = Color.red;
		GUI.Label(FluidRect(spinnerErrorRect), spinnerErrorContent, "WhiteLabel");
		GUI.color = Color.white;
		float num = 0.75f;
		float num2 = spinnerBuySpinsRect.width;
		if (competitionLogic.spinAmount == 0 && competitionLogic.remainingAvailiableSpins > 0)
		{
			num2 = spinnerBuySpinsRect.width * (num + Mathf.PingPong(Time.time * 2f, 1f));
		}
		Rect rect = new Rect(spinnerBuySpinsRect.x - num2 * 0.5f, spinnerBuySpinsRect.y - num2 * 0.5f, num2, num2);
		GUI.DrawTexture(FluidRect(rect), spinnerBuySpinsContent, ScaleMode.ScaleToFit);
		if (GUI.Button(FluidRect(rect), string.Empty, "InvisableButton"))
		{
			competitionLogic.InitBuyScreen();
			StartCoroutine(LoadBuySpinsScreen(0.5f));
		}
		if (GUI.Button(FluidRect(spinnerEmailRect), spinnerEmailContent))
		{
			StartCoroutine(LoadDetailsScreen(0.5f));
		}
		if (GUI.Button(FluidRect(spinnerRulesRect), spinnerRulesContent, "StandardButton"))
		{
			StartCoroutine(LoadRulesScreen(0.5f));
		}
		if (GUI.Button(FluidRect(spinnerTCRect), spinnerTCContent, "StandardButton"))
		{
			StartCoroutine(LoadTCScreen(0.5f));
		}
		if (GUI.Button(FluidRect(spinnerPrizesRect), spinnerPrizesContent, "StandardButton"))
		{
			StartCoroutine(LoadPrizesScreen(0.5f));
		}
		GUI.DrawTexture(FluidRect(0f, 0.85f, 1f, 0.15f), footerTex);
		GUI.color = Color.black;
		if (GUI.Button(FluidRect(spinnerPreviousRect), spinnerPreviousContent, "WhiteTitle"))
		{
			state = State.Previous;
			canSpin = false;
			spinner.SetPlayerControl(canSpin);
			winnings.enabled = true;
		}
		if (GUI.Button(FluidRect(spinnerBackRect), spinnerBackContent, "WhiteTitle"))
		{
			End();
		}
		GUI.color = Color.white;
	}

	private void AsyncGUI()
	{
		if (competitionLogic.contactingServer && competitionLogic.showAsyncCancelBtn)
		{
			GUI.Label(FluidRect(asyncDescRect), asyncDescContent, "WhiteLabel");
			if (GUI.Button(FluidRect(asyncCancelRect), asyncCancelContent, "StandardButton"))
			{
				competitionLogic.CancelSpin();
				state = State.Spinner;
				spinner.SetPlayerControl(true);
			}
		}
	}

	private void WinGUI()
	{
		GUI.color = Color.white;
		winImageContent.image = competitionLogic.prizeIcons[win.PrizeDescription.IconID];
		GUI.Label(AspectfluidRect(winImageRect, 1.6f), winImageContent);
		GUI.Label(AspectfluidRect(winNameRect, 1.6f), win.PrizeDescription.Name, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.color = Color.white;
		GUI.Label(AspectfluidRect(winCodeLabelRect, 1.6f), string.Empty, "TextField");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(winCodeLabelRect, 1.6f), win.Code, "WhiteLabel");
		GUI.Label(AspectfluidRect(winCodeRect, 1.6f), winCodeContent, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(winDescriptionRect, 1.6f), win.PrizeDescription.ShortDescription, "WhiteLabel");
		GUI.color = Color.white;
		GUI.Label(AspectfluidRect(winExplainationRect, 1.6f), winExplainationContent, "WhiteLabel");
		if (GUI.Button(AspectfluidRect(winContinueRect, 1.6f), winContinueContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.325f, State.Spinner));
		}
	}

	private void LoseGUI()
	{
		GUI.Label(AspectfluidRect(loseParaRect, 1.6f), loseParaContent, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(loseTipRect, 1.6f), loseTipContent, "WhiteLabel");
		GUI.color = Color.white;
		if (GUI.Button(AspectfluidRect(loseContinueRect, 1.6f), loseContinueContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.325f, State.Spinner));
		}
	}

	private void BrokenGUI()
	{
		GUI.Label(AspectfluidRect(brokenIconRect, 1.6f), brokenIconContent);
		GUI.Box(AspectfluidRect(brokenParaRect, 1.6f), brokenParaContent, "WhiteText");
		GUI.color = Color.yellow;
		GUI.Box(AspectfluidRect(brokenSolutionRect, 1.6f), brokenSolutionContent, "WhiteText");
		GUI.color = Color.white;
		if (GUI.Button(AspectfluidRect(brokenContinueRect, 1.6f), brokenContinueContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.2f, State.Spinner));
		}
	}

	private void PreviousGUI()
	{
	}

	private void DetailGUI()
	{
		GUI.color = Color.white;
		detailImageContent.image = competitionLogic.prizeIcons[detail.Prize.PrizeDescription.IconID];
		GUI.Label(AspectfluidRect(detailImageRect, 1.6f), detailImageContent);
		GUI.Label(AspectfluidRect(detailNameRect, 1.6f), detail.Prize.PrizeDescription.Name, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.color = Color.white;
		GUI.Label(AspectfluidRect(detailCodeRect, 1.6f), string.Empty, "TextField");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(detailCodeRect, 1.6f), detail.Prize.Code, "WhiteLabel");
		GUI.Label(AspectfluidRect(detailMessageRect, 1.6f), detailMessageContent, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(detailDescriptionRect, 1.6f), detail.Prize.PrizeDescription.ShortDescription, "WhiteLabel");
		GUI.color = Color.white;
		if (GUI.Button(AspectfluidRect(detailContinueRect, 1.6f), detailContinueContent, "ContinueButton"))
		{
			detail = null;
			StartCoroutine(HidePopup(0.2f, State.Previous));
			state = State.Previous;
			spinner.SetPlayerControl(false);
			winnings.enabled = true;
		}
	}

	private void MaxSpinsGUI()
	{
		GUI.Label(AspectfluidRect(maxSpinsParaRect, 1.6f), maxSpinsParaContent, "WhiteLabel");
		GUI.color = Color.yellow;
		GUI.Label(AspectfluidRect(maxSpinsTipRect, 1.6f), maxSpinsTipContent, "WhiteLabel");
		GUI.color = Color.white;
		if (GUI.Button(AspectfluidRect(maxSpinsContinueRect, 1.6f), maxSpinsContinueContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.2f, State.Spinner));
		}
		GUI.Label(AspectfluidRect(maxSpinsIconRect, 1.6f), maxSpinsIconContent);
	}

	private void BuySpinsGUI()
	{
		GUI.Box(AspectfluidRect(buySpinsDescRect, 1.6f), buySpinsDescContent, "WhiteLabel");
		GUI.color = Color.red;
		GUI.Box(AspectfluidRect(buySpinsErrorRect, 1.6f), buySpinsErrorContent, "WhiteLabel");
		GUI.color = Color.white;
		GUI.color = Color.yellow;
		if (numSpinsToBuy > 1)
		{
			if (GUI.Button(AspectfluidRect(buySpinsDecreaseRect, 1.6f), buySpinsDecreaseTex))
			{
				numSpinsToBuy--;
				competitionLogic.SetShopMessage();
			}
		}
		else if (!GUI.Button(AspectfluidRect(buySpinsDecreaseRect, 1.6f), buySpinsDecreaseGreyTex))
		{
		}
		GUI.color = Color.white;
		GUI.color = Color.yellow;
		if (numSpinsToBuy < competitionLogic.remainingAvailiableSpins && numSpinsToBuy * competitionLogic.spinCost < competitionLogic.numCredits - competitionLogic.spinCost)
		{
			if (GUI.Button(AspectfluidRect(buySpinsIncreaseRect, 1.6f), buySpinsIncreaseTex))
			{
				numSpinsToBuy++;
				competitionLogic.SetShopMessage();
			}
		}
		else if (!GUI.Button(AspectfluidRect(buySpinsIncreaseRect, 1.6f), buySpinsIncreaseGreyTex))
		{
		}
		GUI.color = Color.white;
		if (competitionLogic.numCredits < competitionLogic.spinCost)
		{
			GUI.color = Color.red;
		}
		else
		{
			GUI.color = Color.yellow;
		}
		buySpinsBalanceContent.text = competitionLogic.numCredits + string.Empty;
		GUI.Label(AspectfluidRect(buySpinsBalanceRect, 1.6f), buySpinsBalanceContent, "WhiteLabel");
		GUI.color = Color.white;
		buySpinsCurrentCostContent.text = string.Empty + competitionLogic.spinCost * numSpinsToBuy;
		GUI.Label(AspectfluidRect(buySpinsCurrentCostRect, 1.6f), buySpinsCurrentCostContent, "WhiteLabel");
		GUI.Label(AspectfluidRect(buySpinsNumberRect, 1.6f), string.Empty, "TextField");
		GUI.Label(AspectfluidRect(buySpinsNumberRect, 1.6f), numSpinsToBuy + string.Empty, "WhiteLabel");
		if (GUI.Button(AspectfluidRect(buySpinsCancelRect, 1.6f), buySpinsCancelContent, "CancelButton"))
		{
			StartCoroutine(HidePopup(0.2f, State.Spinner));
		}
		if (competitionLogic.numCredits >= competitionLogic.spinCost && competitionLogic.remainingAvailiableSpins > 0)
		{
			if (GUI.Button(AspectfluidRect(buySpinsPurchaseRect, 1.6f), buySpinsPurchaseContent, "ContinueButton"))
			{
				competitionLogic.BuySpins(numSpinsToBuy);
				StartCoroutine(HidePopup(0.2f, State.Spinner));
			}
		}
		else if (!GUI.Button(AspectfluidRect(buySpinsPurchaseRect, 1.6f), buySpinsPurchaseContent, "ContinueGreyButton"))
		{
		}
	}

	private void RulesGUI()
	{
		GUI.color = Color.yellow;
		if (rulesScrollPos < 0f)
		{
			rulesScrollPos = 0f;
		}
		if (rulesScrollPos == 0f)
		{
			GUI.DrawTexture(AspectfluidRect(rulesDownBtnRect, 1.6f), rulesDownGreyTex, ScaleMode.ScaleToFit);
			if (!GUI.RepeatButton(AspectfluidRect(rulesDownBtnRect, 1.6f), string.Empty, "InvisableButton"))
			{
			}
		}
		else
		{
			GUI.DrawTexture(AspectfluidRect(rulesDownBtnRect, 1.6f), rulesDownTex, ScaleMode.ScaleToFit);
			if (GUI.RepeatButton(AspectfluidRect(rulesDownBtnRect, 1.6f), string.Empty, "InvisableButton"))
			{
				rulesScrollPos -= Time.deltaTime * rulesScrollSpeed;
			}
		}
		if (rulesScrollPos > 0.9f)
		{
			rulesScrollPos = 0.9f;
		}
		if (rulesScrollPos == 0.9f)
		{
			GUI.DrawTexture(AspectfluidRect(rulesUpBtnRect, 1.6f), rulesUpGreyTex, ScaleMode.ScaleToFit);
			if (!GUI.RepeatButton(AspectfluidRect(rulesUpBtnRect, 1.6f), string.Empty, "InvisableButton"))
			{
			}
		}
		else
		{
			GUI.DrawTexture(AspectfluidRect(rulesUpBtnRect, 1.6f), rulesUpTex, ScaleMode.ScaleToFit);
			if (GUI.RepeatButton(AspectfluidRect(rulesUpBtnRect, 1.6f), string.Empty, "InvisableButton"))
			{
				rulesScrollPos += Time.deltaTime * rulesScrollSpeed;
			}
		}
		GUI.color = Color.white;
		rules.GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex", new Vector2(0f, rulesScrollPos));
		if (competitionLogic.HasAcceptedRules())
		{
			if (GUI.Button(AspectfluidRect(rulesContinueRect, 1.6f), rulesContinueContent, "ContinueButton"))
			{
				rules.SetActiveRecursively(false);
				if (competitionLogic.HasAcceptedRules())
				{
					StartCoroutine(HidePopup(0.2f, State.Spinner));
				}
				else
				{
					StartCoroutine(HidePopup(0.2f, State.Empty));
				}
			}
			return;
		}
		if (GUI.Button(AspectfluidRect(rulesDeclineRect, 1.6f), rulesDeclineContent, "CancelButton"))
		{
			rules.SetActiveRecursively(false);
			if (competitionLogic.HasAcceptedRules())
			{
				StartCoroutine(HidePopup(0.2f, State.Spinner));
			}
			else
			{
				StartCoroutine(HidePopup(0.2f, State.Empty));
			}
			End();
		}
		if (GUI.Button(AspectfluidRect(rulesAcceptRect, 1.6f), rulesAcceptContent, "ContinueButton"))
		{
			rules.SetActiveRecursively(false);
			if (competitionLogic.HasAcceptedRules())
			{
				StartCoroutine(HidePopup(0.2f, State.Spinner));
			}
			else
			{
				StartCoroutine(HidePopup(0.2f, State.Empty));
			}
			competitionLogic.AcceptRules(true);
		}
	}

	private void TCGUI()
	{
		GUI.Label(AspectfluidRect(tcDescRect, 1.6f), tcDescContent, "WhiteLabel");
		if (GUI.Button(AspectfluidRect(tcBtnRect, 1.6f), tcBtnContent, "StandardButton"))
		{
			Application.OpenURL("http://www.the-smiler.com/competition-terms");
		}
		if (GUI.Button(AspectfluidRect(tcBackRect, 1.6f), tcBackContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.2f, State.Spinner));
		}
	}

	private void SupportGUI()
	{
		GUI.Label(AspectfluidRect(supportDescRect, 1.6f), supportDescContent, "WhiteLabel");
		if (GUI.Button(AspectfluidRect(supportBtnRect, 1.6f), supportBtnContent, "StandardButton"))
		{
			Application.OpenURL("http://www.the-smiler.com/the-ride");
		}
		if (GUI.Button(AspectfluidRect(supportBackRect, 1.6f), supportBackContent, "ContinueButton"))
		{
			StartCoroutine(HidePopup(0.2f, State.Previous));
			state = State.Previous;
			spinner.SetPlayerControl(false);
			winnings.enabled = true;
			MonoBehaviour.print(1);
		}
	}

	public void ClosePrizes()
	{
		StartCoroutine(HidePopup(0.2f, State.Spinner));
	}

	private IEnumerator HidePopup(float waitTime, State newstate)
	{
		state = State.Empty;
		perspectiveHud.inst.HidePopUpWindowC();
		competitionLogic.CheckWheelLock();
		yield return new WaitForSeconds(waitTime);
		state = newstate;
	}

	public IEnumerator LoadErrorScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("ERROR", true);
		yield return new WaitForSeconds(waitTime);
		state = State.Broken;
	}

	public IEnumerator LoadBuySpinsScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("Buy Spins", false);
		yield return new WaitForSeconds(waitTime);
		state = State.BuySpins;
	}

	public IEnumerator TutoralSkiped()
	{
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("PROFILE", false);
		yield return new WaitForSeconds(0.5f);
		nameFieldText = ((competitionLogic.name != null) ? competitionLogic.name : string.Empty);
		emailFieldText = ((competitionLogic.email != null) ? competitionLogic.email : string.Empty);
		phoneFieldText = ((competitionLogic.phone != null) ? competitionLogic.phone : string.Empty);
		toggleStatus = competitionLogic.optin;
		emailValidating = false;
		state = State.Email;
	}

	public void TutoralStart()
	{
		state = State.Empty;
		WaitForTutoral = true;
	}

	public void TutoralEnded()
	{
		state = State.Empty;
		WaitForTutoral = false;
		StartCoroutine(LoadDetailsScreen(0.5f));
	}

	public IEnumerator LoadDetailsScreen(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		TutoralDisplay.inst.TriggerTutoralC(5, base.gameObject);
		state = State.Empty;
		spinner.SetPlayerControl(false);
	}

	public IEnumerator LoadLoseScreen(float waitTime)
	{
		loseSound.Play();
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("SORRY!", true);
		yield return new WaitForSeconds(waitTime);
		state = State.Lose;
	}

	public IEnumerator LoadWinScreen(float waitTime)
	{
		winSound.Play();
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("WINNER!", false);
		yield return new WaitForSeconds(waitTime);
		AchievementManager.SetProgressToAchievement("Spin and Win", 1f);
		state = State.Win;
	}

	public IEnumerator LoadRulesScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("Rules", false);
		yield return new WaitForSeconds(waitTime);
		rules.SetActiveRecursively(true);
		state = State.Rules;
	}

	public IEnumerator LoadTCScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("TERMS", false);
		yield return new WaitForSeconds(waitTime);
		state = State.TC;
	}

	public IEnumerator LoadPrizesScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("PRIZE LIST", false);
		yield return new WaitForSeconds(waitTime);
		prizes.enabled = true;
		state = State.Prizes;
	}

	public IEnumerator LoadMaxSpinsScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("MAX SPINS", false);
		yield return new WaitForSeconds(waitTime);
		state = State.MaxSpins;
	}

	public IEnumerator LoadDetailScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("Prize", false);
		yield return new WaitForSeconds(waitTime);
		state = State.Detail;
	}

	public IEnumerator LoadSupportScreen(float waitTime)
	{
		spinner.SetPlayerControl(false);
		state = State.Empty;
		perspectiveHud.inst.ShowPopUpWindow("Support", false);
		yield return new WaitForSeconds(waitTime);
		state = State.Support;
	}

	public Rect FluidRect(Rect rect)
	{
		rect.x *= Screen.width;
		rect.y *= Screen.height;
		rect.width *= Screen.width;
		rect.height *= Screen.height;
		return rect;
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public Rect AspectfluidRect(Rect rect, float Aspect)
	{
		float num = Aspect * (float)Screen.height;
		rect.x = rect.x * num - (num - (float)Screen.width) * 0.5f;
		rect.y *= Screen.height;
		rect.width *= num;
		rect.height *= Screen.height;
		return rect;
	}

	public Rect AspectfluidRect(float x, float y, float width, float height, float Aspect)
	{
		float num = Aspect * (float)Screen.height;
		return new Rect(x * num - (num - (float)Screen.width) * 0.5f, y * (float)Screen.height, width * num, height * (float)Screen.height);
	}
}

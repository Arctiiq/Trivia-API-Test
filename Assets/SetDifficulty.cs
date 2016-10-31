using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SetDifficulty : MonoBehaviour 
{

	[Header("Booleans")]
	public bool ShowAdvanced 			= false;
	public bool IsGeneratingQuestions 	= false;
	public bool Any						= false;
	public bool Easy					= false;
	public bool Medium					= false;
	public bool Hard					= false;

	public bool StartQuiz 				= false;

	[Header("Dropdowns")]
	public Dropdown category;

	[Header("Toggles")]
	public Toggle 	tgl_AnyDiff;
	public Toggle 	tgl_EasyDiff;
	public Toggle 	tgl_MedDiff;
	public Toggle 	tgl_HardDiff;
	public Toggle	tgl_Casual;
	public Toggle	tgl_Timed;

	[Header("Buttons")]
	public Button 	btn_ShowAdvanced;
	public Button 	btn_GenerateQuiz;
	public Button	btn_ShowHighScores;

	[Header("Text")]
	public Text 	txt_ControlText;
	public Text		txt_SliderEasy;
	public Text		txt_SliderMed;
	public Text		txt_SliderHard;
	public Text 	txt_TotalNumQuestions;
	public Text 	txt_Warning;
	public Text		txt_Mode;

	[Header("Slider")]
	public Slider 	SL_Num;
	public Slider 	SL_Easy;
	public Slider	SL_Medium;
	public Slider	SL_Hard;

	[Header("Canvases")]
	public Canvas Canvas_Slider;
	public Canvas Canvas_Difficulty;
	public Canvas Canvas_Extra;
	public Canvas Canvas_Options;

	public Canvas Canvas_Questions;
	public Canvas Canvas_Confirm;


	[Header("Confirm Panel")]
	public GameObject panel;
	public Text 	txt_Data;
	public Button 	btn_Yes;
	public Button	btn_No;

	private float max_questions;
	private float totalQuestions;
	private float numEasy;
	private float numMedium;
	private float numHard;

	// Use this for initialization
	void Start () 
	{
		max_questions = 50;
		Canvas_Slider.enabled = false;
	}

	public void showData()
	{
		Canvas_Slider.enabled 		= false;
		Canvas_Difficulty.enabled 	= false;
		Canvas_Options.enabled		= false;
		Canvas_Extra.enabled		= false;

		Canvas_Confirm.enabled		= true;

		string cat = category.captionText.text;

		if (!ShowAdvanced)
		{
			if (Easy)
				txt_Data.GetComponent<Text>().text = "Number of Questions: " + numQuestions + "\nDifficulty: Easy\nCategory: " + cat;
			else if (Medium)
				txt_Data.GetComponent<Text>().text = "Number of Questions: " + numQuestions + "\nDifficulty: Medium\nCategory: " + cat;
			else if (Hard)
				txt_Data.GetComponent<Text>().text = "Number of Questions: " + numQuestions + "\nDifficulty: Hard\nCategory: " + cat;
			else
				txt_Data.GetComponent<Text>().text = "Number of Questions: " + numQuestions + "\nDifficulty: Any Difficulty\nCategory: " + cat;
		}
	}

	public void hideData()
	{
		Canvas_Difficulty.enabled 	= true;
		Canvas_Options.enabled		= true;
		Canvas_Extra.enabled		= true;

		Canvas_Confirm.enabled		= false;
		ShowAdvanced = false;
	}

	public void ShowGame()
	{
		Canvas_Slider.enabled 		= false;
		Canvas_Difficulty.enabled 	= false;
		Canvas_Options.enabled		= false;
		Canvas_Extra.enabled		= false;
		Canvas_Confirm.enabled		= false;

		Canvas_Questions.enabled	= true;
		this.gameObject.GetComponent<QuestionHandler>().StartTriviaGame();

	}

	// Update is called once per frame
	void Update () 
	{

		btn_ShowAdvanced.interactable = (tgl_AnyDiff.isOn) ? true : false;

		Any 	= (tgl_AnyDiff.isOn) 	? true : false;
		Easy 	= (tgl_EasyDiff.isOn) 	? true : false;
		Medium 	= (tgl_MedDiff.isOn) 	? true : false;
		Hard	= (tgl_HardDiff.isOn) 	? true : false;
			
			

		txt_SliderEasy.GetComponent<Text>().text 	= SL_Easy.value.ToString();
		txt_SliderMed.GetComponent<Text>().text 	= SL_Medium.value.ToString();
		txt_SliderHard.GetComponent<Text>().text 	= SL_Hard.value.ToString();

		numEasy 	= SL_Easy.value;
		numMedium 	= SL_Medium.value;
		numHard 	= SL_Hard.value;

		totalQuestions = numEasy + numMedium + numHard;

		txt_TotalNumQuestions.GetComponent<Text>().text = totalQuestions.ToString();


		//--You can't have 0 questions or greater than 50 questions (because the API can only generate 50 questions)--
		if (totalQuestions == 0 || totalQuestions > 50)
		{
			txt_TotalNumQuestions.color = Color.red;
			txt_Warning.GetComponent<Text>().text = "Warning: Total questions can only be between 1 and 50";
			btn_GenerateQuiz.interactable = false;
		}
		else
		{
			btn_GenerateQuiz.interactable = true;
			txt_TotalNumQuestions.color = Color.white;
			txt_Warning.GetComponent<Text>().text = "";
		}
		//---------------------------------------------

		//--Changes position of the mode text to make it easier to identify--
		if (tgl_Casual.isOn)
		{
			txt_Mode.GetComponent<Text>().text = "Answer questions at your own pace.";
			txt_Mode.transform.position = new Vector2(txt_Mode.transform.position.x,tgl_Casual.transform.position.y);
		}
		else if (tgl_Timed.isOn)
		{
			txt_Mode.GetComponent<Text>().text = "Answer questions within a set time limit.";
			txt_Mode.transform.position = new Vector2(txt_Mode.transform.position.x,tgl_Timed.transform.position.y);
		}
		//---------------------------------------------


	}

	public void GoToWebsite()
	{
		Application.OpenURL("https://opentdb.com");
	}

	/// <summary>
	/// Used for the button
	/// </summary>
	public void ShowHideAdvancedEntry()
	{
		if (!ShowAdvanced)//Show the Advanced objects
		{
			ShowAdvanced = true;
			btn_ShowAdvanced.GetComponentInChildren<Text>().text = "Hide Advanced";
			txt_ControlText.enabled 	= true;
			Canvas_Slider.enabled 		= true;

			SL_Num.interactable 		= false;
			tgl_EasyDiff.interactable 	= false;
			tgl_MedDiff.interactable 	= false;
			tgl_HardDiff.interactable 	= false;

		}
		else //Hide the advanced objects
		{
			ShowAdvanced = false;
			btn_ShowAdvanced.GetComponentInChildren<Text>().text = "Show Advanced";

			txt_ControlText.enabled 	= false;
			Canvas_Slider.enabled		= false;

			SL_Num.interactable 		= true;
			tgl_EasyDiff.interactable 	= true;
			tgl_MedDiff.interactable 	= true;
			tgl_HardDiff.interactable 	= true;

			SL_Easy.value 	= 1;
			SL_Medium.value = 1;
			SL_Hard.value 	= 1;


		}
	}


	/// <summary>
	/// Getter to determine if the player selected the advanced mode
	/// </summary>
	/// <value><c>true</c> if advanced mode; otherwise, <c>false</c>.</value>
	public bool AdvancedMode
	{
		get{return ShowAdvanced;}
	}

	/// <summary>
	/// Used for non-advanced mode
	/// </summary>
	/// <value>The number questions.</value>
	public float numQuestions
	{
		get {return SL_Num.value;}
	}

	public float TotalQuestions
	{
		get {return totalQuestions;}
	}

	public float NumEasy
	{
		get {return numEasy;}
	}
	public float NumMedium
	{
		get {return numMedium;}
	}
	public float NumHard
	{
		get {return numHard;}
	}

	public bool StartNewQuiz
	{
		get {return StartQuiz;}
	}





}

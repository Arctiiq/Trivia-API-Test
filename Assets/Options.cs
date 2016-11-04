using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Options : MonoBehaviour 
{

	
	public Dropdown ResAspect;
	public Dropdown Resolutions;
	public Toggle	FullScreen;

	public Canvas Canvas_Options;
	public Canvas Canvas_Options_Difficulty;
	public Canvas Canvas_Options_Extra;
	public Canvas Canvas_Resolution;

	private double aspect1;//4:3
	private double aspect2;//5:4
	private double aspect3;//16:9
	private double aspect4;//16:10

	private List<Resolution> res_A1;//4:3
	private List<Resolution> res_A2;//5:4
	private List<Resolution> res_A3;//16:9
	private List<Resolution> res_A4;//16:10

	private List<string> text_A1;//4:3
	private List<string> text_A2;//5:4
	private List<string> text_A3;//16:9
	private List<string> text_A4;//16:10

	private List<string> Aspects;

	private double currentAspect;
	private int resW;
	private int resH;

	private bool isFullScreen;

	private Resolution nativeRes;

	private Resolution current_SelectedRes;

	// Use this for initialization
	void Start () 
	{

		aspect1 = (4.0 / 3.0);
		aspect2 = (5.0 / 4.0);
		aspect3 = (16.0 / 9.0);
		aspect4 = (16.0 / 10.0);
		Aspects = new List<string>();

		text_A1 = new List<string>();
		text_A2 = new List<string>();
		text_A3 = new List<string>();
		text_A4 = new List<string>();

		res_A1 = new List<Resolution>();
		res_A2 = new List<Resolution>();
		res_A3 = new List<Resolution>();
		res_A4 = new List<Resolution>();

		Aspects.Add("4:3");
		Aspects.Add("5:4");
		Aspects.Add("16:9");
		Aspects.Add("16:10");

		ResAspect.ClearOptions();
		ResAspect.AddOptions(Aspects);




		foreach (Resolution res in Screen.resolutions)
		{
			double testAspect = (double)res.width / (double)res.height;

			if (testAspect == aspect1)
			{
				res_A1.Add(res);
				text_A1.Add(res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added: " + res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added to 4:3");
			}
			else if (testAspect == aspect2)
			{
				res_A2.Add(res);
				text_A2.Add(res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added: " + res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added to 5:4");
			}
			else if (testAspect == aspect3)
			{
				res_A3.Add(res);
				text_A3.Add(res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added: " + res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added to 16:9");
			}
			else if (testAspect == aspect4)
			{
				res_A4.Add(res);
				text_A4.Add(res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added: " + res.width.ToString() + "x" + res.height.ToString());
				Debug.Log("Added to 16:10");
			}
			
		}

		Resolutions.ClearOptions();
		Resolutions.AddOptions(text_A1);

		//LoadCurrentResolution();

		DefaultNativeRes();
		FullScreen.isOn = Screen.fullScreen ? true : false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (ResAspect.value == 0)
			currentAspect = aspect1;
		else if (ResAspect.value == 1)
			currentAspect = aspect2;
		else if (ResAspect.value == 2)
			currentAspect = aspect3;
		else if (ResAspect.value == 3)
			currentAspect = aspect4;


	}

	public void SetAspect()
	{

		if (ResAspect.value == 0)
		{
			Resolutions.ClearOptions();
			Resolutions.AddOptions(text_A1);
			Resolutions.value = 0;
			Debug.Log("Switched to 4:3");
		}
		else if (ResAspect.value == 1)
		{
			Resolutions.ClearOptions();
			Resolutions.AddOptions(text_A2);
			Resolutions.value = 0;
			Debug.Log("Switched to 5:4");
		}
		else if (ResAspect.value == 2)
		{
			Resolutions.ClearOptions();
			Resolutions.AddOptions(text_A3);
			Resolutions.value = 0;
			Debug.Log("Switched to 16:9");
		}
		else if (ResAspect.value == 3)
		{
			Resolutions.ClearOptions();
			Resolutions.AddOptions(text_A4);
			Resolutions.value = 0;
			Debug.Log("Switched to 16:10");
		}

	}

	public void DefaultNativeRes()
	{
		if (!PlayerPrefs.HasKey("defaultResW") && !PlayerPrefs.HasKey("defaultResH"))
		{
			Screen.fullScreen = false;
			PlayerPrefs.SetInt("defaultResW",Screen.currentResolution.width);
			PlayerPrefs.SetInt("defaultResH",Screen.currentResolution.height);

			PlayerPrefs.Save();

			bool full = Screen.fullScreen;
			Screen.SetResolution(PlayerPrefs.GetInt("defaultResW"),PlayerPrefs.GetInt("defaultResH"), true);
		}
		else 
		{
			Screen.fullScreen = (PlayerPrefs.HasKey("isFullScreen") && System.Convert.ToBoolean(PlayerPrefs.GetInt("isFullScreen"))) ? true: false;

			bool full = Screen.fullScreen;

			Screen.SetResolution(PlayerPrefs.GetInt("defaultResW"),PlayerPrefs.GetInt("defaultResH"), full);
		}
	}

	IEnumerator WaitForScreenChange()
	{
		Screen.SetResolution(resW,resH, FullScreen.isOn);

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
	}

	void SaveData(List<Resolution> Resolutions)
	{

		for (int i = 0; i < Resolutions.Count; i++)
		{
			string resTest = Resolutions[i].width.ToString() + "x" + Resolutions[i].height.ToString();


			if (this.Resolutions.value == i)
			{
				resW = Resolutions[i].width;
				resH = Resolutions[i].height;

				Debug.Log(resW + "x" + resH);

				StartCoroutine(WaitForScreenChange());

			}

		}
		PlayerPrefs.Save();
	}

	public void SetResolution()
	{
		
		if (ResAspect.value == 0)
			SaveData(res_A1);
		else if (ResAspect.value == 1)
			SaveData(res_A2);
		else if (ResAspect.value == 2)
			SaveData(res_A3);
		else if (ResAspect.value == 3)
			SaveData(res_A4);

		Debug.Log("Setting resolution of: " + resW + "x" + resH);
		//Screen.SetResolution(resW,resH,isFullScreen);
		//LoadCurrentResolution();
	}

	public void FindRes(List<string> text)
	{
		Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,FullScreen.isOn);
		string screen = (Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString());
		SaveCurrentResolution();
		for (int i = 0; i < text.Count; i++)
		{
			
			if (screen == Resolutions.options[i].ToString())
			{
				Debug.Log(Resolutions.options[i].ToString());
				Resolutions.value = i;
			}
		}
	}

	IEnumerator NativeRes(bool isFull)
	{
		Screen.fullScreen = isFull;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
	}

	public void SetNativeRes()
	{
		double ScreenAspect = 0.0;
		bool checkFullScreen = false;
		if (isFullScreen)
		{
			StartCoroutine(NativeRes(false));
			ScreenAspect = (double)Screen.currentResolution.width / (double)Screen.currentResolution.height;
			StartCoroutine(NativeRes(true));
			checkFullScreen = true;
		}

		if (!checkFullScreen)
		ScreenAspect = (double)Screen.currentResolution.width / (double)Screen.currentResolution.height;

		if (Screen.fullScreen == false && isFullScreen)
			Screen.fullScreen = true;

		if (ScreenAspect == aspect1)
		{
			ResAspect.value = 0;
			FindRes(text_A1);
		}
		else if (ScreenAspect == aspect2)
		{
			ResAspect.value = 1;
			FindRes(text_A2);
		}
		else if (ScreenAspect == aspect3)
		{
			ResAspect.value = 2;
			FindRes(text_A3);
		}
		else if (ScreenAspect == aspect4)
		{
			ResAspect.value = 3;
			FindRes(text_A4);
		}
	}


	public void SaveCurrentResolution()
	{	
		PlayerPrefs.SetInt("resW", resW);
		PlayerPrefs.SetInt("resH", resH);

		if (isFullScreen)
			PlayerPrefs.SetInt("isFullScreen", 1);
		else
			PlayerPrefs.SetInt("isFullScreen", 0);
	}

	/// <summary>
	/// Used for loading the saved resolutions at game startup
	/// </summary>
	public void LoadCurrentResolution()
	{
		resW = PlayerPrefs.GetInt("resW");
		resH = PlayerPrefs.GetInt("resH");
	
		Screen.SetResolution(resW,resH, FullScreen.isOn);
	}

	public void ToggleFullscreen()
	{
		if (FullScreen.isOn)
			isFullScreen = true;
		else
			isFullScreen = false;

		Debug.Log("Fullscreen: " + isFullScreen);
	}

	public void LoadResolutionOptions()
	{
		Canvas_Resolution.enabled 			= true;

		Canvas_Options.enabled 				= false;
		Canvas_Options_Difficulty.enabled	= false;
		Canvas_Options_Extra.enabled		= false;
	}

	public void Return()
	{
		Canvas_Resolution.enabled 			= false;

		Canvas_Options.enabled 				= true;
		Canvas_Options_Difficulty.enabled	= true;
		Canvas_Options_Extra.enabled		= true;
	}


}



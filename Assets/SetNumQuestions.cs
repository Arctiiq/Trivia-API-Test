using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetNumQuestions : MonoBehaviour {

	public Slider slider;
	public Text text;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		text.GetComponent<Text>().text = slider.value.ToString();
	}

	/// <summary>
	/// Returns the number of questions set by the slider
	/// </summary>
	/// <value>The number questions.</value>
	public float NumQuestions
	{
		get{return slider.value;}	
	}
}

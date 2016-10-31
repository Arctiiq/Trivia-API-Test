using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
public class SetCategories : MonoBehaviour {

	public Dropdown drop;

	JSONNode node;
	List<string> options;

	// Use this for initialization
	void Start () 
	{	
		
		options = new List<string>();

		WWW url = new WWW("https://opentdb.com/api_category.php");

		while (!url.isDone)
		{
			//Wait till URL is finished downloading
		}

		node = JSON.Parse(url.text);
		//node = n;

		for(int i = 0; i < node["trivia_categories"].AsArray.Count; i++)//Iterates through each of the categories and adds it to the list
		{
			options.Add(node["trivia_categories"][i]["name"]);
		}
		options.Insert(0,"Any Category");//We have to add this since it's not in the list itself



		drop.ClearOptions();
		drop.AddOptions(options);

	}
	
	// Update is called once per frame
	void Update () {
	
	}




	/// <summary>
	/// Returns the currently selected category in the dropdown
	/// </summary>
	/// <returns>The category.</returns>
	public string GetCategory
	{
		//category.captionText.text;
		get {return drop.GetComponent<Dropdown>().captionText.text;}
	}

	public JSONNode GetJson
	{
		get {return node;}
	}
}

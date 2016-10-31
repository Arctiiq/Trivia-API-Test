using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class QuestionHandler : MonoBehaviour 
{
	
	public Text txt_Question;
	public Text txt_numQuestion;
	public Text txt_category;
	public Text txt_difficulty;

	public Button a1;
	public Button a2;
	public Button a3;
	public Button a4;


	int currentQuestion;
	int maxQuestions = 0;

	bool TriviaInProgress = false;

	SetDifficulty diff;
	SetCategories cat;

	List<Question> questionList;//A list to store each question in
	WWW url;
	Token token;

	// Use this for initialization
	void Start () 
	{
		diff = this.gameObject.GetComponent<SetDifficulty>();
		cat = this.gameObject.GetComponent<SetCategories>();


		currentQuestion = 0;
		questionList = new List<Question>();
		maxQuestions = questionList.Count + 1;






	}

	public void StartTriviaGame()
	{
		
		currentQuestion = 0;
		token = new Token();//Initalizes a new token for generating new questions
		GenerateFromToken();
		SetQuestion(0);
		TriviaInProgress = true;

	}

	void CheckQuestion()
	{

		
	}

	IEnumerator ShowCorrectQuestion()
	{

		yield return new WaitForSeconds(1f);
	}

	void SetQuestion(int questionNum)
	{

		List<string> text = new List<string>();
		text.Add(questionList[questionNum].CorrectAnswer);
		text.Add(questionList[questionNum].IncorrectAnswers[0]);
		text.Add(questionList[questionNum].IncorrectAnswers[1]);
		text.Add(questionList[questionNum].IncorrectAnswers[2]);

		text[0] = text[0].Replace("\"","");

		//Randomizes the list
		if (text[1] != null && text[2] != null)
		{
			for (int i = 0; i < text.Count; i++) 
			{
				string temp = text[i];
				int randomIndex = Random.Range(i, text.Count);
				text[i] = text[randomIndex];
				text[randomIndex] = temp;
	     	}
     	}
		
		string s = questionList[questionNum].GetQuestion.Replace("&quot;","\"");
		txt_Question.GetComponent<Text>().text = s;
		txt_Question.GetComponent<Text>().text = questionList[questionNum].GetQuestion.Replace("&#039;","\'");




		a1.GetComponentInChildren<Text>().text = text[0];
		a2.GetComponentInChildren<Text>().text = text[1];
		a3.GetComponentInChildren<Text>().text = text[2];
		a4.GetComponentInChildren<Text>().text = text[3];




	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (TriviaInProgress)
		{
			txt_numQuestion.GetComponent<Text>().text = "Question: " + (currentQuestion + 1) + "/" + maxQuestions.ToString();
			txt_category.GetComponent<Text>().text = "Category: " + questionList[currentQuestion].Category.ToString();
			txt_difficulty.GetComponent<Text>().text = "Difficulty: " + questionList[currentQuestion].Difficulty;
		}
	}

	void GenerateFromToken()
	{
		
		//token.ResetToken();
		if (!diff.AdvancedMode)
		{	
			for (int i = 0; i < cat.GetJson["trivia_categories"].AsArray.Count;i++)
			{
				Debug.Log(cat.GetCategory.ToString());
				//Check if selected category is in the list
				if (cat.GetCategory.ToString() == cat.GetJson["trivia_categories"][i]["name"].ToString())
				{
					
					int id = cat.GetJson["trivia_categories"][i]["id"].AsInt;
					//Debug.Log("Category id= " + id);
					if (diff.Easy)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&category=" + id +"&difficulty=easy&token="+token.GetToken);
					}
					else if (diff.Medium)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&category=" + id +"&difficulty=medium&token="+token.GetToken);
					}
					else if (diff.Hard)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&category=" + id +"&difficulty=hard&token="+token.GetToken);
					}
					else
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&category=" + id +"&token="+token.GetToken);
					}
				}
				else if (cat.GetCategory.ToString() != cat.GetJson["trivia_categories"][i]["name"].ToString() && i == cat.GetJson["trivia_categories"].AsArray.Count) //If we selected Any Category
				{
					Debug.Log("Selected Any Category");
					//Debug.Log(cat.GetJson["trivia_categories"][i]["name"].ToString());
					if (diff.Easy)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&difficulty=easy&token="+token.GetToken);
					}
					else if (diff.Medium)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&difficulty=medium&token="+token.GetToken);
					}
					else if (diff.Hard)
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&difficulty=hard&token="+token.GetToken);
					}
					else
					{
						url = new WWW("https://www.opentdb.com/api.php?amount=" + diff.numQuestions + "&token="+token.GetToken);
					}
				}
			}

			if (url != null)
			{	
				JSONNode node;
				while (!url.isDone)
				{}
				node = JSON.Parse(url.text);
				for (int i = 0; i < node["results"].AsArray.Count; i++)
				{
					string[] s = {"","",""};

					if (node["results"][i]["question"].ToString().Contains("&quot;"))
					{
						node["results"][i]["question"].ToString().Replace("&quot;","/");	
					}


					s[0] = node["results"][i]["incorrect_answers"][0];
					s[1] = node["results"][i]["incorrect_answers"][1];
					s[2] = node["results"][i]["incorrect_answers"][2];
					
					Question q = new Question(	node["results"][i]["question"].ToString(),
												node["results"][i]["correct_answer"].ToString(),
												s, 
												node["results"][i]["category"].ToString(),
												node["results"][i]["difficulty"].ToString());

					
					questionList.Add(q);
				}

			}
			else
			{
				Debug.Log("Couldn't retrieve Json from the URL!");
			}
		}
	}
}

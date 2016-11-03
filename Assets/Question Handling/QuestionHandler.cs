
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

	public Canvas Canvas_End;
	public Canvas Canvas_Question;
	public Canvas Canvas_Options;
	public Canvas Canvas_Options_Difficulty;
	public Canvas Canvas_Options_Difficulty_Extra;
	public Text Text_Data;

	public Text txt_NumRight;



	public int numRight = 0;

	Color defaultHighlightedColor;


	int currentQuestion;
	int maxQuestions;

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



		defaultHighlightedColor = a1.GetComponent<Image>().color;

	}

	public void StartTriviaGame()
	{
		
		currentQuestion = 0;

		token = new Token();//Initalizes a new token for generating new questions
		StartCoroutine(token.WaitForDownload());//Waits for the token to finish downloading
		token.ParseToken();//Parses the token information

		GenerateFromToken();

		maxQuestions = questionList.Count;

		SetQuestion(0);
		TriviaInProgress = true;

		Debug.Log(questionList.Count + " questions");

	}

	bool CheckQuestion(string selectedAnswer)
	{
		if (selectedAnswer == questionList[currentQuestion].CorrectAnswer.ToString().Replace("\"",""))//If selected answer is equal to the correct answer
			return true;
			
		return false;
	}

	public void Answer1()
	{
		if (CheckQuestion(a1.GetComponentInChildren<Text>().text))
			StartCoroutine(ShowCorrectQuestion(true, a1));
		else
			StartCoroutine(ShowCorrectQuestion(false, a1));
		
	}

	public void Answer2()
	{
		if (CheckQuestion(a2.GetComponentInChildren<Text>().text))
			StartCoroutine(ShowCorrectQuestion(true, a2));
		else
			StartCoroutine(ShowCorrectQuestion(false, a2));
	}

	public void Answer3()
	{
		if (a3.GetComponentInChildren<Text>().text != "")
		{
			if (CheckQuestion(a3.GetComponentInChildren<Text>().text))
				StartCoroutine(ShowCorrectQuestion(true, a3));
			else
				StartCoroutine(ShowCorrectQuestion(false, a3));
		}
	}

	public void Answer4()
	{
		if (a4.GetComponentInChildren<Text>().text != "")
		{
			if (CheckQuestion(a4.GetComponentInChildren<Text>().text))
				StartCoroutine(ShowCorrectQuestion(true, a4));
			else
				StartCoroutine(ShowCorrectQuestion(false, a4));
		}
	}

	IEnumerator ShowCorrectQuestion(bool correct, Button selectedButton)
	{

		if (correct)
		{

			selectedButton.GetComponent<Image>().color = Color.green;
			numRight++;
			Debug.Log("Answer is correct!");
		}
		else
		{
			Debug.Log("Answer is not correct!");
			if (CheckQuestion(a1.GetComponentInChildren<Text>().text.ToString().Replace("\"","")))
				a1.GetComponent<Image>().color = Color.green;
			else if (CheckQuestion(a2.GetComponentInChildren<Text>().text.ToString().Replace("\"","")))
				a2.GetComponent<Image>().color = Color.green;
			else if (CheckQuestion(a3.GetComponentInChildren<Text>().text.ToString().Replace("\"","")))
				a3.GetComponent<Image>().color = Color.green;
			else if (CheckQuestion(a4.GetComponentInChildren<Text>().text.ToString().Replace("\"","")))
				a4.GetComponent<Image>().color = Color.green;

			selectedButton.GetComponent<Image>().color = Color.red;
		}

		a1.interactable = false;
		a2.interactable = false;
		a3.interactable = false;
		a4.interactable = false;

		/*
		 	TODO:
		 	-Feedback if answer was correct
		 	-Feedback for scoring
		*/

		yield return new WaitForSeconds(5);

		a1.GetComponent<Image>().color = defaultHighlightedColor;
		a2.GetComponent<Image>().color = defaultHighlightedColor;
		a3.GetComponent<Image>().color = defaultHighlightedColor;
		a4.GetComponent<Image>().color = defaultHighlightedColor;

		if (currentQuestion < maxQuestions - 1)
		{
			currentQuestion++;
			SetQuestion(currentQuestion);
		}
		else if (currentQuestion == maxQuestions - 1)
		{
			EndGame();
			Debug.Log("Last question!");
		}

		a1.interactable = true;
		a2.interactable = true;
		a3.interactable = true;
		a4.interactable = true;
	}

	public IEnumerator WaitForDownload(WWW url)
	{
		if (!url.isDone)
			StartCoroutine(WaitForDownload(url));
		else
		yield break;
	}

	void EndGame()
	{
		Canvas_Question.enabled = false;
		Canvas_End.enabled = true;
		Text_Data.text = "Number of questions correct: " + numRight + "/" + maxQuestions;
	}

	public void ReturnToMain()
	{
		Canvas_End.enabled = false;
		Canvas_Options.enabled = true;
		Canvas_Options_Difficulty.enabled = true;
		Canvas_Options_Difficulty_Extra.enabled = true;
		numRight = 0;
		maxQuestions = 0;
		currentQuestion = 0;
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



		txt_Question.GetComponent<Text>().text = questionList[questionNum].GetQuestion.Replace("&quot;","\"").Replace("&#039;","\'");

		a1.GetComponentInChildren<Text>().text = text[0];
		a2.GetComponentInChildren<Text>().text = text[1];
		a3.GetComponentInChildren<Text>().text = text[2];
		a4.GetComponentInChildren<Text>().text = text[3];

		a1.GetComponentInChildren<Text>().text = a1.GetComponentInChildren<Text>().text.Replace("&quot;","\"").Replace("&#039;","\'");
		a2.GetComponentInChildren<Text>().text = a2.GetComponentInChildren<Text>().text.Replace("&quot;","\"").Replace("&#039;","\'");
		a3.GetComponentInChildren<Text>().text = a3.GetComponentInChildren<Text>().text.Replace("&quot;","\"").Replace("&#039;","\'");
		a4.GetComponentInChildren<Text>().text = a4.GetComponentInChildren<Text>().text.Replace("&quot;","\"").Replace("&#039;","\'");

	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (TriviaInProgress)
		{
			txt_numQuestion.GetComponent<Text>().text = "Question: " + (currentQuestion + 1) + "/" + maxQuestions.ToString();
			txt_category.GetComponent<Text>().text = "Category: " + questionList[currentQuestion].Category.ToString().Replace("\"","").ToUpper();
			txt_difficulty.GetComponent<Text>().text = "Difficulty: " + questionList[currentQuestion].Difficulty.Replace("\"","").ToUpper();
			txt_NumRight.text = numRight.ToString() + "/" + maxQuestions.ToString() + " correct";
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
				if (cat.GetCategory == cat.GetJson["trivia_categories"][i]["name"].ToString().Replace("\"",""))
				{
					
					int id = cat.GetJson["trivia_categories"][i]["id"].AsInt;//Returns the ID of the trivia category from the Json
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
					break;
				}
				else if (cat.GetCategory == "Any Category") //If we selected Any Category
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
					break;
				}
			}

			StartCoroutine(WaitForDownload(url));

			if (url != null)
			{	
				JSONNode node;

				node = JSON.Parse(url.text);

				for (int i = 0; i < node["results"].AsArray.Count; i++)
				{
					string[] s = {"","",""};

					s[0] = node["results"][i]["incorrect_answers"][0];
					s[1] = node["results"][i]["incorrect_answers"][1];
					s[2] = node["results"][i]["incorrect_answers"][2];
					
					Question q = new Question(	node["results"][i]["question"].ToString().Replace("\"",""),
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


} 

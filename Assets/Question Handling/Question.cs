using UnityEngine;
using System.Collections;

public class Question
{

	string		question;
	string		correct;
	string[] 	answers;
	string		category;
	string		difficulty;

	public Question(string question, string correct, string[] answers, string category, string difficulty)
	{
		this.question 	= question;
		this.correct 	= correct;
		this.answers 	= answers;
		this.category	= category;
		this.difficulty = difficulty;
	}


	public string GetQuestion
	{
		get {return question;}
	}

	public string CorrectAnswer
	{
		get {return correct;}
	}

	public string[] IncorrectAnswers
	{
		get {return answers;}
	}

	public string Category
	{
		get {return category;}
	}

	public string Difficulty
	{
		get {return difficulty;}
	}

}

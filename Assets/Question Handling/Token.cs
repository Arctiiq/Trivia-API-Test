using UnityEngine;
using System.Collections;
using SimpleJSON;

[System.Serializable]
public class Token
{
	int response_code;
	string response_message;
	string token;



	public Token()
	{	
		WWW url = new WWW("https://opentdb.com/api_token.php?command=request");


		while (!url.isDone)//Wait till the URL is finished downloading
		{}

		Debug.Log("TOKEN DOWNLOADED");
		var n = JSON.Parse(url.text);

		response_code 		= n["response_code"].AsInt;
		response_message 	= n["reponse_message"];
		token 				= n["token"];
	}

	/// <summary>
	/// Resets the active token
	/// </summary>
	public void ResetToken()
	{
		WWW url = new WWW(new WWW("https://www.opentdb.com/api_token.php?command=reset&token=" + GetToken).text);
		while (!url.isDone)
		{}
		var n = JSON.Parse(url.text);
		response_code 		= n["response_code"].AsInt;
		response_message 	= n["reponse_message"];
		token 				= n["token"];
	}

	public int ResponseCode
	{
		get {return response_code;}
	}
	public string ResponseMessage
	{
		get {return response_message;}
	}
	public string GetToken
	{
		get{return token;}
	}
}

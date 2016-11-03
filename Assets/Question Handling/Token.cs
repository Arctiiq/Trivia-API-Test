using UnityEngine;
using System.Collections;
using SimpleJSON;

//[System.Serializable]
public class Token
{
	int response_code;
	static int tokens_created = 0;
	string response_message;
	string token;

	WWW url;


	public Token()
	{	
		url = new WWW("https://opentdb.com/api_token.php?command=request");
		tokens_created++;

		Debug.Log("TOKEN DOWNLOADED");
		Debug.Log("How many tokens downloaded: " + tokens_created);
	
	}

	/// <summary>
	/// Resets the active token
	/// </summary>
	public void ResetToken()
	{
		url = new WWW(new WWW("https://www.opentdb.com/api_token.php?command=reset&token=" + GetToken).text);
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

	public void ParseToken()
	{
		if (url.isDone)
		{
			var n = JSON.Parse(url.text);
			response_code 		= n["response_code"].AsInt;
			response_message 	= n["reponse_message"].ToString();
			token 				= n["token"];
		}
	}

	public IEnumerator WaitForDownload()
	{
		while (!url.isDone)
		{
			yield return null;
		}
	}
}

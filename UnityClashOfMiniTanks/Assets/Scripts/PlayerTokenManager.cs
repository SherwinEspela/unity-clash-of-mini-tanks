using UnityEngine;
using System.Collections;

public class PlayerTokenManager : MonoBehaviour {

	private static string playerPrefsPlayerTokenString = "PlayerPrefsPlayerTokens"; 

	public static void ReplenishPlayerToken()
	{
		PlayerPrefs.SetInt(playerPrefsPlayerTokenString,3);
		PlayerPrefs.Save(); 
	}

	public static bool HasPlayerPrefsPlayerToken()
	{
		if (PlayerPrefs.HasKey(playerPrefsPlayerTokenString)) {
			return true;	
		} else {
			return false; 
		}
	}

	public static int GetRemainingPlayerTokens()
	{
		return PlayerPrefs.GetInt(playerPrefsPlayerTokenString); 
	}

	public static void ConsumePlayerToken()
	{
		int tokensRemaining = GetRemainingPlayerTokens();
		tokensRemaining --; 
		PlayerPrefs.SetInt(playerPrefsPlayerTokenString,tokensRemaining);
		PlayerPrefs.Save(); 
	}
}

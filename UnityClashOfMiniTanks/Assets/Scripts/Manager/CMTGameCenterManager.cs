using UnityEngine;
using System.Collections;

public class CMTGameCenterManager : MonoBehaviour {

	private string leaderboardID_highestScore = "com.SherwinEspela.ClashOfMiniTanks.HighestScore";
	private string leaderboardID_highestEnemyKills = "com.SherwinEspela.ClashOfMiniTanks.HighestEnemyKills";
	private string leaderboardID_LongestSurvivalTime = "com.SherwinEspela.ClashOfMiniTanks.LongestSurvivalTime"; 

	public static int savedTotalScore;
	public static int savedNumberOfEnemyKills;
	public static double savedSurvivalTime; 

	// Use this for initialization
	void Start () {
		GameCenterManager.OnAuthFinished += OnAuthFinished; 
		//GameCenterManager.OnGameCenterViewDismissed; 
		//GameCenterManager.OnLeadrboardInfoLoaded;
		//GameCenterManager.OnLeadrboardInfoLoaded; 
		//this.InitializeGameCenter (); 
		//Invoke("InitializeGameCenter",2f); 
	}

	public void InitializeGameCenter()
	{
		if (!GameCenterManager.IsPlayerAuthenticated) {
			GameCenterManager.Init ();	
		}
	}

	public void ShowHighestScoreLeaderboard()
	{
		if (GameCenterManager.IsPlayerAuthenticated) {
			GameCenterManager.ShowLeaderboard (leaderboardID_highestScore);	
		} else {
			GameCenterManager.Init (); 
		} 
	}

	public void ShowHighestEnemyKillsLeaderboard()
	{
		if (GameCenterManager.IsPlayerAuthenticated) {
			GameCenterManager.ShowLeaderboard (leaderboardID_highestEnemyKills); 	
		} else {
			GameCenterManager.Init (); 
		}
	}

	public void ShowLongestSurvivalTimeLeaderboard()
	{
		if (GameCenterManager.IsPlayerAuthenticated) {
			GameCenterManager.ShowLeaderboard (leaderboardID_LongestSurvivalTime);
		} else {
			GameCenterManager.Init ();
		}
	}

	public void ReportScoresToGameCenter(int latestScore, int latestEnemyKills, double latestSurvivalTime)
	{
		if (GameCenterManager.IsPlayerAuthenticated) {
			GameCenterManager.ReportScore (latestScore, leaderboardID_highestScore); 
			GameCenterManager.ReportScore (latestEnemyKills, leaderboardID_highestEnemyKills);
			GameCenterManager.ReportScore (latestSurvivalTime, leaderboardID_LongestSurvivalTime);	
		}
	}

	void OnAuthFinished(ISN_Result res){
		if (res.IsSucceeded) {
			IOSNativePopUpManager.showMessage ("Player Authored ", "ID: " + GameCenterManager.Player.Id + "\n" + "Alias: " + GameCenterManager.Player.Alias); 
		} else {
			IOSNativePopUpManager.showMessage ("GameCenter ", "Player auth failed"); 
		}
	}

//	void OnGameCenterViewDismissed(string data){
//		
//	}
}

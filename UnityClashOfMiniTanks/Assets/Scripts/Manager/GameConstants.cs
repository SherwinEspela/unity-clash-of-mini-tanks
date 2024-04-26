using UnityEngine;

public class GameConstants : MonoBehaviour {

	public static GameObject gameManager; 
	public static GameObject musicManager;
	public static GameObject powerUpPool;
	public static GameObject missionOutcomeAudioManager; 
	// public static bool gameOver = false; 
	public static bool playerIsOutsideTheBorder = false; 
	public static float showEndOfBattleReportDelayTime = 2f;

    public const string kStringTankData = "DataFiles/TankData.xml";
	public const string kStringSavedTankDataEditor = "DataFiles/SavedTankData.xml"; 
	public const string kStringSavedTankDataMobileDevice = "SavedTankData.xml";
	public const string kStringTankDataResourcesFolder = "TankData.xml";

	public const string kStringScoreAndCollectedItemsDataUnityEditor = "DataFiles/SavedScoreAndCollectedItemsData.xml";
	public const string kStringScoreAndCollectedItemsDataMobileDevice = "SavedScoreAndCollectedItemsData.xml";

	public static GameObject player; 
	public static int roundCounter = 1;

    public const string kRound = "ROUND ";

    public const string kTagPlayerTank = "Player";
    public const string kTagPlayerTankTurret = "PlayerTurret";
    public const string kTagEnemyTank = "EnemyTank";
	public const string kTagFriendlyTank = "FriendlyTank";
    public const string kTagBase = "BaseHealth";
    public const string kTagUntagged = "Untagged";

    public const string kTankBodyRotation = "TankBodyRotation";
    public const string kTurret = "Turret";
    public const string kPointerToNewTarget = "pointerToNewTarget";

    public const float cannonDamage = 10f; 
	public const float cannonDamageOneShotPlayer = 500f;
	public const float cannonDamageOnBase = 10f;

    public const float kPlayerReloadSpeedFast = 1.5f;
    public const float kPlayerReloadSpeedNormal = 1.5f;

    public struct PlayerTankState
    {
        public static bool CanFireCannon { get; set; }
    }

    public struct GameplayState
    {
        public static bool GamePaused { get; set; }
        public static bool GameOver { get; set; }
        public static bool MissionFailed { get; set; }
        public static bool TimeMarkReached { get; set; }
        public static bool HasTrophyReward { get; set; }
        public static double SurvivalTime { get; set; }

        public static void Init()
        {
            GameOver = false;
            GamePaused = false;
            MissionFailed = false;
            TimeMarkReached = false;
            HasTrophyReward = false;
            SurvivalTime = 0;
        }
    }

    public struct GameScene
    {
        public const string SceneMainMenu = "MainMenu";
        public const string SceneAdScene = "AdScene";
        public const string SceneLoading = "LoadingScene";
    }

    public struct Score
    {
        public const int EnemyKill = 10;
        public const int PowerUpPick = 2;
        public const int FriendlyKill = 4;
    }

    // Use this for initialization
    void Start () {
		musicManager = GameObject.Find("MusicManager");
		missionOutcomeAudioManager = GameObject.Find("MissionOutcomeAudioManager");
        PlayerTankState.CanFireCannon = true;

        GameplayState.Init();

        Invoke ("FindPlayer", 3f);
	}

	void FindPlayer()
	{
		player = GameObject.FindWithTag ("Player"); 
		gameManager = GameObject.Find("GameManager"); 
		powerUpPool = GameObject.Find("PowerUpPool");
	}

    public static string GetRound()
    {
        return "ROUND " + roundCounter;
    }
}
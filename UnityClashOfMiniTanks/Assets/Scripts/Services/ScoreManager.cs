using UnityEngine;

public interface IScoreManagerDelegate
{
    void DidScore(int scoreValue);
}

public class ScoreManager : MonoBehaviour, PowerUpManagerDelegate {

    [SerializeField] PowerUpManager powerUpManager;
    [SerializeField] BaseCounter baseCounter;

    private Score _score;
    private int _totalShot = 0;
    private SavePlayerData _savePlayerData;
    private const int _requiredEnemyKillsToGetMedalReward = 15;

    public int EnemyKills { get; set; }

    public IScoreManagerDelegate delegateScoreManager;

    void Start () {
        _savePlayerData = new SavePlayerData();
        _score = new Score();
        EnemyKills = 0;

        powerUpManager.delegatePowerUpManager = this;
	}

	public void AddEnemyKillScore()
	{
        _score.ScorePoints += GameConstants.Score.EnemyKill;
        DidScoreEvent();

        EnemyKills++; 
		if (!GameConstants.GameplayState.GameOver) {
			if (EnemyKills > _requiredEnemyKillsToGetMedalReward){
                powerUpManager.DisplayMedalReward();

                // player gets a medal reward
                _score.MedalsEarned++;
				EnemyKills = 0; 
			}	
		}
	}

	public void AddPowerUpPickScore()
	{
        _score.ScorePoints += GameConstants.Score.PowerUpPick;
        DidScoreEvent();
    }

	public void FriendlyFirePenalty()
	{
        _score.ScorePoints -= GameConstants.Score.FriendlyKill;
        _score.FriendlyFire++;
        DidScoreEvent();
    }

    public void AddTotalShot()
    {
        _totalShot++;
    }

    private float GetHitAccuracy()
    {
        return EnemyKills == 0 ? 0.0f : (EnemyKills / _totalShot) * 100.0f;
    }

    private int GetMedalsEarned()
    {
        return 1;
    }

    public Score GetScoreData()
    {
        if (GameConstants.GameplayState.TimeMarkReached)
        {
            GameConstants.GameplayState.HasTrophyReward = baseCounter.GetRemainingBase() == 4;
            _score.MedalsEarned += baseCounter.GetRemainingBase();
        }

        _score.BaseRemaining = baseCounter.GetRemainingBase();
        _score.HitAccuracy = GetHitAccuracy();

        _savePlayerData.Save(_score);

        return _score;
    }

    private void DidScoreEvent()
    {
        if (delegateScoreManager != null)
        {
            delegateScoreManager.DidScore(_score.ScorePoints);
        }
    }

    //*****************************************
    // Delegate methods
    //*****************************************

    // PowerUpManager delegate methods
    public void DidDisplayRewardMedal()
    {
        _score.MedalsEarned++;
    }

    public void DidCollectGear()
    {
        _score.GearsCollected++;
    }
}
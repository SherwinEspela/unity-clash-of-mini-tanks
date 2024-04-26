using UnityEngine;

public interface MainTimerManagerDelegate
{
}

public class MainTimerManager : MonoBehaviour {

    [SerializeField] SpawnEnemyTanksManager spawnEnemyTanksManager;
    [SerializeField] string stringTimeFinish = "04:00";

	private bool _wave2IsTriggered = false; 
	private bool _wave3IsTriggered = false; 
	private bool _wave4IsTriggered = false;
    private bool _playerWinsIsTriggered = false;

    public string TimeValue { get; set; }

    // timer handles
    vp_Timer.Handle m_Timer = new vp_Timer.Handle();

    // events
    public delegate void MainTimerEvent();
    public static event MainTimerEvent OnTimeFinished;

    void Start () { 
        Invoke("RunMainTimer", 6.28f);
    }

	void Update () {
		TimeValue = vp_TimeUtility.TimeToString (m_Timer.Duration, false, true, true, false, false, false, ':');

		if (TimeValue.Equals ("01:00") && !_wave2IsTriggered) {
            spawnEnemyTanksManager.AddEnemy(3);
            _wave2IsTriggered = true;
        } else if (TimeValue.Equals ("02:00") && !_wave3IsTriggered) {
            spawnEnemyTanksManager.AddEnemy(3);
            _wave3IsTriggered = true;
        }
		else if (TimeValue.Equals ("03:00") && !_wave4IsTriggered) {
            spawnEnemyTanksManager.AddEnemy(3);
            _wave4IsTriggered = true;
        }
		else if (TimeValue.Equals (stringTimeFinish) && !_playerWinsIsTriggered) {

			// game ends, player wins!!!
            _playerWinsIsTriggered = true;
            GameConstants.GameplayState.TimeMarkReached = true;

            if (OnTimeFinished != null) { OnTimeFinished(); }
		}
	}

	public void RunMainTimer()
	{
		vp_Timer.Start(m_Timer);	
	}

	public void StopMainTimer()
	{
        m_Timer.Cancel();
	}

	public void PauseTimer()
	{
		GameConstants.GameplayState.SurvivalTime = m_Timer.Duration;
        m_Timer.Paused = true;
	}

    public void ResumeTimer()
    {
        GameConstants.GameplayState.SurvivalTime = m_Timer.Duration;
        m_Timer.Paused = false;
    }
}
using UnityEngine;

public class BaseCounter : MonoBehaviour {

	private int numberOfBase = 4; 

    // events
    public delegate void BaseCounterEvent();
    public static event BaseCounterEvent OnAllBasesDestroyed;

    void Start()
	{
        numberOfBase = 4;
        
	}

    private void OnEnable()
    {
        HealthBase.OnBaseDestroyed += DecreaseBaseCountAndVerifyIfAllBaseIsDestroyed;
        HealthBase.OnBaseDestroyedByPlayer += DecreaseBaseCount;
    }

    private void OnDisable()
    {
        HealthBase.OnBaseDestroyed -= DecreaseBaseCountAndVerifyIfAllBaseIsDestroyed;
        HealthBase.OnBaseDestroyedByPlayer -= DecreaseBaseCount;
    }

    public void DecreaseBaseCount()
	{
		numberOfBase--;
    }

    public void DecreaseBaseCountAndVerifyIfAllBaseIsDestroyed()
    {
        DecreaseBaseCount();
        if (numberOfBase <= 0)
        {
            // game over
            GameConstants.GameplayState.MissionFailed = true;
           
            if (OnAllBasesDestroyed != null)
            {
                OnAllBasesDestroyed();
            }
        }
    }

    public int GetRemainingBase()
    {
        return numberOfBase;
    }
}

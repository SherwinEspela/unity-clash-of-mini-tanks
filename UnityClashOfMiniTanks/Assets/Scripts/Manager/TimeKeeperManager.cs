using UnityEngine;
using Chronos; 

public class TimeKeeperManager : MonoBehaviour
{
    private string friendlyTankGlobalClock = "FriendlyTanks";
    private string enemyTankGlobalClock = "EnemyTanks";
    private string mainClock = "MainClock";

    public void PauseAITanks(bool value)
    {
        // Change the global clock's time scale for Friendly and Enemy Tanks
        Timekeeper.instance.Clock(friendlyTankGlobalClock).paused = value;
        Timekeeper.instance.Clock(enemyTankGlobalClock).paused = value;
        //Timekeeper.instance.Clock(mainClock).paused = value; 

        if (value)
        {
            //Timekeeper.instance.Clock(mainClock).localTimeScale = 0f;
            Timekeeper.instance.Clock(friendlyTankGlobalClock).localTimeScale = 0f;
            Timekeeper.instance.Clock(enemyTankGlobalClock).localTimeScale = 0f;
        }
        else
        {
            //Timekeeper.instance.Clock(mainClock).localTimeScale = 1f; 
            Timekeeper.instance.Clock(friendlyTankGlobalClock).localTimeScale = 1f;
            Timekeeper.instance.Clock(enemyTankGlobalClock).localTimeScale = 1f;
        }
    }
}
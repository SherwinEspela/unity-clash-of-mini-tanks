using UnityEngine;
using System.IO;

public class SavePlayerData : MonoBehaviour
{
    public void Save(Score scoreData)
    {
        // TODO: relocate / refactor
        TankUnlockManager.gearsInventory += scoreData.GearsCollected;
        TankUnlockManager.medalsInventory += scoreData.MedalsEarned;

        if (GameConstants.GameplayState.TimeMarkReached && GameConstants.GameplayState.HasTrophyReward)
        {
            TankUnlockManager.trophiesInventory++;
        }

        // TODO: refactor / relocate / remove enemy kills
        ScoreAndCollectedItems scoreAndCollectedItems = new ScoreAndCollectedItems()
        {
            totalScore = CMTGameCenterManager.savedTotalScore + scoreData.ScorePoints,
            numberOfEnemyKills = CMTGameCenterManager.savedNumberOfEnemyKills + 100,
            collectedGears = TankUnlockManager.gearsInventory,
            collectedMedals = TankUnlockManager.medalsInventory,
            collectedTrophies = TankUnlockManager.trophiesInventory
        };

        // add the Survival time value
        if (CMTGameCenterManager.savedSurvivalTime < GameConstants.GameplayState.SurvivalTime)
        {
            scoreAndCollectedItems.survivalTime = GameConstants.GameplayState.SurvivalTime;
        }

#if UNITY_EDITOR
        XmlUtility.SavePlayerData(Path.Combine(Application.dataPath,
                                               GameConstants.kStringScoreAndCollectedItemsDataUnityEditor), scoreAndCollectedItems);
#else
        XmlUtility.SavePlayerData(Path.Combine(Application.persistentDataPath,
                                               GameConstants.kStringScoreAndCollectedItemsDataMobileDevice),scoreAndCollectedItems);
#endif
    }
}

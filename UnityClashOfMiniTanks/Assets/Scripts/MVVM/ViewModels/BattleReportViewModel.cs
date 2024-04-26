using UnityEngine;

public class BattleReportViewModel: MonoBehaviour
{
    public string TextScore { get; set; }
    public string TextBaseRemaining { get; set; }
    public string TextFriendlyFire { get; set; }
    public string TextHitAccuracy { get; set; }
    public string TextGearsCollected { get; set; }
    public string TextMedalsEarned { get; set; }
    public string TextBattleReport { get; set; }

    [SerializeField] ScoreManager scoreManager;

    public void UpdateDetails()
    {
        Score score = scoreManager.GetScoreData();
        TextScore = "Total Score: " + score.ScorePoints;
        TextBaseRemaining = "Remaining Base: " + score.BaseRemaining;
        TextFriendlyFire = "Friendly Fire: " + score.FriendlyFire;
        TextHitAccuracy = "Hit Accuracy: " + score.HitAccuracy;
        TextGearsCollected = "Gears Collected: " + score.GearsCollected;
        TextMedalsEarned = "Medals Earned: " + score.MedalsEarned;
        TextBattleReport = GetBattleReport(score.BaseRemaining);
    }

    private string GetBattleReport(int baseRemaining)
    {
        string battleReport = string.Empty;
        if (!GameConstants.GameplayState.TimeMarkReached)
        {
            return battleReport;
        }

        switch (baseRemaining)
        {
            case 1:
                battleReport = "You survived!";
                break;
            case 2:
                battleReport = "Not good enough!";
                break;
            case 3:
                battleReport = "Very good!!!";
                break;
            case 4:
                battleReport = "You saved all bases! Excellent!!!";
                break;
            default:
                break;
        }

        return battleReport;
    }
}

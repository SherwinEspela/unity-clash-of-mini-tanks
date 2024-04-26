using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour, IScoreManagerDelegate
{
    [SerializeField] Text textScore;
    [SerializeField] ScoreManager scoreManager;

    private void Start()
    {
        scoreManager.delegateScoreManager = this;
    }

    // ScoreManager delegate method
    public void DidScore(int scoreValue)
    {
        textScore.text = scoreValue.ToString();
    }
}

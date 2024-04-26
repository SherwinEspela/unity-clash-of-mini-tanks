using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IBattleReportViewControllerDelegate
{
    void BattleReportDidClickPlayAgainButton();
    void BattleReportDidClickMainMenuButton();
}

public class BattleReportViewController : ViewController
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI textTopResult;
    [SerializeField] Text textBattleReport;
    [SerializeField] Text textScore;
    [SerializeField] Text textBaseRemaining;
    [SerializeField] Text textFriendlyFire;
    [SerializeField] Text textHitAccuracy;
    [SerializeField] Text textGearsCollected;
    [SerializeField] Text textMedalsEarned;
    [SerializeField] Button buttonPlayAgain;
    [SerializeField] Text textPlayAgain;
    [SerializeField] Button buttonMainMenu;

    [Header("View Model")]
    [SerializeField] BattleReportViewModel battleReportVM;

    [Header("UI Animations")]
    [SerializeField] AnimationClip clipPanelEndOfBattleReportSlideIn;
    [SerializeField] AnimationClip clipPanelEndOfBattleReportSlideOut;
    [SerializeField] Animation animationPanelEndOfBattleReport;

    public IBattleReportViewControllerDelegate delegateBattleReportVC;

    private void Start()
    {
        this.gameObject.SetActive(false);

        buttonPlayAgain.enabled = false;
        buttonMainMenu.enabled = false;
        buttonPlayAgain.onClick.AddListener(OnPlayAgainButtonClicked);
        buttonMainMenu.onClick.AddListener(OnMainMenuButtonClicked);
    }

    public override void Display()
    {
        base.Display();

        buttonPlayAgain.enabled = true;
        buttonMainMenu.enabled = true;
        TriggerShowPanelBattleReport();
    }

    public void UpdateWithResultAndBattleReport(string topResult, string battleReport)
    {
        textTopResult.text = topResult;
        textBattleReport.text = battleReport;
        textPlayAgain.text = GameConstants.GameplayState.TimeMarkReached ? "CONTINUE" : "PLAY AGAIN";
        UpdateUI();
    }

    public override void Hide()
    {
        buttonPlayAgain.enabled = false;
        buttonMainMenu.enabled = false;
        TriggerHidePanelBattleReport();

        base.Hide();
    }

    private void UpdateUI()
    {
        battleReportVM.UpdateDetails();

        textScore.text = battleReportVM.TextScore;
        textBaseRemaining.text = battleReportVM.TextBaseRemaining;
        textFriendlyFire.text = battleReportVM.TextFriendlyFire;
        textHitAccuracy.text = battleReportVM.TextHitAccuracy;
        textGearsCollected.text = battleReportVM.TextGearsCollected;
        textMedalsEarned.text = battleReportVM.TextMedalsEarned;

        if (GameConstants.GameplayState.TimeMarkReached)
        {
            textBattleReport.text = battleReportVM.TextBattleReport;
        }
    }

    // Animation Triggers
    private void TriggerShowPanelBattleReport()
    {
        animationPanelEndOfBattleReport.Play(clipPanelEndOfBattleReportSlideIn.name);
    }

    private void TriggerHidePanelBattleReport()
    {
        animationPanelEndOfBattleReport.Play(clipPanelEndOfBattleReportSlideOut.name);
    }

    private void OnPlayAgainButtonClicked()
    {
        if (delegateBattleReportVC != null)
        {
            delegateBattleReportVC.BattleReportDidClickPlayAgainButton();
        }
    }

    private void OnMainMenuButtonClicked()
    {
        if (delegateBattleReportVC != null)
        {
            delegateBattleReportVC.BattleReportDidClickMainMenuButton();
        }
    }
}

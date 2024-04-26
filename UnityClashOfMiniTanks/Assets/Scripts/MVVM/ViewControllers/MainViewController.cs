using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainViewController : MonoBehaviour, GameplayInterfaceDelegate, IntroDelegate
    , PauseViewControllerDelegate, ConfirmViewControllerDelegate, ITrophyRewardDelegate
    , IBattleReportViewControllerDelegate
{
    [Header("View Controllers")]
    [SerializeField] public GameplayInterfaceViewController gameplayInterfaceVC;
    [SerializeField] public PauseViewController pauseVC;
    [SerializeField] public IntroViewController introVC;
    [SerializeField] ConfirmViewController confirmVC;
    [SerializeField] BattleReportViewController battleReportVC;
    [SerializeField] TrophyRewardViewController trophyRewardVC;

    [Header("Curtains")]
    [SerializeField] Animator animatorImageCurtainTop;
    [SerializeField] Animator animatorImageCurtainBottom;

    [Header("Services")]
    [SerializeField] SpawnEnemyTanksManager spawnEnemyTanksManager;
    [SerializeField] SpawnFriendlyTanksManager spawnFriendlyTanksManager;
    [SerializeField] TimeKeeperManager timeKeeperManager;
    [SerializeField] MainTimerManager mainTimerManager;
    [SerializeField] MusicVolumeAdjustment musicVolumeAdjustment;
    [SerializeField] SceneLoadingManager sceneLoadingManager;

    private Stack<ViewController> _popupStack = new Stack<ViewController>();
    private bool isRestartSelected = false;

    private void Start()
    {
        gameplayInterfaceVC.delegateGameplayInterface = this;
        introVC.delegateIntro = this;
        pauseVC.delegatePauseVC = this;
        confirmVC.delegateConfirmVC = this;
        trophyRewardVC.delegateTrophyRewardVC = this;
        battleReportVC.delegateBattleReportVC = this;

        StartCoroutine(HideImageCurtainTopDelayed());
        animatorImageCurtainBottom.SetTrigger("TriggerHide");
    }

    private void SubscribeToEvent()
    {
        HealthPlayer.OnPlayerDestroyed += OnGameOverPlayerDestroyed;
        BaseCounter.OnAllBasesDestroyed += OnGameOverBaseDestroyed;
        MainTimerManager.OnTimeFinished += OnGameOverTimeFinished;
        HealthAI.OnFriendlyTankKilled += OnGameOverPlayerKilledAFriendly;
        HealthBase.OnBaseDestroyedByPlayer += OnGameOverPlayerDestroyedBase;
    }

    private void OnEnable()
    {
        SubscribeToEvent();
    }

    private void OnDisable()
    {
        HealthPlayer.OnPlayerDestroyed -= OnGameOverPlayerDestroyed;
        BaseCounter.OnAllBasesDestroyed -= OnGameOverBaseDestroyed;
        MainTimerManager.OnTimeFinished -= OnGameOverTimeFinished;
        HealthAI.OnFriendlyTankKilled -= OnGameOverPlayerKilledAFriendly;
        HealthBase.OnBaseDestroyedByPlayer -= OnGameOverPlayerDestroyedBase;
    }

    IEnumerator HideImageCurtainTopDelayed()
    {
        yield return new WaitForSeconds(3f);
        DisplayCurtainTop(false);
    }

    public void DisplayCurtainTop(bool display)
    {
        string trigger = display ? "TriggerShow" : "TriggerHide";
        animatorImageCurtainTop.SetTrigger(trigger);
    }

    private void ShowCurtainBottom()
    {
        animatorImageCurtainBottom.SetTrigger("TriggerShowHalf");
    }

    private void HideCurtainBottom()
    {
        animatorImageCurtainBottom.SetTrigger("TriggerHideHalf");
    }

    public void ShowCurtainBottomDelayed()
    {
        StartCoroutine(ShowCurtainBottomCoroutine());
    }

    IEnumerator ShowCurtainBottomCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        ShowCurtainBottom();
    }

    //*****************************************
    // Events
    //*****************************************
    private void OnGameOver()
    {
        GameConstants.GameplayState.GameOver = true;
        gameplayInterfaceVC.Hide();
        StartCoroutine(GameOverDelayed());
    }

    IEnumerator GameOverDelayed()
    {
        yield return new WaitForSeconds(2f);

        battleReportVC.Display();
        _popupStack.Clear();
        _popupStack.Push(battleReportVC);

        ShowCurtainBottom();
        timeKeeperManager.PauseAITanks(true);
        musicVolumeAdjustment.TurnDownMusic();
        mainTimerManager.PauseTimer();

        GameConstants.gameManager.GetComponent<SoundFXManager>().StopWarningLowHealth();
    }

    private void OnGameOverPlayerDestroyed()
    {
        battleReportVC.UpdateWithResultAndBattleReport("GAME OVER", "You have been destroyed!");
        GameConstants.missionOutcomeAudioManager.GetComponent<MissionOutcomeAudioManager>().PlayGameOverDelayed();
        OnGameOver();
        GameConstants.roundCounter = 1;
    }

    private void OnGameOverBaseDestroyed()
    {
        battleReportVC.UpdateWithResultAndBattleReport("GAME OVER", "All base were destroyed!");
        GameConstants.missionOutcomeAudioManager.GetComponent<MissionOutcomeAudioManager>().PlayGameOverDelayed();
        OnGameOver();
        GameConstants.roundCounter = 1;
    }

    private void OnGameOverPlayerKilledAFriendly()
    {
        battleReportVC.UpdateWithResultAndBattleReport("MISSION FAILED", "You killed a friendly tank!");
        GameConstants.missionOutcomeAudioManager.GetComponent<MissionOutcomeAudioManager>().PlayGameOverDelayed();
        OnGameOver();
        GameConstants.roundCounter = 1;
    }

    private void OnGameOverPlayerDestroyedBase()
    {
        battleReportVC.UpdateWithResultAndBattleReport("MISSION FAILED", "You destroyed a base!");
        GameConstants.missionOutcomeAudioManager.GetComponent<MissionOutcomeAudioManager>().PlayGameOverDelayed();
        OnGameOver();
        GameConstants.roundCounter = 1;
    }

    private void OnGameOverTimeFinished()
    {
        battleReportVC.UpdateWithResultAndBattleReport("VICTORY", "You survived!");
        OnGameOver();
        GameConstants.missionOutcomeAudioManager.GetComponent<MissionOutcomeAudioManager>().PlayVictory();
    }

    //*****************************************
    // Delegate methods
    //*****************************************

    // GameplayInterfaceVC delegate methods
    public void DidClickPauseButton()
    {
        GameConstants.GameplayState.GamePaused = true;
        pauseVC.Display();
        ShowCurtainBottom();
        timeKeeperManager.PauseAITanks(true);
        mainTimerManager.PauseTimer();

        musicVolumeAdjustment.TurnDownMusic();
        _popupStack.Push(pauseVC);
    }

    // IntroVC delegate methods
    public void IntroDidFinish()
    {
        this.gameplayInterfaceVC.Display();
        this.spawnEnemyTanksManager.StartAddingEnemies();
        this.spawnFriendlyTanksManager.StartAddingFriendlyTanks();
    }

    public void AddTankInRadar(Transform tank, bool isEnemy)
    {
        this.gameplayInterfaceVC.AddTankInRadar(tank, isEnemy);
    }

    // PauseVC delegate methods
    public void DidClickResumeButton()
    {
        this.gameplayInterfaceVC.Display();
        HideCurtainBottom();
        timeKeeperManager.PauseAITanks(false);
        mainTimerManager.ResumeTimer();
        musicVolumeAdjustment.TurnUpMusic();
        GameConstants.GameplayState.GamePaused = false;
        _popupStack.Clear();
    }

    public void DidClickRestartButton()
    {
        isRestartSelected = true;
        confirmVC.DisplayWithMessageType(ConfirmMessageType.Restart);
    }

    public void DidClickMainMenuButton()
    {
        isRestartSelected = false;
        confirmVC.DisplayWithMessageType(ConfirmMessageType.MainMenu);
    }

    // ConfirmVC delegate methods
    public void DidClickYesButton(ConfirmMessageType messageType)
    {
        if (GameConstants.GameplayState.HasTrophyReward)
        {
            confirmVC.Hide();
            trophyRewardVC.Display();
            GameConstants.gameManager.GetComponent<SoundFXManager>().PlayTrophyRewardMusic();
        }
        else
        {
            DisplayCurtainTop(true);
            switch (messageType)
            {
                case ConfirmMessageType.Restart:
                    sceneLoadingManager.RestartTheGame();
                    break;
                case ConfirmMessageType.MainMenu:
                    sceneLoadingManager.LoadMainMenuScene();
                    GameConstants.roundCounter = 1;
                    break;
                case ConfirmMessageType.Continue:
                    sceneLoadingManager.RestartTheGame();
                    GameConstants.roundCounter++;
                    break;
                default:
                    break;
            }
        }
    }

    public void DidClickNoButton()
    {
        confirmVC.Hide();
        var currentPopup = _popupStack.Peek();
        currentPopup.Display();
    }

    // TrophyRewardVC delegate method
    public void TrophyRewardDidClickOkButton()
    {
        DisplayCurtainTop(true);
        if (isRestartSelected)
        {
            GameConstants.roundCounter++;
            sceneLoadingManager.RestartTheGame();
        }
        else
        {
            sceneLoadingManager.LoadMainMenuScene();
        }
    }

    // BattleReportVC delegate methods
    public void BattleReportDidClickPlayAgainButton()
    {
        isRestartSelected = true;
        battleReportVC.Hide();

        if (GameConstants.GameplayState.TimeMarkReached)
        {
            confirmVC.DisplayWithMessageType(ConfirmMessageType.Continue);
        } else
        {
            confirmVC.DisplayWithMessageType(ConfirmMessageType.Restart);
        }
    }

    public void BattleReportDidClickMainMenuButton()
    {
        isRestartSelected = false;
        battleReportVC.Hide();
        confirmVC.DisplayWithMessageType(ConfirmMessageType.MainMenu);
    }
}

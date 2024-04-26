using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuNavigationManager : MonoBehaviour {

	public GameObject panelMainMenu; 
	public GameObject panelGameMode; 
	public GameObject panelGameModeContent;  
	public GameObject panelTopBottomButtonsContents; 
	public GameObject panelBottomButtons; 
	public GameObject panelStartButton; 
	public GameObject panelTextConfirmation; 
	public GameObject panelLeaderboardContents; 
	private string stringAdScene = "AdScene"; 
	private string stringLoadingScene = "LoadingScene";

	public Animator animatorImageSelectTankBG; 
	public Animator animatorPanelGameMode; 
	public Animator animatorPanelMainMenu; 
	public Animator animatorPanelTopBottomButtons; 
	public Animator animatorPanelBottomButtons; 
	public Animator animatorPanelStartButton; 
	public Animator animatorPanelTextConfirmation; 
	public Animator animatorImageCurtainTop;
	public Animator animatorPanelCollectedItems; 
	public Animator animatorPanelNoInternetConnection; 
	public Animator animatorPanelLeaderboard;
	public Animator animatorPanelGameInfo;

	public SelectTankManager selectTankManagerScript; 

	public enum BackButtonModes
	{
		SelectGameMode,
		ChooseYourTankMode,
		StartPlayingMode, 
		GameCenter
	}

	private BackButtonModes backButtonMode = BackButtonModes.SelectGameMode; 
	public AudioSource audioSource;
    public GameObject panelSelectedTankUnlockInfo;
	public Button buttonBack; 
	public Button buttonGameModeSurvival; 
	public Button buttonGenericStart; 
	public Button buttonConfirmTankSelectionYes;
	public Button buttonConfirmTankSelectionNo;
	public Button buttonConfirmTankUnlockYes;
	public Button buttonConfirmTankUnlockNo;
	public Button buttonBottomBack;
	public Button buttonBottomForward;
	public Button buttonBottomSelect;
	public Button buttonMainMenuPlay; 
	public Button buttonMainMenuLeaderboard;
	public Button buttonMainMenuSettings;
	public Button buttonLeaderboard;
	public Button buttonGameInfo; 

	public Text textSelectGameMode; 

	public Animation panelSocialMediaAnimation; 
	public AnimationClip panelSocialMediaSlideInClip;
	public AnimationClip panelSocialMediaSlideOutClip; 

    void Start()
	{
		Invoke ("TriggerPanelMainMenuEnterButtons",2.5f);
		Invoke ("PlayPanelSocialMediaSlideIn",2.8f);
		Invoke ("TriggerHideImageCurtainTop",2f);
	}
		
	public void TriggerShowImageCurtainTop()
	{
		animatorImageCurtainTop.SetTrigger ("TriggerShow"); 
	}
	
	void TriggerHideImageCurtainTop()
	{
		animatorImageCurtainTop.SetTrigger ("TriggerHide"); 
	}

	public void SetBackButtonMode_SelectGameMode()
	{
		textSelectGameMode.text = "SELECT GAME MODE"; 
		backButtonMode = BackButtonModes.SelectGameMode; 
	}

	public void SetBackButtonMode_ChooseYourTankMode()
	{
		backButtonMode = BackButtonModes.ChooseYourTankMode; 
	}

	public void SetBackButtonMode_StartPlayingMode()
	{
		backButtonMode = BackButtonModes.StartPlayingMode; 
	}

	public void SetBackButtonMode_GameCenter()
	{
		textSelectGameMode.text = "GAMECENTER"; 
		backButtonMode = BackButtonModes.GameCenter; 
	}

	public void BackButtonPressed()
	{
		if (backButtonMode == BackButtonModes.SelectGameMode) {
			buttonBack.enabled = false; 
			ShowThisPanel(panelMainMenu); 
			TriggerPanelTopBottomButtonsExitButton();
			TriggerPanelMainMenuEnterButtons();
			TriggerPanelGameModeExitButtons(); 
			HideThisPanel(panelGameModeContent);
			HideThisPanel(panelTopBottomButtonsContents);
			PlayPanelSocialMediaSlideIn (); 
		}
		else if (backButtonMode == BackButtonModes.ChooseYourTankMode) {
			buttonBack.enabled = false;
			Invoke("ReenableButtonBack",0.75f); 
			buttonGameModeSurvival.enabled = true; 
			ShowThisPanel(panelGameModeContent); 
			TriggerPanelTopBottomButtonsChooseYourTankToSelectGameMode();
			TriggerPanelGameModeEnterButtons(); 
			selectTankManagerScript.DismissCurrentSelectedTank(); 
			HideImageSelectTankBG(); 
			SetBackButtonMode_SelectGameMode(); 
			TriggerBottomButtonsHideButtons(); 
			HideThisPanel(panelBottomButtons);
            panelSelectedTankUnlockInfo.SetActive(false); 
			TriggerHidePanelCollectedItems();
        }
		else if (backButtonMode == BackButtonModes.StartPlayingMode) {
			buttonBack.enabled = false;
			Invoke("ReenableButtonBack",0.75f);
			CancelTankSelectionProcess();  
		}
		else if (backButtonMode == BackButtonModes.GameCenter) {
			buttonBack.enabled = false; 
			ShowThisPanel(panelMainMenu); 
			TriggerPanelTopBottomButtonsExitButton();
			TriggerPanelMainMenuEnterButtons();
			TriggerHidePanelLeaderboard(); 
			HideThisPanel(panelLeaderboardContents); 
			HideThisPanel(panelTopBottomButtonsContents); 
		}
	}

	void ReenableButtonBack()
	{
		buttonBack.enabled = true; 
	}

    public void CancelTankSelectionProcess()
    {
        ShowThisPanel(panelBottomButtons);
        TriggerBottomButtonsShowButtons();
        TriggerPanelTopBottomButtonsStartPlayingToChooseYourTank();
        SetBackButtonMode_ChooseYourTankMode();
        TriggerPanelStartButtonHide();
        HideThisPanel(panelStartButton);
        TriggerPanelTextConfirmationHideText();
        HideThisPanel(panelTextConfirmation);
    }

	public void HideTankSelectionUiWhenTheresNoInternetConnection()
	{
		TriggerPanelTopBottomButtonsNoInternetConnection (); 
		TriggerPanelStartButtonHide();
		HideThisPanel(panelStartButton);
		TriggerPanelTextConfirmationHideText();
		HideThisPanel(panelTextConfirmation);
		TriggerHidePanelCollectedItems (); 
	}

	public void UserConfirmedNoInternetConnection()
	{
		TriggerPanelTopBottomButtonsNoNetToStartPlaying ();
		ShowThisPanel (panelStartButton); 
		TriggerPanelStartButtonShow (); 
		ShowThisPanel (panelTextConfirmation);
		TriggerPanelTextConfirmationShowText ();
		TriggerHidePanelNoInternetConnection ();
		TriggerShowPanelCollectedItems (); 
	}

	public void TriggerPanelTextConfirmationShowText()
	{
		animatorPanelTextConfirmation.SetTrigger ("TriggerShowText"); 
	}

	public void TriggerPanelTextConfirmationHideText()
	{
		animatorPanelTextConfirmation.SetTrigger ("TriggerHideText"); 
	}

	public void InvokeTriggerPanelStartButtonShow()
	{
		Invoke ("TriggerPanelStartButtonShow",0.5f); 
	}

	void TriggerPanelStartButtonShow()
	{
		animatorPanelStartButton.SetTrigger ("TriggerShowButton"); 
	}

	public void TriggerPanelStartButtonHide()
	{
		animatorPanelStartButton.SetTrigger ("TriggerHideButton"); 
	}

	public void InvokeTriggerBottomButtonsShowButtons()
	{
		Invoke ("TriggerBottomButtonsShowButtons",0.7f);
	}

	void TriggerBottomButtonsShowButtons()
	{
		buttonBottomBack.enabled = true;
		buttonBottomForward.enabled = true;
		buttonBottomSelect.enabled = true; 
		animatorPanelBottomButtons.SetTrigger ("TriggerShowButtons"); 
	}

	public void TriggerBottomButtonsHideButtons()
	{
		animatorPanelBottomButtons.SetTrigger ("TriggerHideButtons"); 
	}

	void TriggerPanelTopBottomButtonsChooseYourTankToSelectGameMode()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerChooseYourTankToSelectGameMode"); 
	}

	public void InvokeTriggerPanelTopBottomButtonsSelectGameMode()
	{
		buttonBack.enabled = true; 
		Invoke ("TriggerPanelTopBottomButtonsSelectGameMode",0.20f); 
	}

	void TriggerPanelTopBottomButtonsSelectGameMode()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerSelectGameMode"); 
	}

	public void TriggerPanelTopBottomButtonsExitButton()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerExitButton"); 
	}

	public void TriggerPanelTopBottomButtonsChooseYourTank()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerChooseYourTank"); 
	}

	public void TriggerPanelTopBottomButtonsStartPlaying()
	{
		buttonGenericStart.enabled = true; 
		buttonConfirmTankSelectionYes.enabled = true;
		buttonConfirmTankSelectionNo.enabled = true;
		buttonConfirmTankUnlockYes.enabled = true;
		buttonConfirmTankUnlockNo.enabled = true;

		animatorPanelTopBottomButtons.SetTrigger ("TriggerStartPlaying"); 
	}

	public void TriggerPanelTopBottomButtonsStartPlayingToChooseYourTank()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerStartPlayingToChooseYourTank"); 
	}

	public void TriggerPanelTopBottomButtonsNoInternetConnection()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerNoInternetConnection"); 
	}

	public void TriggerPanelTopBottomButtonsNoNetToStartPlaying()
	{
		animatorPanelTopBottomButtons.SetTrigger ("TriggerNoNetToStartPlaying"); 
	}

	public void ShowThisPanel(GameObject showPanel)
	{
		showPanel.SetActive (true); 
	}

	public void HideThisPanel(GameObject hidePanel)
	{
		StartCoroutine (HideThisPanelDelay(hidePanel)); 
	}

	IEnumerator HideThisPanelDelay(GameObject hidePanel)
	{
		yield return new WaitForSeconds (0.75f); 

		hidePanel.SetActive (false);
	}

	public void InvokeTriggerPanelMainMenuEnterButtons()
	{
		Invoke ("TriggerPanelMainMenuEnterButtons",0.25f); 
	}

	public void TriggerPanelMainMenuEnterButtons()
	{
		buttonMainMenuPlay.enabled = true;
		buttonMainMenuLeaderboard.enabled = true; 
		buttonMainMenuSettings.enabled = true;

		animatorPanelMainMenu.SetTrigger ("TriggerEnterButtons"); 
	}

	public void TriggerPanelMainMenuExitButtons()
	{
		buttonMainMenuPlay.enabled = false;
		buttonMainMenuLeaderboard.enabled = false; 
		buttonMainMenuSettings.enabled = false;
		buttonLeaderboard.enabled = true;
		buttonGameInfo.enabled = true; 

		animatorPanelMainMenu.SetTrigger ("TriggerExitButtons"); 
	}

	public void InvokeTriggerPanelGameModeEnterButtons()
	{
		Invoke ("TriggerPanelGameModeEnterButtons",0.20f); 
	}

	public void TriggerPanelGameModeEnterButtons()
	{
		animatorPanelGameMode.SetTrigger ("TriggerEnterButtons"); 
	}

	public void TriggerPanelGameModeExitButtons()
	{
		animatorPanelGameMode.SetTrigger ("TriggerExitButtons"); 
	}

	public void ShowPanelMainMenu()
	{
		panelMainMenu.SetActive (true);
		panelGameMode.SetActive (false); 
	}

	public void ShowPanelGameMode()
	{
		panelMainMenu.SetActive(false); 
		panelGameMode.SetActive(true);
	}

	public void ShowPanelChooseTank()
	{
		//panelSelectTank.SetActive (true); 
//		panelLaunch.SetActive (false);
//		panelGameMode.SetActive (false); 
	}

	public void InvokeGoToAdScene()
	{
		Invoke("GoToAdScene",2f); 
	}

	public void GoToAdScene()
	{ 
		SceneManager.LoadScene (stringAdScene); 
	}

	public void InvokeGoToLoadingScene()
	{
		Invoke("GoToLoadingScene",2f); 
	}

	public void GoToLoadingScene()
	{
		SceneManager.LoadScene (stringLoadingScene); 
	}

	public void ShowImageSelectTankBG()
	{
		animatorImageSelectTankBG.SetTrigger ("TriggerShow"); 
	}

	public void HideImageSelectTankBG()
	{
		animatorImageSelectTankBG.SetTrigger ("TriggerHide"); 
	}

	public void TriggerShowPanelCollectedItems()
	{
		animatorPanelCollectedItems.SetTrigger ("TriggerShow"); 
	}

	public void TriggerHidePanelCollectedItems()
	{
		animatorPanelCollectedItems.SetTrigger ("TriggerHide"); 
	}

	public void TriggerShowPanelNoInternetConnection()
	{
		animatorPanelNoInternetConnection.gameObject.SetActive (true); 
		animatorPanelNoInternetConnection.SetTrigger ("TriggerShow"); 
	}
	
	void TriggerHidePanelNoInternetConnection()
	{
		animatorPanelNoInternetConnection.SetTrigger ("TriggerHide");
		HideThisPanel (animatorPanelNoInternetConnection.gameObject); 
	}

	public void TriggerShowPanelLeaderboard()
	{
		animatorPanelLeaderboard.gameObject.SetActive (true); 
		animatorPanelLeaderboard.SetTrigger ("TriggerShow"); 
	}
	
	public void TriggerHidePanelLeaderboard()
	{
		animatorPanelLeaderboard.SetTrigger ("TriggerHide");
		HideThisPanel (animatorPanelLeaderboard.gameObject); 
		ShowThisPanel(panelMainMenu);
		TriggerPanelMainMenuEnterButtons();
	}

	public void TriggerShowPanelGameInfo()
	{
		animatorPanelGameInfo.gameObject.SetActive (true); 
		animatorPanelGameInfo.SetTrigger ("TriggerShow"); 
	}
	
	public void TriggerHidePanelGameInfo()
	{
		animatorPanelGameInfo.SetTrigger ("TriggerHide");
		HideThisPanel (animatorPanelGameInfo.gameObject); 
		ShowThisPanel(panelMainMenu);
		TriggerPanelMainMenuEnterButtons();
	}

	public void PlayPanelSocialMediaSlideIn()
	{
		panelSocialMediaAnimation.Play (panelSocialMediaSlideInClip.name); 
	}

	public void PlayPanelSocialMediaSlideOut()
	{
		panelSocialMediaAnimation.Play (panelSocialMediaSlideOutClip.name); 
	}

    public void LinkToPrivacyPolicyPage()
    {
        Application.OpenURL("https://cybermash-interactive.ca/privacypolicy");
    }
}
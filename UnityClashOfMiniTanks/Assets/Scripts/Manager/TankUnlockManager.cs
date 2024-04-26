using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.IO; 

public class TankUnlockManager : MonoBehaviour {

    private Tank selectedTank;
    private List<Tank> tankCollectionValues = new List<Tank>();
    public static int gearsInventory = 0; 
	public static int trophiesInventory = 0; 
	public static int medalsInventory = 0; 

	public Text textCollectedGears;
	public Text textCollectedMedals;
	public Text textCollectedTrophies;

	public CMTGameCenterManager gameCenterManagerScript; 

	public Text textGearsValue;
	public Text textMedalValue;
	public Text textTrophyValue;
    
    public MainMenuNavigationManager mainMenuNavigationManagerScript;
    public GameObject panelBottomButtons;
    public GameObject panelConfirmButtons;
    public GameObject panelTextConfirmation;
    public Text textConfirmation;

    public GameObject buttonGotIt;
    public GameObject panelConfirmTankSelectionButtons;
    public GameObject panelConfirmTankUnlockButtons;

    public Text textStartPlaying;

    private TankDataList tankDataList; 
	private ScoreAndCollectedItems scoreAndCollectedItemsData; 

	public Animator animatorPanelTankUnlockInfo;

	void Start()
	{

#if UNITY_EDITOR

		// load Tank data
		if (File.Exists(Path.Combine(Application.dataPath,
		                             GameConstants.kStringSavedTankDataEditor)))
		{
			
			tankDataList = XmlUtility.LoadFromXmlFile(Path.Combine(Application.dataPath,
			                                                       GameConstants.kStringSavedTankDataEditor));
		} else {
			
			tankDataList = XmlUtility.LoadFromXmlFile(Path.Combine(Application.dataPath,
			                                                       GameConstants.kStringTankData));
		}

		// Load score and collected items data
		if (File.Exists (Path.Combine (Application.dataPath, GameConstants.kStringScoreAndCollectedItemsDataUnityEditor))) {
			
			scoreAndCollectedItemsData = XmlUtility.LoadScoreAndCollectedItemsFromXmlFile (Path.Combine (Application.dataPath,
			                                                       GameConstants.kStringScoreAndCollectedItemsDataUnityEditor));

			gearsInventory = scoreAndCollectedItemsData.collectedGears;
			medalsInventory = scoreAndCollectedItemsData.collectedMedals;
			trophiesInventory = scoreAndCollectedItemsData.collectedTrophies;

			textCollectedGears.text = scoreAndCollectedItemsData.collectedGears.ToString();
			textCollectedMedals.text = scoreAndCollectedItemsData.collectedMedals.ToString(); 
			textCollectedTrophies.text = scoreAndCollectedItemsData.collectedTrophies.ToString();
		} else {
			
			textCollectedGears.text = gearsInventory.ToString();
			textCollectedMedals.text = medalsInventory.ToString(); 
			textCollectedTrophies.text = trophiesInventory.ToString();
		}
#else
		// Load Tank Data on mobile device
		if (File.Exists(Path.Combine(Application.persistentDataPath,
		                             GameConstants.kStringSavedTankDataMobileDevice)))
		{
			//Debug.Log("The SavedTankData file exists!");
			
			tankDataList = XmlUtility.LoadFromXmlFile(Path.Combine(Application.persistentDataPath,
			                                                       GameConstants.kStringSavedTankDataMobileDevice));
		}
		else
		{
			//Debug.Log("The SavedTankData file does not exist. Loading TankData from Resources folder...");
			
			TextAsset textAsset = (TextAsset) Resources.Load("TankData",typeof(TextAsset));
			tankDataList = XmlUtility.LoadFromXmlFileInResourcesFolder(textAsset);  
		}

		// Load score and collected items data
		if (File.Exists (Path.Combine (Application.persistentDataPath, GameConstants.kStringScoreAndCollectedItemsDataMobileDevice))) {
			//Debug.Log ("The SavedScoreAndCollectedItemsData file exists!");
			
			scoreAndCollectedItemsData = XmlUtility.LoadScoreAndCollectedItemsFromXmlFile (Path.Combine (Application.persistentDataPath,
			                                            					GameConstants.kStringScoreAndCollectedItemsDataMobileDevice));
			
			gearsInventory = scoreAndCollectedItemsData.collectedGears;
			medalsInventory = scoreAndCollectedItemsData.collectedMedals;
			trophiesInventory = scoreAndCollectedItemsData.collectedTrophies;
			
			textCollectedGears.text = scoreAndCollectedItemsData.collectedGears.ToString();
			textCollectedMedals.text = scoreAndCollectedItemsData.collectedMedals.ToString(); 
			textCollectedTrophies.text = scoreAndCollectedItemsData.collectedTrophies.ToString(); 

			// Report the Scores to GameCenter
			CMTGameCenterManager.savedTotalScore = scoreAndCollectedItemsData.totalScore; 
			CMTGameCenterManager.savedNumberOfEnemyKills = scoreAndCollectedItemsData.numberOfEnemyKills; 
			CMTGameCenterManager.savedSurvivalTime = scoreAndCollectedItemsData.survivalTime; 
			gameCenterManagerScript.ReportScoresToGameCenter(CMTGameCenterManager.savedTotalScore,CMTGameCenterManager.savedNumberOfEnemyKills,
				CMTGameCenterManager.savedSurvivalTime); 

		} else {
			textCollectedGears.text = gearsInventory.ToString();
			textCollectedMedals.text = medalsInventory.ToString(); 
			textCollectedTrophies.text = trophiesInventory.ToString();
		}
#endif

        foreach (var tank in tankDataList.Tanks)
        {
            tankCollectionValues.Add(tank);
        }

        selectedTank = tankCollectionValues[0]; 
    }

    public void SaveTankData()
    {
        foreach (var tank in tankCollectionValues)
        {
            tankDataList.Tanks.Add(tank);
        }

#if UNITY_EDITOR
        XmlUtility.SaveTankData(Path.Combine(Application.dataPath,
                            GameConstants.kStringSavedTankDataEditor),tankDataList);
#else
		XmlUtility.SaveTankData(Path.Combine(Application.persistentDataPath,
		                                     GameConstants.kStringSavedTankDataMobileDevice),tankDataList);
#endif
    }

	public void ShowTankStatus(int tankIndex)
	{
		SetCurrentSelectedTank (tankIndex); 
		if (selectedTank.isLocked) {
			Invoke("TriggerShowPanelTankUnlockInfo",0.5f); 
			Invoke("SelectedTankInfoUpdate",0.4f);
		} else {
			TriggerHidePanelTankUnlockInfo(); 
        }
	}

	public void SetCurrentSelectedTank(int tankIndex)
	{
		selectedTank = tankCollectionValues[tankIndex]; 
	}

	void SelectedTankInfoUpdate()
	{
        textGearsValue.text = selectedTank.gearValue.ToString();
		textMedalValue.text = selectedTank.medalValue.ToString(); 
		textTrophyValue.text = selectedTank.trophyValue.ToString();
	}

    public void ValidateTankSelection()
    {
        if (selectedTank.isLocked){
            UnlockTankConfirmation();
        } else {
            ConfirmTankSelection();
        }
    }

    void ConfirmTankSelection()
    {
        textStartPlaying.text = "START PLAYING"; 
        textConfirmation.text = "Are you sure you want to play this Tank on Survival Mode?";

        mainMenuNavigationManagerScript.TriggerBottomButtonsHideButtons();
        mainMenuNavigationManagerScript.HideThisPanel(panelBottomButtons);
        mainMenuNavigationManagerScript.TriggerPanelTopBottomButtonsStartPlaying();
        mainMenuNavigationManagerScript.SetBackButtonMode_StartPlayingMode();

        buttonGotIt.SetActive(false);
        panelConfirmTankUnlockButtons.SetActive(false);
        panelConfirmTankSelectionButtons.SetActive(true);  
        mainMenuNavigationManagerScript.ShowThisPanel(panelConfirmButtons);
        mainMenuNavigationManagerScript.InvokeTriggerPanelStartButtonShow();

        mainMenuNavigationManagerScript.ShowThisPanel(panelTextConfirmation);
        mainMenuNavigationManagerScript.TriggerPanelTextConfirmationShowText();      
    }

	void UnlockTankConfirmation()
	{
        textStartPlaying.text = "UNLOCK TANK";
		TriggerHidePanelTankUnlockInfo (); 

        if (gearsInventory < selectedTank.gearValue || medalsInventory < selectedTank.medalValue
            || trophiesInventory < selectedTank.trophyValue)
        {
            textConfirmation.text = "You do not have enough required items to unlock this Tank.";
            buttonGotIt.SetActive(true);
            panelConfirmTankUnlockButtons.SetActive(false);
            panelConfirmTankSelectionButtons.SetActive(false);
        }
        else
        {
            textConfirmation.text = "Are you sure you want to unlock this Tank?";
            buttonGotIt.SetActive(false);
            panelConfirmTankUnlockButtons.SetActive(true);
            panelConfirmTankSelectionButtons.SetActive(false);
        }

        mainMenuNavigationManagerScript.TriggerBottomButtonsHideButtons();
        mainMenuNavigationManagerScript.HideThisPanel(panelBottomButtons);
        mainMenuNavigationManagerScript.TriggerPanelTopBottomButtonsStartPlaying();
        mainMenuNavigationManagerScript.SetBackButtonMode_StartPlayingMode();

        mainMenuNavigationManagerScript.ShowThisPanel(panelTextConfirmation);
        mainMenuNavigationManagerScript.TriggerPanelTextConfirmationShowText();
        mainMenuNavigationManagerScript.ShowThisPanel(panelConfirmButtons);
        mainMenuNavigationManagerScript.InvokeTriggerPanelStartButtonShow();
    }

    public void UnlockTankProcess()
    {
        selectedTank.isLocked = false;

        gearsInventory -= selectedTank.gearValue;
        medalsInventory -= selectedTank.medalValue;
        trophiesInventory -= selectedTank.trophyValue;

        textCollectedGears.text = gearsInventory.ToString();
        textCollectedMedals.text = medalsInventory.ToString();
        textCollectedTrophies.text = trophiesInventory.ToString(); 

		TriggerHidePanelTankUnlockInfo (); 

        mainMenuNavigationManagerScript.CancelTankSelectionProcess();

        SaveTankData(); 
    }

	public void TriggerShowPanelTankUnlockInfo()
	{
		animatorPanelTankUnlockInfo.SetTrigger ("TriggerShow"); 
	}
	
	public void TriggerHidePanelTankUnlockInfo()
	{
		animatorPanelTankUnlockInfo.SetTrigger ("TriggerHide"); 
	}
}
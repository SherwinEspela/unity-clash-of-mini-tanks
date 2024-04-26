using UnityEngine;
using System.Collections;

public class CMTInAppPurchaseManager : MonoBehaviour {

	public bool isMainMenu = true; 
	private string stringLoadingScene = "LoadingScene"; 
	public const string INAPPPURCHASE_REMOVEADS =  "com.SherwinEspela.ClashOfMiniTanks.RemoveAds";
	private bool IsInitialized = false;
	public static string PlayerPrefsRemoveAdsValue = "PlayerPrefsRemoveAdsValue"; 

	// MainMenu UI
	public GameObject panelRequestingPurchase_mainMenu; 
	public GameObject panelPaymentComplete_mainMenu; 
	public GameObject panelPaymentFailed_mainMenu;

	// InAppPurchaseScene UI
	public GameObject panelRequestingPurchase_inAppPurchaseScene; 
	public GameObject panelPaymentComplete_inAppPurchaseScene; 
	public GameObject panelPaymentFailed_inAppPurchaseScene;

	// Use this for initialization
	void Start () {
		if (!isMainMenu) {
			this.InitializeInAppPurchase (); 
		}
	}

	public void InitializeInAppPurchase()
	{
		if (!IsInitialized) {
			IOSInAppPurchaseManager.Instance.AddProductId (INAPPPURCHASE_REMOVEADS);

			//IOSInAppPurchaseManager.OnTransactionStarted += OnTransactionStarted; 

			IOSInAppPurchaseManager.OnVerificationComplete += HandleOnVerificationComplete;
			IOSInAppPurchaseManager.OnStoreKitInitComplete += OnStoreKitInitComplete;
			IOSInAppPurchaseManager.OnTransactionComplete += OnTransactionComplete;
			IOSInAppPurchaseManager.OnRestoreComplete += OnRestoreComplete;

			IsInitialized = true;
		}

		IOSInAppPurchaseManager.Instance.LoadStore ();
	}
		
	public void GotoLoadingScene()
	{
		Application.LoadLevel (stringLoadingScene); 
	}
		
	public void BuyRemoveAd() {

		if(IOSInAppPurchaseManager.Instance.IsStoreLoaded){
			if (isMainMenu) {
				panelRequestingPurchase_mainMenu.SetActive (true); 
			} else {
				panelRequestingPurchase_inAppPurchaseScene.SetActive (true); 
			}

			IOSInAppPurchaseManager.Instance.BuyProduct(INAPPPURCHASE_REMOVEADS);
		} else {
				
			IOSNativePopUpManager.showMessage("Oooops!","Your request cannot be processed this time. Please try again later.");
		}
	}

	private void UnlockProducts(string productIdentifier) {
		Debug.Log ("UnlockProducts()"); 

		if (productIdentifier.Equals(INAPPPURCHASE_REMOVEADS)) {
			RemoveAds (); 
		}
	}

	private void RemoveAds()
	{
		Debug.Log ("UnlockProducts()");

		PlayerPrefs.SetInt (PlayerPrefsRemoveAdsValue, 1); 	
		PlayerPrefs.Save (); 

		Debug.Log ("PlayerPrefsRemoveAdsValue = " + PlayerPrefs.GetInt(PlayerPrefsRemoveAdsValue));

		if (isMainMenu) {
			panelRequestingPurchase_mainMenu.SetActive (false); 
			panelPaymentComplete_mainMenu.SetActive (true); 
		} else {
			panelRequestingPurchase_inAppPurchaseScene.SetActive (false); 
			panelPaymentComplete_inAppPurchaseScene.SetActive (true); 
		}
	}

	void HandleOnVerificationComplete (IOSStoreKitVerificationResponse response) {
		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.status.ToString());

		Debug.Log("ORIGINAL JSON: " + response.originalJSON);
	}

	void OnStoreKitInitComplete(ISN_Result result) {

		if(result.IsSucceeded) {

			int avaliableProductsCount = 0;
			string productTitle = "";

			foreach(IOSProductTemplate tpl in IOSInAppPurchaseManager.instance.Products) {
				productTitle = tpl.DisplayName;
				if(tpl.IsAvaliable) {
					avaliableProductsCount++;
				}
			}

			//IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount);
			IOSNativePopUpManager.showMessage("StoreKit Init Successful", "Available products title: " + productTitle);
			Debug.Log("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);
		} else {
			IOSNativePopUpManager.showMessage("StoreKit Init Failed",  "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		}
	}

	private void OnTransactionStarted(string value){
		Debug.Log("OnTransactionStarted...");

		if (isMainMenu) {
			panelRequestingPurchase_mainMenu.SetActive (true); 
		} else {
			panelRequestingPurchase_inAppPurchaseScene.SetActive (true); 
		}
	}

	private void OnTransactionComplete (IOSStoreKitResult result) {

		Debug.Log("OnTransactionComplete: " + result.ProductIdentifier);
		Debug.Log("OnTransactionComplete: state: " + result.State);

		switch(result.State) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts(result.ProductIdentifier);
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock intrefase and repor user that the purchase is failed. 
			Debug.Log ("Transaction failed with error, code: " + result.Error.Code);
			Debug.Log ("Transaction failed with error, description: " + result.Error.Description);

			if (isMainMenu) {
				panelRequestingPurchase_mainMenu.SetActive (false); 
				panelPaymentFailed_mainMenu.SetActive (true);
			} else {
				panelRequestingPurchase_inAppPurchaseScene.SetActive (false); 
				panelPaymentFailed_inAppPurchaseScene.SetActive (true);
			}
			break;
		}

//		if(result.State == InAppPurchaseState.Failed) {
//			IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
//		} else {
//			IOSNativePopUpManager.showMessage("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString());
//		}

	}

	private void OnRestoreComplete (IOSStoreKitRestoreResult res) {
		if(res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Success", "Restore Completed");
		} else {
			IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Description);
		}
	}	
}

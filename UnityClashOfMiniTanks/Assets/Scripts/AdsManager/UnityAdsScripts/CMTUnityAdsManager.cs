using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public interface AdsManagerDelegate
{
    void DidReportPlatformIsSupported();
    void DidReportPlatformIsNotSupported();
    void DidReportAdIsPlaying();
    void DidReportResults(string result);
}

public class CMTUnityAdsManager : MonoBehaviour {

	private string stringLoadingScene = "LoadingScene"; 
	private string stringInAppPurchaseScene = "InAppPurchaseScene";

    public AdsManagerDelegate delegateAdsManager;
	
    public void DoUnityAds()
    {
        DoInitializeUnityAds();
        PlayUnityAdsVideo();
    }

	void DoInitializeUnityAds()
	{
		if (Advertisement.isSupported) {
            //Advertisement.allowPrecache = true;

            if (delegateAdsManager != null)
            {
                delegateAdsManager.DidReportPlatformIsSupported();
            }

#if UNITY_IOS
			Advertisement.Initialize(InitializeUnityAds.stringID_iOS,false);
#elif UNITY_ANDROID
			Advertisement.Initialize(InitializeUnityAds.stringID_Android,false); 
#endif

		} else {
			Debug.Log("Platform not supported");

            if (delegateAdsManager != null)
            {
                delegateAdsManager.DidReportPlatformIsNotSupported();
            }
        }
	}
	
	public void PlayUnityAdsVideo()
	{
		if (Advertisement.isInitialized) {
			ShowUnityAdvetisement(); 
		} else {
			DoInitializeUnityAds(); 
			ShowUnityAdvetisement(); 
		}
	}
	
	void ShowUnityAdvetisement()
	{
		string zoneName = string.Empty;
#if UNITY_IOS
		zoneName = "defaultVideoAndPictureZone"; 
#elif UNITY_ANDROID
		zoneName = "defaultVideoAndPictureZone";
#endif

		ShowOptions options = new ShowOptions ();
		options.resultCallback = AdCallbackhandler;

		if (Advertisement.IsReady(zoneName)) {
			Advertisement.Show (zoneName, options);

            if (delegateAdsManager != null)
            {
                delegateAdsManager.DidReportAdIsPlaying();
            }
        }
	}

	void AdCallbackhandler (ShowResult result)
	{
        string resultString = string.Empty;
		switch(result)
		{
		case ShowResult.Finished:
			Debug.Log ("Ad Finished. Rewarding player...");
                resultString = "Ad Finished. Rewarding player...";
                break;
		case ShowResult.Skipped:
			Debug.Log ("Ad skipped. Son, I am dissapointed in you");
                resultString = "Ad skipped. Son, I am dissapointed in you";
                break;
		case ShowResult.Failed:
			Debug.Log("I swear this has never happened to me before");
                resultString = "I swear this has never happened to me before";
                break;
		}

        if (delegateAdsManager != null)
        {
            delegateAdsManager.DidReportResults(resultString);
        }

        GoToLoadingScene ();
	}

	void GoToLoadingScene()
	{
        SceneManager.LoadScene(stringLoadingScene);
	}
}
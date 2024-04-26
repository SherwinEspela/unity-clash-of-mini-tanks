using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeUnityAds : MonoBehaviour {

	public const string stringID_iOS = "22173";
    public const string stringID_Android = "73999";

    // Use this for initialization
    void Start () {
		DoInitialize(); 
	}
	
	void DoInitialize()
	{
		if (Advertisement.isSupported) {
#if UNITY_ANDROID
			Advertisement.Initialize(stringID_Android,false);
#elif UNITY_IOS
			Advertisement.Initialize(stringID_iOS,false);
#endif
		} else {
			Debug.Log("Platform not supported"); 
		}
	}
}
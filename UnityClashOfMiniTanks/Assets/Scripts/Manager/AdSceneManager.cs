using UnityEngine;
using UnityEngine.SceneManagement;

public class AdSceneManager : MonoBehaviour
{
    [Header("View Controller")]
    [SerializeField] GameObject internetConnectionVC;

    [Header("Services")]
    [SerializeField] CMTUnityAdsManager unityAdsManager;

    private string stringLoadingScene = "LoadingScene";
    private string sceneMainMenu = "MainMenu";

    private void Start()
    {
        internetConnectionVC.SetActive(false);

#if UNITY_EDITOR
        GoToLoadingScene(); 
#else
		CheckInternetconnection();
#endif
    }

    private void CheckInternetconnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // has internet connection
            unityAdsManager.DoUnityAds();
        }
        else
        {
            // no internet connection
            internetConnectionVC.SetActive(true);
        }
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadSceneAsync(sceneMainMenu);
    }

    void GoToLoadingScene()
    {
        SceneManager.LoadScene(stringLoadingScene);
    }
}

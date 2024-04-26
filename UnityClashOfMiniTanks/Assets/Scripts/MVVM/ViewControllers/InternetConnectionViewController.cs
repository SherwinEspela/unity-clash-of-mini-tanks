using UnityEngine;

public class InternetConnectionViewController : ViewController
{
    [SerializeField] AnimationClip clipPanelNoInternetConnectionSlideIn;
    [SerializeField] AnimationClip clipPanelNoInternetConnectionSlideOut;
    [SerializeField] Animation animationPanelNoInternetConnection;

    public void TriggerShowPanelNoInternetConnection()
    {
        animationPanelNoInternetConnection.gameObject.SetActive(true);
        animationPanelNoInternetConnection.Play(clipPanelNoInternetConnectionSlideIn.name);
    }

    public void TriggerHidePanelNoInternetConnection()
    {
        animationPanelNoInternetConnection.Play(clipPanelNoInternetConnectionSlideOut.name);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class AdsDebugger : MonoBehaviour, AdsManagerDelegate
{
    [SerializeField] Text textDebugger1;
    [SerializeField] Text textDebugger2;
    [SerializeField] Text textDebugger3;
    [SerializeField] Text textDebugger4;

    [SerializeField] CMTUnityAdsManager adsManager;

    public void DidReportPlatformIsNotSupported()
    {
        textDebugger1.text = "Platform is not supported...";
    }

    public void DidReportPlatformIsSupported()
    {
        textDebugger2.text = "Platform is supported...";
    }

    public void DidReportAdIsPlaying()
    {
        textDebugger3.text = "Ad is playing...";
    }

    public void DidReportResults(string result)
    {
        textDebugger4.text = result;
    }
}

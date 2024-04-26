using UnityEngine;

public class ControlsGuideViewController : MonoBehaviour
{
    [SerializeField] GameObject guideShootCannon;
    [SerializeField] GameObject guideMoveTank;
    [SerializeField] GameObject guideRotateTurret;

    private bool guideMoveTankIsDisabled = false;
    private bool guideRotateTurretIsDisabled = false;
    private bool guideFireCannonIsDisabled = false;

    private void Start()
    {
        ShowGuides();
    }

    private void OnEnable()
    {
        HealthPlayer.OnPlayerDestroyed += HideGuides;
        BaseCounter.OnAllBasesDestroyed += HideGuides;
        MainTimerManager.OnTimeFinished += HideGuides;
    }

    private void OnDisable()
    {
        HealthPlayer.OnPlayerDestroyed -= HideGuides;
        BaseCounter.OnAllBasesDestroyed -= HideGuides;
        MainTimerManager.OnTimeFinished -= HideGuides;
    }

    public void HideGuides()
    {
        guideMoveTank.SetActive(false);
        guideRotateTurret.SetActive(false);
        guideShootCannon.SetActive(false);
    }

    public void ShowGuides()
    {
        if (GameConstants.roundCounter > 1)
        {
            return;
        }

        if (!guideMoveTankIsDisabled)
        {
            guideMoveTank.SetActive(true);
        }

        if (!guideRotateTurretIsDisabled)
        {
            guideRotateTurret.SetActive(true);
        }

        if (!guideFireCannonIsDisabled)
        {
            guideShootCannon.SetActive(true);
        }
    }

    public void HideGuideMoveTankByLeftJoystick()
    {
        guideMoveTank.SetActive(false);
        guideMoveTankIsDisabled = true;
    }

    public void HideGuideRotateTurretByRightJoystick()
    {
        guideRotateTurret.SetActive(false);
        guideRotateTurretIsDisabled = true;
    }

    public void HideGuideShootByTouchPad()
    {
        guideShootCannon.SetActive(false);
        guideFireCannonIsDisabled = true;
    }
}

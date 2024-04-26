using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public interface GameplayInterfaceDelegate
{
    void DidClickPauseButton();
}

public class GameplayInterfaceViewController : ViewController, ControlsDelegate
{
    [SerializeField] RadarMapViewController radarMapVC;
    [SerializeField] Button buttonPause;
    [SerializeField] GameObject timer;
    [SerializeField] Text textReloadingCannon;
    [SerializeField] GameObject healthBar;
    [SerializeField] JoystickLeft joystickLeft;
    [SerializeField] JoystickRight joystickRight;
    [SerializeField] TurretControlsTouchPad touchpadCannonFire;
    [SerializeField] GameObject score;
    [SerializeField] Animator animatorOtherDisplays;

    [SerializeField] ControlsGuideViewController controlsGuide;

    private Animator animatorTextReloadingCannon;
    private float playerCannonReloadSpeed;

    public GameplayInterfaceDelegate delegateGameplayInterface;

    private void Start()
    {
        this.gameObject.SetActive(false);
        textReloadingCannon.gameObject.SetActive(false);
        animatorTextReloadingCannon = this.textReloadingCannon.gameObject.GetComponent<Animator>();
        playerCannonReloadSpeed = GameConstants.kPlayerReloadSpeedNormal;

        joystickLeft.delegateControls = this;
        joystickRight.delegateControls = this;
        touchpadCannonFire.delegateControls = this;

        buttonPause.onClick.AddListener(OnPauseButtonClicked);
    }

    void OnPauseButtonClicked()
    {
        this.Hide();

        if (delegateGameplayInterface != null)
        {
            delegateGameplayInterface.DidClickPauseButton();
        }
    }

    public override void Display()
    {
        base.Display();

        EnableControls();
        this.controlsGuide.ShowGuides();
        this.radarMapVC.ShowRadarMap();
        this.ShowOtherDisplays();
        controlsGuide.ShowGuides();
        buttonPause.enabled = true;
    }

    public override void Hide()
    {
        DisableControls();
        this.controlsGuide.HideGuides();
        this.radarMapVC.HideRadarMap();
        this.HideOtherDisplays();
        controlsGuide.HideGuides();
        buttonPause.enabled = false;

        base.Hide();
    }

    private void EnableControls()
    {
        joystickLeft.DisplayJoystick();
        joystickRight.DisplayJoystick();
        touchpadCannonFire.GetComponent<ETCTouchPad>().activated = true;
        textReloadingCannon.gameObject.SetActive(true);
    }

    private void DisableControls()
    {
        joystickLeft.HideJoystick();
        joystickRight.HideJoystick();
        touchpadCannonFire.GetComponent<ETCTouchPad>().activated = false;
        textReloadingCannon.gameObject.SetActive(false);
    }

    public void SetFasterReloadSpeed(float speedValue, bool faster = false)
    {
        playerCannonReloadSpeed = speedValue;
        textReloadingCannon.color = faster ? Color.red : Color.white;
    }

    void ReloadPlayerCannon()
    {
        touchpadCannonFire.GetComponent<ETCTouchPad>().activated = false;
        textReloadingCannon.text = "RELOADING";
        textReloadingCannon.color = Color.red;
        animatorTextReloadingCannon.SetTrigger("TriggerPulsing");
        StartCoroutine(ProcessCannonReloadCoroutine());
    }

    IEnumerator ProcessCannonReloadCoroutine()
    {
        yield return new WaitForSeconds(playerCannonReloadSpeed);

        animatorTextReloadingCannon.SetTrigger("TriggerNotPulsing");
        touchpadCannonFire.GetComponent<ETCTouchPad>().activated = true;
        textReloadingCannon.text = "READY TO SHOOT";
        textReloadingCannon.color = Color.white;

        GameConstants.PlayerTankState.CanFireCannon = true;
    }

    private void ShowOtherDisplays()
    {
        animatorOtherDisplays.SetTrigger("TriggerShowHud");
    }

    private void HideOtherDisplays()
    {
        animatorOtherDisplays.SetTrigger("TriggerHideHud");
    }

    // Delegate methods
    public void AddTankInRadar(Transform tank, bool isEnemy)
    {
        this.radarMapVC.AddTankInRadar(tank, isEnemy);
    }

    // Controls delegate methods
    public void DidTouchLeftJoystick()
    {
        controlsGuide.HideGuideMoveTankByLeftJoystick();
    }

    public void DidTouchRightJoystick()
    {
        controlsGuide.HideGuideRotateTurretByRightJoystick();
    }

    public void DidTouchTheTouchPad()
    {
        controlsGuide.HideGuideShootByTouchPad();

        ReloadPlayerCannon();
    }
}

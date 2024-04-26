using UnityEngine;

public class JoystickLeft : JoystickControls
{
    [SerializeField] int playerTankMovementSpeed = 6;

    public Transform PlayerTank { get; set; }
    public static bool tankIsMoving = false;
    public static int tankSpeed;

    void Start()
    {
        tankSpeed = playerTankMovementSpeed;
        joystickScript = this.gameObject.GetComponent<ETCJoystick>();
        joystickScript.onTouchStart.AddListener(OnLeftJoystickTouched);
    }

    void Update()
    {
        UpdateAngleValue();
        // UpdateTankSpeed ();
    }

    public override void UpdateAngleValue()
    {
        base.UpdateAngleValue();

        if (this.PlayerTank)
        {
            this.PlayerTank.localEulerAngles = new Vector3(PlayerTank.localRotation.x, angleValue, PlayerTank.localRotation.z);
        }
    }

    /*
    void UpdateTankSpeed()
    {
        float axisVal = 0.5f;
        bool isNormalSpeed = joystickScript.axisX.axisValue > axisVal || joystickScript.axisX.axisValue < -axisVal ||
            joystickScript.axisY.axisValue > axisVal || joystickScript.axisY.axisValue < -axisVal;
        tankSpeed = isNormalSpeed ? playerTankMovementSpeed : playerTankMovementSpeed - 2;
    }
    */

    public void On_JoystickMove(){  
        if (!GameConstants.GameplayState.GameOver) {
          PlayerTank.Translate (Vector3.forward * Time.deltaTime * tankSpeed);    
        }
    }

    public void On_JoystickMoveStart(){
        if (!GameConstants.GameplayState.GameOver) {
          MoveTrackScript.gearStatus = 1;
          tankIsMoving = true;
        }
    }

    public void On_JoystickMoveEnd(){
        if (!GameConstants.GameplayState.GameOver) {
          StopTheTankConfig (); 
        }
    }

    public void StopTheTankConfig()
    {
        MoveTrackScript.gearStatus = 0;
        tankIsMoving = false;
    }

    private void OnLeftJoystickTouched()
    {
        if (delegateControls != null)
        {
            delegateControls.DidTouchLeftJoystick();
        }
    }
}

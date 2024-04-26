using System.Collections;
using UnityEngine;

public class JoystickRight : JoystickControls
{
    private Transform playerTankTurret;
    private Vector3 defaultRotationValue;
    private bool shouldUpdateRotation = false;

    public Transform PlayerTankTurret {
        get
        {
            return playerTankTurret;
        }
        set
        {
            playerTankTurret = value;
            defaultRotationValue = playerTankTurret.localEulerAngles;
        }
    }
    
    void Start()
    {
        joystickScript = this.gameObject.GetComponent<ETCJoystick>();
        joystickScript.onTouchStart.AddListener(OnRightJoystickTouched);
    }

    private void Update()
    {
        UpdateAngleValue();
    }

    public override void UpdateAngleValue()
    {
        if (!shouldUpdateRotation)
        {
            return;
        }

        base.UpdateAngleValue();

        if (this.PlayerTankTurret)
        {
            this.PlayerTankTurret.eulerAngles = new Vector3(0, angleValue, 0);
        }
    }

    // TODO: code cleanup
    public void OnJoystickMove()
    {
        // this.shouldUpdateRotation = true;
        // StopCoroutine(SetTurretToDefaultRotation());
        // StopAllCoroutines();
    }

    public void OnJoystickMoveStart()
    {
        this.shouldUpdateRotation = true;
        // StopCoroutine(SetTurretToDefaultRotation());
        // StopAllCoroutines();
    }

    public void OnJoystickTouchStart()
    {
        this.shouldUpdateRotation = true;
        // StopCoroutine(SetTurretToDefaultRotation());
        // StopAllCoroutines();
    }

    public void OnJoystickTouchUp()
    {
        // StartCoroutine(SetTurretToDefaultRotation());
    }

    /*
    IEnumerator SetTurretToDefaultRotation()
    {
        yield return new WaitForSeconds(5.0f);

        this.shouldUpdateRotation = false;
        PlayerTankTurret.localEulerAngles = this.defaultRotationValue;
    }
    */

    private void OnRightJoystickTouched()
    {
        if (delegateControls != null)
        {
            delegateControls.DidTouchRightJoystick();
        }
    }
}

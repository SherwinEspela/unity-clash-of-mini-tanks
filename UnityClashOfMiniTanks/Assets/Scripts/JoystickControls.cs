using UnityEngine;
using System.Collections;

public interface ControlsDelegate
{
    void DidTouchLeftJoystick();
    void DidTouchRightJoystick();
    void DidTouchTheTouchPad();
}

public class JoystickControls : MonoBehaviour
{
    protected float axisX;
    protected float axisY;
    protected float angleValue;
    protected ETCJoystick joystickScript;

    [SerializeField] float positionOffsetX_display = 10.0f;
    [SerializeField] float positionOffsetX_hide = -200.0f;
    [SerializeField] float positionOffsetMoveSpeed = 8f;

    private float positionOffsetY = 10f;

    public ControlsDelegate delegateControls;

    void Start()
    {
        joystickScript = this.gameObject.GetComponent<ETCJoystick>();
    }

    public virtual void UpdateAngleValue()
    {
        axisX = joystickScript.axisX.axisValue;
        axisY = joystickScript.axisY.axisValue;

        float result = axisX / axisY;
        double radians = Mathf.Atan(result);
        float degreeAngle = (float)(radians * (180 / Mathf.PI));

        if ((axisX > 0) && (axisY > 0))
        {
            angleValue = degreeAngle;
        }

        else if ((axisX < 0) && (axisY > 0))
        {
            angleValue = degreeAngle;
        }

        else if ((axisX < 0) && (axisY < 0))
        {
            angleValue = degreeAngle + 180f;
        }

        else if ((axisX > 0) && (axisY < 0))
        {
            angleValue = degreeAngle + 180f;
        }
    }

    public void DisplayJoystick()
    {
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(JoystickMoveDisplayCoroutine());
    }

    public void HideJoystick()
    {
        this.joystickScript.activated = false;
        StopAllCoroutines();
        StartCoroutine(JoystickMoveHideCoroutine());
    }

    IEnumerator JoystickMoveDisplayCoroutine()
    {
        float offset = 0.02f;
        while (this.joystickScript.anchorOffet.x < positionOffsetX_display - offset)
        {
            this.joystickScript.anchorOffet = Vector2.Lerp(this.joystickScript.anchorOffet, new Vector2(positionOffsetX_display, positionOffsetY), Time.deltaTime * positionOffsetMoveSpeed);
            yield return null;
        }

        this.joystickScript.activated = true;
    }

    IEnumerator JoystickMoveHideCoroutine()
    {
        float offset = 0.02f;
        while (this.joystickScript.anchorOffet.x > positionOffsetX_hide + offset)
        {
            this.joystickScript.anchorOffet = Vector2.Lerp(this.joystickScript.anchorOffet, new Vector2(positionOffsetX_hide, positionOffsetY), Time.deltaTime * positionOffsetMoveSpeed);
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}

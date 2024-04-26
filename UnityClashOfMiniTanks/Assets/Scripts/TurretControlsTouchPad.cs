using UnityEngine;

public class TurretControlsTouchPad : MonoBehaviour
{
    public TurretControls TurretControlsScript { get; set; }
    public ControlsDelegate delegateControls;

    public void OnTouchStart()
    {
        TurretControlsScript.Fire();

        if (delegateControls != null)
        {
            delegateControls.DidTouchTheTouchPad();
        }
    }
}

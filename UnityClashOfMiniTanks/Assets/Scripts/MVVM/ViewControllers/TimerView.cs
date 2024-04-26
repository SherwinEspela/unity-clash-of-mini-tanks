using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] Text textTimer;

    [SerializeField] MainTimerManager mainTimerManager;

    private void Update()
    {
        if (GameConstants.GameplayState.GamePaused)
        {
            return;
        }

        textTimer.text = mainTimerManager.TimeValue;
    }
}

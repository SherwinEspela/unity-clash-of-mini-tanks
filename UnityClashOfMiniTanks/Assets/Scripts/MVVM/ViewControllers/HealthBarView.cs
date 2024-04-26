using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] Image imageHealthColor;
    [SerializeField] Animator animatorImageHealthColor;
    [SerializeField] Animator animatorImageHealthIndicatorGlow;
    [SerializeField] GameObject imageIndicatorRed;
    [SerializeField] GameObject imageGlowRed;

    private void OnEnable()
    {
        HealthPlayer.OnPlayerCritical += OnPlayerHealthCritical;
        HealthPlayer.OnPlayerNormal += OnPlayerHealthNormal;
        HealthPlayer.OnPlayerHealthUpdated += UpdateSliderPlayerHealthValue;
    }

    private void OnDisable()
    {
        HealthPlayer.OnPlayerCritical -= OnPlayerHealthCritical;
        HealthPlayer.OnPlayerNormal -= OnPlayerHealthNormal;
        HealthPlayer.OnPlayerHealthUpdated -= UpdateSliderPlayerHealthValue;
    }

    void UpdateSliderPlayerHealthValue(float newValue)
    {
        imageHealthColor.fillAmount = newValue / 100;
    }

    void OnPlayerHealthCritical()
    {
        TriggerFillPlayerHealthBlinking();
        TriggerImageHealthIndicatorGlowCritical();
    }

    void OnPlayerHealthNormal()
    {
        TriggerFillPlayerHealthNormal();
        TriggerImageHealthIndicatorGlowNormal();
    }

    void TriggerFillPlayerHealthNormal()
    {
        animatorImageHealthColor.SetTrigger("TriggerNormal");
    }

    void TriggerFillPlayerHealthBlinking()
    {
        animatorImageHealthColor.SetTrigger("TriggerBlinking");
    }

    void TriggerImageHealthIndicatorGlowNormal()
    {
        imageIndicatorRed.SetActive(false);
        imageGlowRed.SetActive(false);
        animatorImageHealthIndicatorGlow.SetTrigger("TriggerNormal");
    }

    void TriggerImageHealthIndicatorGlowCritical()
    {
        imageIndicatorRed.SetActive(true);
        imageGlowRed.SetActive(true);
        animatorImageHealthIndicatorGlow.SetTrigger("TriggerCritical");
    }
}

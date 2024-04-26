using UnityEngine;
using UnityEngine.UI;

public interface BaseHealthBarDelegate
{
    void BaseHealthBarNotCritical();
}

public class BaseHealthBar : MonoBehaviour
{
    [SerializeField] Image imageHealthBaseScript;

    private Animator imageHealthBaseAnimator;

    public float InititialHealth { get; set; }

    public BaseHealthBarDelegate delegateBaseHealthBar;

    void Start()
    {
        imageHealthBaseAnimator = imageHealthBaseScript.gameObject.GetComponent<Animator>();
    }

    public void UpdateHealthBar(float currentHealth)
    {
        imageHealthBaseScript.fillAmount = currentHealth / InititialHealth;
        if (currentHealth > 25)
        {
            EnableHealthBaseAnimator(false);
            if (delegateBaseHealthBar != null)
            {
                delegateBaseHealthBar.BaseHealthBarNotCritical();
            }
        }
    }

    public void EnableHealthBaseAnimator(bool isEnabled)
    {
        imageHealthBaseAnimator.SetBool("imageHealthBaseIsCritical", isEnabled);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

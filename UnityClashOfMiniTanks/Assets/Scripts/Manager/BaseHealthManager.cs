using UnityEngine;

public class BaseHealthManager : MonoBehaviour
{
    [SerializeField] int startingBaseHealth = 200;
    [SerializeField] int healthIncreasePowerUp = 50;
    [SerializeField] HealthBase[] basesHealth;

    void Start()
    {
        foreach (var baseHealth in basesHealth)
        {
            baseHealth.health = startingBaseHealth;
            baseHealth.SetInitialHealthBarValue(startingBaseHealth);
        }
    }

    public void RepairAll()
    {
        foreach (var baseHealth in basesHealth)
        {
            baseHealth.health += healthIncreasePowerUp;
            if (baseHealth.health > startingBaseHealth)
            {
                baseHealth.health = startingBaseHealth;
            }
            baseHealth.UpdateHealthBar();
        }
    }

    public void HideNavMeshGeos()
    {
        foreach (var item in basesHealth)
        {
            item.HideNavBaseGeo();
        }
    }
}

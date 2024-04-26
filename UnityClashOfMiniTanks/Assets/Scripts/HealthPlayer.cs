using UnityEngine;

public class HealthPlayer : Health {

	public bool hasPowerUpShield = false; 
	public ParticleSystem tankDamageFX;

    private bool powerUpShieldIsActive = false;
    private GameObject shieldFx;
    private GameObject healthFx;
    
    // event
    public delegate void HealthPlayerEvent();
    public static event HealthPlayerEvent OnPlayerDestroyed;
    public static event HealthPlayerEvent OnPlayerCritical;
    public static event HealthPlayerEvent OnPlayerNormal;

    public delegate void HealthPlayerUpdatedEvent(float newValue);
    public static event HealthPlayerUpdatedEvent OnPlayerHealthUpdated;

    public void ReducePlayerHealthValue(float damage)
	{
        if (GameConstants.GameplayState.GameOver)
        {
            return;
        }

        if (hasPowerUpShield)
        {
            return;
        }

        health -= damage;

        // update the player health slider value
        if (OnPlayerHealthUpdated != null)
        {
            OnPlayerHealthUpdated(health);
        }

        if (health <= 0f)
        {
            GameConstants.GameplayState.MissionFailed = true;

            SoundFXManager sfxManager = this.gameManager.GetComponent<SoundFXManager>();
            sfxManager.PlayExplosionSound();
            sfxManager.StopWarningLowHealth();

            this.gameManager.GetComponent<ExploderManager>().DoExplode(this.gameObject);

            if (OnPlayerDestroyed != null)
            {
                OnPlayerDestroyed();
            }
        }

        else if (health <= 25f)
        {
            GameConstants.gameManager.GetComponent<SoundFXManager>().PlayWarningLowHealth();
            GameConstants.gameManager.GetComponent<VoiceResponseManager>().PlayPlayerShot();
            tankDamageFX.gameObject.SetActive(true);
            tankDamageFX.Play();

            if (OnPlayerCritical != null)
            {
                OnPlayerCritical();
            }
        }

        else
        {
            GameConstants.gameManager.GetComponent<VoiceResponseManager>().PlayPlayerShot();

            if (OnPlayerNormal != null)
            {
                OnPlayerNormal();
            }
        }
    }

	public void IncreasePlayerHealthValue(float healthIncrease)
	{
		health += healthIncrease;

		if (health > 100f) {
			health = 100f; 
		}

		if (health > 25f) {
            GameConstants.gameManager.GetComponent<SoundFXManager>().StopWarningLowHealth();

            if (OnPlayerNormal != null)
            {
                OnPlayerNormal();
            }

            tankDamageFX.Stop(); 
			tankDamageFX.gameObject.SetActive(false);            
        }

		// update the player health slider value
        if (OnPlayerHealthUpdated != null)
        {
            OnPlayerHealthUpdated(health);
        }

        var fxType = EffectsType.PowerUpHealthFx;
        Transform clone = this.gameManager.GetComponent<EffectsManager>().SpawnEffectsWithoutDespawn(fxType, this.transform);
        clone.parent = this.transform;
        clone.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
        HealthFxScript hfs = clone.gameObject.GetComponent<HealthFxScript>();
        hfs.Display();
        hfs.OnHealthFxHidden += DespawnHealthFx;
        this.healthFx = clone.gameObject;
    }

	public void SetPlayerPowerUpShield(bool value)
	{
        hasPowerUpShield = value;
        
		if (hasPowerUpShield) {
            if (powerUpShieldIsActive)
            {
                return;
            }

            powerUpShieldIsActive = true;
            var fxType = EffectsType.PowerUpShieldFx;
            Transform clone = this.gameManager.GetComponent<EffectsManager>().SpawnEffectsWithoutDespawn(fxType, this.transform);
            clone.parent = this.transform;
            clone.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
            clone.gameObject.GetComponent<ShieldFx>().Display();
            this.shieldFx = clone.gameObject;
        }
        else {
            ShieldFx shieldFxScript = this.shieldFx.GetComponent<ShieldFx>();
            shieldFxScript.OnShieldFxHidden += DespawnShieldFx;
            shieldFxScript.HideShield();
            powerUpShieldIsActive = false;
        }
    }

    private void DespawnShieldFx()
    {
        ShieldFx shieldFxScript = this.shieldFx.GetComponent<ShieldFx>();
        shieldFxScript.OnShieldFxHidden -= DespawnShieldFx;
        this.gameManager.GetComponent<EffectsManager>().DespawnEffectsDelayed(EffectsType.PowerUpShieldFx, this.shieldFx, 0.5f);
    }

    private void DespawnHealthFx()
    {
        HealthFxScript hfs = this.healthFx.gameObject.GetComponent<HealthFxScript>();
        hfs.OnHealthFxHidden -= DespawnHealthFx;
        this.gameManager.GetComponent<EffectsManager>().DespawnEffectsDelayed(EffectsType.PowerUpHealthFx, this.healthFx, 0.5f);
    }
}

using UnityEngine;
using System.Collections;

public class HealthBase : Health, BaseHealthBarDelegate
{	
	[SerializeField] CameraVisibility cameraVisibilityScript;
    [SerializeField] GameObject baseGeo;
    [SerializeField] GameObject navMeshGeo;
    [SerializeField] GameObject baseTarget; 
    [SerializeField] BaseTargetInfo baseTargetInfo;
    [SerializeField] ParticleSystem damageFX;
    [SerializeField] BaseHealthBar baseHealthBar;
    [SerializeField] string baseName;

    public bool HasBaseShieldPowerUp { get; set; }

    private bool sirenHasNotBeenPlayed = true;

    // events
    public delegate void BaseDestroyedDelegate();
    public event BaseDestroyedDelegate OnBaseDestroyedByThisTank;
    public static event BaseDestroyedDelegate OnBaseDestroyed;
    public static event BaseDestroyedDelegate OnBaseDestroyedByPlayer;

    void Start()
    {
        HasBaseShieldPowerUp = false;

        baseHealthBar.delegateBaseHealthBar = this;  
    }

    public void SetInitialHealthBarValue(float amount)
    {
        baseHealthBar.InititialHealth = amount;
    }

    public void ReduceHealthValue(float damage, bool isShotByPlayer)
	{
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        if (HasBaseShieldPowerUp) {
            return;
		}

        health -= damage;
        baseHealthBar.UpdateHealthBar(health);

        if (health <= 0)
        {
            GameConstants.gameManager.GetComponent<VoiceResponseManager>().PlayBaseDestroyed();

            baseTarget.SetActive(false);
            baseHealthBar.Hide();
            baseTargetInfo.BaseCriticalRadarMapIcon.SetActive(false);
            baseTargetInfo.IsDestroyed = true;
            damageFX.Stop();
            damageFX.gameObject.SetActive(false);
            GameConstants.gameManager.GetComponent<SoundFXManager>().PlayBaseExplosionSound();

            if (isShotByPlayer && OnBaseDestroyedByPlayer != null)
            {
                GameConstants.GameplayState.MissionFailed = true;
                OnBaseDestroyedByPlayer();
            }
            else
            {
                if (OnBaseDestroyed != null) { OnBaseDestroyed(); }
                if (OnBaseDestroyedByThisTank != null) { OnBaseDestroyedByThisTank(); }
            }
            
            if (cameraVisibilityScript.isVisibleInCamera == true)
            {
                GameConstants.gameManager.GetComponent<ExploderManager>().DoExplodeBase(this.baseGeo);
                Invoke("HideBaseObjects", 12f);
            }
            else
            {
                GameConstants.gameManager.GetComponent<ExploderManager>().DoExplodeBaseFXOnly(this.transform);
                HideBaseObjects();
            }

            StartCoroutine(ShowBaseDestroyedEventMessageDelayed());
        }

        else if (health <= 35)
        {
            GameConstants.gameManager.GetComponent<VoiceResponseManager>().PlayBaseUnderAttack();

            baseHealthBar.EnableHealthBaseAnimator(true);
            baseTargetInfo.BaseCriticalRadarMapIcon.SetActive(true);
            damageFX.gameObject.SetActive(true);
            damageFX.Play();
            if (sirenHasNotBeenPlayed)
            {
                GameConstants.gameManager.GetComponent<SoundFXManager>().PlayWarningLowBaseHealth();
                sirenHasNotBeenPlayed = false;
            }
        }

        else
        {
            if (!isShotByPlayer)
            {
                GameConstants.gameManager.GetComponent<VoiceResponseManager>().PlayBaseUnderAttack();
            }

            baseTargetInfo.BaseAttackedRadarMapIcon.SetActive(true);
            Invoke("HideBaseAttackedRadarMapIcon", 4f);
        }
    }

	public void UpdateHealthBar()
    {
        baseHealthBar.UpdateHealthBar(health);
    }

    public void HideNavBaseGeo()
    {
        navMeshGeo.SetActive(false);
    }

    void HideBaseObjects()
	{ 
		baseGeo.SetActive (false);
	}

	void HideBaseAttackedRadarMapIcon()
	{
		baseTargetInfo.BaseAttackedRadarMapIcon.SetActive(false);
	}

	IEnumerator ShowBaseDestroyedEventMessageDelayed()
	{
        yield return new WaitForSeconds(2.0f);
        GameConstants.gameManager.GetComponent<EventMessageManager>().ShowEventMessage(baseName + " DESTROYED");
        this.gameObject.SetActive(false);
    }

    // BaseHealthBar delegate method
    public void BaseHealthBarNotCritical()
    {
        baseTargetInfo.BaseCriticalRadarMapIcon.SetActive(false);
        damageFX.Stop();
        damageFX.gameObject.SetActive(false);
        sirenHasNotBeenPlayed = true;
    }
}

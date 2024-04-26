using UnityEngine;
using CMT.AI;

public class HealthAI : Health
{
    public bool isEnemyTank;

    private SpawnFriendlyTanksManager spawnFriendlyTanksManagerScript; 
	private SpawnEnemyTanksManager spawnEnemyTanksManagerScript; 
	private PowerUpManager powerupManagerScript;
	private GameObject tankDamageFXGo;
	private ParticleSystem tankDamageFX;
	private const float kHealthToDisplayDamageFX = 22f;

	public CameraVisibility cameraVisibilityScript;
    public bool IsPowerUpTank = false;

	// Event
	public delegate void AITankDestroyedDelegate();
	public event AITankDestroyedDelegate OnAITankDestroyed;
    public static event AITankDestroyedDelegate OnPowerUpTankDestroyed;
    public static event AITankDestroyedDelegate OnFriendlyTankKilled;

    void Start()
	{
		this.gameManager = GameObject.Find("GameManager");
        this.powerupManagerScript = this.gameManager.GetComponent<PowerUpManager>();
		spawnFriendlyTanksManagerScript = gameManager.GetComponent<SpawnFriendlyTanksManager>(); 
		spawnEnemyTanksManagerScript = gameManager.GetComponent<SpawnEnemyTanksManager>();
		this.GetDamageFX ();
	}

	void GetDamageFX()
	{
		if (tankDamageFX != null) {
			return;
		}

		Transform tankBodyRotation = this.transform.Find (GameConstants.kTankBodyRotation);
		Transform body = tankBodyRotation.Find ("Body");
		Transform damageFXLocator = body.Find ("damageFXLocator");
		tankDamageFXGo = damageFXLocator.Find ("damageFX").gameObject;
		tankDamageFX = tankDamageFXGo.GetComponent<ParticleSystem>();
	}

	public void ReduceHealthValue(float damage, bool isShotByPlayer)
	{
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

		health -= damage;

		if (health <= 0) {
          
			if (cameraVisibilityScript.isVisibleInCamera) {
                this.gameManager.GetComponent<ExploderManager>().DoExplode(this.gameObject);
			}
            this.gameManager.GetComponent<SoundFXManager>().PlayExplosionSound();

            if (isEnemyTank)
            {
                this.gameObject.GetComponent<EnemyTankAI>().IsHidden = true;
            } else
            {
                this.gameObject.GetComponent<FriendlyTankAI>().IsHidden = true;
            }

            if (OnAITankDestroyed != null)
            {
                OnAITankDestroyed();
            }

            if (isEnemyTank)
            {
                if (isShotByPlayer == true && this.gameObject.activeInHierarchy)
                {
                    DropAPowerUp();
                    AddScore();
                }
                spawnEnemyTanksManagerScript.DespawnTank(this.gameObject);
            }
            else {                
                if (isShotByPlayer)
                {
                    GameConstants.gameManager.GetComponent<ScoreManager>().FriendlyFirePenalty();
                    GameConstants.GameplayState.MissionFailed = true;
                    if (OnFriendlyTankKilled != null)
                    {
                        OnFriendlyTankKilled();
                    }
                }

                if (this.IsPowerUpTank && OnPowerUpTankDestroyed != null)
                {
                    OnPowerUpTankDestroyed();
                }

                spawnFriendlyTanksManagerScript.DespawnTank(this.gameObject);
            }
        } 
		else if (health <= kHealthToDisplayDamageFX) {
			tankDamageFXGo.SetActive (true);
			tankDamageFX.Play ();
		}
		else {
			if (!isEnemyTank) {
                // friendly tank
                this.gameManager.GetComponent<VoiceResponseManager>().PlayFriendlyTankShot();
				if (isShotByPlayer) {
                    GameConstants.gameManager.GetComponent<ScoreManager>().FriendlyFirePenalty();
                }
			}
		}
	}

	void DropAPowerUp()
	{
        Vector3 powerUpPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z);
        GameObject powerupClone = powerupManagerScript.SpawnPowerUp(powerUpPosition, Quaternion.identity);

        if (!powerupClone.name.Contains("Empty"))
        {
            PowerUp puScript = powerupClone.GetComponent<PowerUp>();
            puScript.InvokeDespawnThePowerUp();
        }
    }

	void AddScore()
	{
        GameConstants.gameManager.GetComponent<ScoreManager>().AddEnemyKillScore();
		gameManager.SendMessage("PlayPlayerHappy",SendMessageOptions.DontRequireReceiver); // from VoiceResponseManager.cs
	}
}

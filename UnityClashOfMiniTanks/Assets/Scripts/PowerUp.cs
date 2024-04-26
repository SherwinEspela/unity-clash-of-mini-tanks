using UnityEngine;
using Chronos; 

public class PowerUp : MonoBehaviour {

	public bool isBeingDespawned = true; 
	public enum PowerUpType
	{
		PowerUpType_health1,
		PowerUpType_health2,
		PowerUpType_moveSpeed,
		PowerUpType_reloadSpeed,
		PowerUpType_shield,
		PowerUpType_slowEnemies,
		PowerUpType_baseRepair,
		PowerUpType_baseShield,
		PowerUpType_friendlyTank,
		PowerUpType_increasePlayerCannonPower,
		PowerUpType_gear
	}
	public PowerUpType selectPowerUpType; 
	private float rotateSpeed = 2f; 
	public GameObject powerUpCollideFX; 
	public Timeline timeline; 

	void Update()
	{
		this.transform.Rotate(0,rotateSpeed,0); 
	}

	public void InvokeDespawnThePowerUp()
	{
		isBeingDespawned = true; 
		timeline.Plan (7f,DespawnThePowerUp); 
	}

	void DespawnThePowerUp()
	{
		if (isBeingDespawned) {
            GameConstants.gameManager.GetComponent<PowerUpManager>().DespawnPowerUp(this.gameObject);
		}
	}

	public void ForceDespawn()
	{
		isBeingDespawned = false;
        GameConstants.gameManager.GetComponent<PowerUpManager>().DespawnPowerUp(this.gameObject);
    }

	void DoPowerUpProcess()
	{
		ForceDespawn();
		
		switch (selectPowerUpType) {
		case PowerUpType.PowerUpType_health1:
            GameConstants.gameManager.GetComponent<PowerUpManager>().IncreasePlayerHealth1();
			break; 
			
		case PowerUpType.PowerUpType_health2:
            GameConstants.gameManager.GetComponent<PowerUpManager>().IncreasePlayerHealth2();
			break;
			
		case PowerUpType.PowerUpType_moveSpeed:
			GameConstants.gameManager.SendMessage("IncreasePlayerMoveSpeed",SendMessageOptions.DontRequireReceiver); 
			break;
			
		case PowerUpType.PowerUpType_reloadSpeed:
			GameConstants.gameManager.SendMessage("IncreasePlayerReloadSpeed",SendMessageOptions.DontRequireReceiver); 
			break;
			
		case PowerUpType.PowerUpType_shield:
            GameConstants.gameManager.GetComponent<PowerUpManager>().AddPlayerShield();
			break;
			
		case PowerUpType.PowerUpType_slowEnemies:
			GameConstants.gameManager.SendMessage("SlowDownEnemySpeed",SendMessageOptions.DontRequireReceiver); 
			break;
			
		case PowerUpType.PowerUpType_baseRepair:
            GameConstants.gameManager.GetComponent<PowerUpManager>().RepairAllBase();
			break;
			
		case PowerUpType.PowerUpType_baseShield:
			GameConstants.gameManager.SendMessage("AddBaseShield",SendMessageOptions.DontRequireReceiver); 
			break;
			 
		case PowerUpType.PowerUpType_friendlyTank:
            GameConstants.gameManager.GetComponent<PowerUpManager>().AddFriendlyPowerUpTank();
			break;
			
		case PowerUpType.PowerUpType_increasePlayerCannonPower:
            GameConstants.gameManager.GetComponent<PowerUpManager>().IncreasePlayerCannonPower();
            break;
			
		case PowerUpType.PowerUpType_gear:
            GameConstants.gameManager.GetComponent<PowerUpManager>().GearCollected();
            break;
			
		default:
			break;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag.Equals(GameConstants.kTagPlayerTank)){
			if (!GameConstants.GameplayState.GameOver) {
				DoPowerUpProcess();
                GameConstants.gameManager.GetComponent<EffectsManager>().SpawnEffects(EffectsType.PowerUpFx, this.transform);
                GameConstants.gameManager.GetComponent<ScoreManager>().AddPowerUpPickScore();
			}
		}
	}
}

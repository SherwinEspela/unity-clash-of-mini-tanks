using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Chronos;
using Cybermash;

public interface PowerUpManagerDelegate
{
    void DidDisplayRewardMedal();
    void DidCollectGear();
}

public class PowerUpManager : MonoBehaviour {

    [SerializeField] GameObject textPowerUpMessagePrefab;

    [SerializeField] bool includeHealth1 = true;
    [SerializeField] GameObject health1PowerUpPrefab;
    [SerializeField] int quantityForHealth1 = 10;
    [SerializeField] bool includeHealth2 = true;
    [SerializeField] GameObject health2PowerUpPrefab;
    [SerializeField] int quantityForHealth2 = 7;
    [SerializeField] bool includeBaseRepair = true;
    [SerializeField] GameObject baseRepairPowerUpPrefab;
    [SerializeField] int quantityForBaseRepair = 10;
    //[SerializeField] bool includeReloadSpeed = true;
    //[SerializeField] GameObject reloadSpeedPowerUpPrefab;
    //[SerializeField] int quantityForReloadSpeed = 5;
    [SerializeField] bool includeEmpty = true;
    [SerializeField] GameObject emptyPowerUpPrefab;
    [SerializeField] int quantityForEmpty = 8;
    [SerializeField] bool includeGear = true;
    [SerializeField] GameObject gearPowerUpPrefab;
    [SerializeField] int quantityForGear = 15;
    [SerializeField] bool includeFriendlyTank = true;
    [SerializeField] GameObject friendlyTankPowerUpPrefab;
    [SerializeField] int quantityForFriendlyTank = 2;
    [SerializeField] bool includeOneShot = true;
    [SerializeField] GameObject oneShotPowerUpPrefab;
    [SerializeField] int quantityForOneShot = 5;
    [SerializeField] bool includeShield = true;
    [SerializeField] GameObject shieldPowerUpPrefab;
    [SerializeField] int quantityForShield = 6;

    private GameObject playerTank; 	
	private const float kHealthIncrease1 = 10f; 
	private const float kHealthIncrease2 = 25f;
	private const float kPowerUpLength = 19.5f; 
	private float reloadSpeedTimer;
	private bool rapidReloadIsActive = false; 
	 
	private float playerShieldTimer;
	private bool forceShieldIsActive = false; 
	private float slowEnemyTimer; 
	private const float kBaseHealthIncrease = 25f;
	private float baseShieldTimer;
	private float oneShotKillTimer; 
	public static bool increasePlayerCannonPowerIsActivated = false; 
	public static bool cannotDropPowerUps = false; 
	public GameObject panelPowerUpMessage;
    private Text tpumpTextScript;
	private bool oneShotKillIsActive = false;

	// PowerUp Sounds
	public AudioClip powerUpSound_health1; 
	public AudioClip powerUpSound_health2;
	public AudioClip powerUpSound_baseRepair;
	public AudioClip powerUpSound_friendlyTank;
	public AudioClip powerUpSound_gear;
	public AudioClip powerUpSound_oneShotKill;
	public AudioClip powerUpSound_rapidReload;
	public AudioClip powerUpSound_shield;
	public AudioClip powerUpSound_break;

	public Timeline timeline;
	public bool isTestingPowerUp = false;
	public Transform testPowerUp;

    [SerializeField] BaseHealthManager baseHealthManager;

    private CMPoolManager powerUpPoolManager = new CMPoolManager();
    private CMPoolManager powerUpMessagePoolManager = new CMPoolManager();
    List<GameObject> powerUpList;

    // for medal rewards fx
    private CMPoolManager medalPoolManager = new CMPoolManager();
    [SerializeField] GameObject medalPrefab;
    [SerializeField] AudioClip medalRewardSound;

    // delegate
    public PowerUpManagerDelegate delegatePowerUpManager;

    void Start()
	{
		Invoke("FindThePlayerTank",1.5f);  
        Invoke("LoadPowerUpsPool", 3.0f);
        Invoke("LoadPowerUpMessage", 3.5f);
        Invoke("LoadMedalPool", 4.0f);
        increasePlayerCannonPowerIsActivated = false; 
	}

    private void LoadPowerUpsPool()
    {
        powerUpList = new List<GameObject>();

        InsertPowerUp(this.health1PowerUpPrefab, this.quantityForHealth1, this.includeHealth1);
        InsertPowerUp(this.health2PowerUpPrefab, this.quantityForHealth2, this.includeHealth2);
        InsertPowerUp(this.baseRepairPowerUpPrefab, this.quantityForBaseRepair, this.includeBaseRepair);
        //InsertPowerUp(this.reloadSpeedPowerUpPrefab, this.quantityForReloadSpeed, includeReloadSpeed);
        InsertPowerUp(this.emptyPowerUpPrefab, this.quantityForEmpty, includeEmpty);
        InsertPowerUp(this.gearPowerUpPrefab, this.quantityForGear, includeGear);
        InsertPowerUp(this.friendlyTankPowerUpPrefab, this.quantityForFriendlyTank, includeFriendlyTank);
        InsertPowerUp(this.oneShotPowerUpPrefab, this.quantityForOneShot, includeOneShot);
        InsertPowerUp(this.shieldPowerUpPrefab, this.quantityForShield, includeShield);

        GameObject[] shuffled = Utility.Shuffle<GameObject>(powerUpList.ToArray(), 100);
        foreach (var item in shuffled)
        {
            GameObject clone = Instantiate(item);
            powerUpPoolManager.Add(clone);
        }
    }

    private void InsertPowerUp(GameObject powerUp, int quantity, bool include)
    {
        if (!include)
        {
            return;
        }

        for (int i = 0; i < quantity; i++)
        {
            powerUpList.Add(powerUp);
        }
    }

    private void LoadMedalPool()
    {
        medalPoolManager.Add(medalPrefab, 3);
    }

    private void LoadPowerUpMessage()
    {
        powerUpMessagePoolManager.Add(textPowerUpMessagePrefab, 5);
    }

	public void DisplayPowerUpMessage()
	{
        Transform cloneTextPowerUpMessage = powerUpMessagePoolManager.Spawn().transform;
        cloneTextPowerUpMessage.SetParent (panelPowerUpMessage.transform,false); 
		RectTransform rtScript = cloneTextPowerUpMessage.gameObject.GetComponent<RectTransform>();
		rtScript.anchoredPosition = new Vector2 (838f,259f); 
		tpumpTextScript = cloneTextPowerUpMessage.gameObject.GetComponent<Text>();
		StartCoroutine (DespawnTheTextPowerUpMessage (cloneTextPowerUpMessage)); 
	}

	IEnumerator DespawnTheTextPowerUpMessage(Transform powerUpMessage)
	{
		yield return new WaitForSeconds(0.9f);
        powerUpMessagePoolManager.Despawn(powerUpMessage.gameObject);
	}

	public GameObject SpawnPowerUp(Vector3 pos, Quaternion rot)
	{
        GameObject powerup = powerUpPoolManager.Spawn(pos, rot);
        powerup.SetActive(true);
        return powerup;
	}

    public void DespawnPowerUp(GameObject go)
    {
        powerUpPoolManager.Despawn(go);
    }

	void FindThePlayerTank()
	{
		if (playerTank == null) {
			playerTank = GameObject.FindWithTag("Player"); 
		}
	}

	public void IncreasePlayerHealth1()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_health1);
        this.DisplayPowerUpMessage();
        tpumpTextScript.text = "Health Increased +" + kHealthIncrease1.ToString();
		FindThePlayerTank();
        this.playerTank.GetComponent<HealthPlayer>().IncreasePlayerHealthValue(kHealthIncrease1);
    }

	public void IncreasePlayerHealth2()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_health2);
        this.DisplayPowerUpMessage();
        tpumpTextScript.text = "Health Increased +" + kHealthIncrease2.ToString();
		FindThePlayerTank();
        this.playerTank.GetComponent<HealthPlayer>().IncreasePlayerHealthValue(kHealthIncrease2);
    }

	public void IncreasePlayerReloadSpeed()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_rapidReload);
		reloadSpeedTimer = timeline.time + 29.5f;
        this.DisplayPowerUpMessage();
        if (rapidReloadIsActive) {
			tpumpTextScript.text = "Rapid Reload Extended";	
		} else {
			tpumpTextScript.text = "Rapid Reload";
		}

        // TODO: refactor
        //this.gameObject.GetComponent<InterfaceDisplayManager>().SetFasterReloadSpeed(GameConstants.kPlayerReloadSpeedFast, true);
        
		rapidReloadIsActive = true; 
		timeline.Plan (30f,SetPlayerReloadSpeedToNormal);
    }

	void SetPlayerReloadSpeedToNormal()
	{
		if (timeline.time > reloadSpeedTimer) {
            //this.gameObject.GetComponent<InterfaceDisplayManager>().SetFasterReloadSpeed(GameConstants.kPlayerReloadSpeedNormal);	
			rapidReloadIsActive = false; 
		}
	}

	public void AddPlayerShield()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_shield);
        playerShieldTimer = timeline.time + 29.5f;
        this.DisplayPowerUpMessage();
        if (forceShieldIsActive) {
			tpumpTextScript.text = "Force Shield Extended";
		} else {
			tpumpTextScript.text = "Force Shield";
		}
		forceShieldIsActive = true; 
        this.playerTank.GetComponent<HealthPlayer>().SetPlayerPowerUpShield(true);
		timeline.Plan (30f,RemovePlayerShield);
    }

	void RemovePlayerShield()
	{
		if (timeline.time > playerShieldTimer) {
            this.playerTank.GetComponent<HealthPlayer>().SetPlayerPowerUpShield(false);
			this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_break);
			forceShieldIsActive = false; 
		}
	}

	public void AddBaseShield()
	{
		baseShieldTimer = timeline.time + kPowerUpLength; 
		GameObject[] baseRoots = GameObject.FindGameObjectsWithTag("BaseRoot"); 
		foreach (var baseRoot in baseRoots) {
			HealthBase hbScript = baseRoot.GetComponent<HealthBase>(); 
			hbScript.HasBaseShieldPowerUp = true;
		}
		timeline.Plan (20f,RemoveBaseShield);
        // this.DisplayPowerUpMessage();
    }
	
	void RemoveBaseShield()
	{
		if (timeline.time > baseShieldTimer) {
			GameObject[] baseRoots = GameObject.FindGameObjectsWithTag("BaseRoot"); 
			foreach (var baseRoot in baseRoots) {
				HealthBase hbScript = baseRoot.GetComponent<HealthBase>(); 
				hbScript.HasBaseShieldPowerUp = false; 
			}
		}
	}
	
	public void SlowDownEnemySpeed()
	{
		slowEnemyTimer = timeline.time + kPowerUpLength; 
		GameObject[] enemyTanks = GameObject.FindGameObjectsWithTag("EnemyTank");
		foreach (var tank in enemyTanks) {
			UnityEngine.AI.NavMeshAgent nmaScript = tank.GetComponent<UnityEngine.AI.NavMeshAgent>();
			nmaScript.speed = 0.5f; 
		}
		Invoke("SetEnemySpeedToNormal",20f);
		timeline.Plan (20f,SetEnemySpeedToNormal);
        
    }

	void SetEnemySpeedToNormal()
	{
		if (timeline.time > slowEnemyTimer) {
			GameObject[] enemyTanks = GameObject.FindGameObjectsWithTag("EnemyTank");
			foreach (var tank in enemyTanks) {
				UnityEngine.AI.NavMeshAgent nmaScript = tank.GetComponent<UnityEngine.AI.NavMeshAgent>();
				nmaScript.speed = 1f; 
			}
		}
	}

	public void RepairAllBase()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_baseRepair);
        this.DisplayPowerUpMessage();
        tpumpTextScript.text = "Base Repaired";
        baseHealthManager.RepairAll();
    }

	public void AddFriendlyPowerUpTank()
	{
        if (this.GetComponent<SpawnFriendlyTanksManager>().AddPowerUpTank())
        {
            this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_friendlyTank);
            this.DisplayPowerUpMessage();
            tpumpTextScript.text = "Friedly Tank Added";
        }
	}

	public void IncreasePlayerCannonPower()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_oneShotKill);
        this.DisplayPowerUpMessage();

        if (oneShotKillIsActive) {
			tpumpTextScript.text = "One-Shot Kill Extended";
		} else {
			tpumpTextScript.text = "One-Shot Kill";
		}

		oneShotKillTimer = timeline.time + 29.5f; 
		increasePlayerCannonPowerIsActivated = true;

		if (!oneShotKillIsActive) {
			oneShotKillIsActive = true;

            // TODO: remove pool manager
            //clonePowerUpFxOneShotFirePoint = PoolManager.Pools ["PowerUp"].Spawn (powerUpFX_oneShotFirePoint_prefab.transform, firePointPlayer.position, firePointPlayer.rotation);
            //clonePowerUpFxOneShotFirePoint.parent = firePointPlayer; 

            // TODO: fix this, its causing an error
            // clonePowerUpFxOneShotFirePoint.position = new Vector3 (firePointPlayer.position.x,firePointPlayer.position.y + 0.05f,firePointPlayer.position.z);
		}

		timeline.Plan (30f,SetPlayerCannonToNormal);
    }

	void SetPlayerCannonToNormal()
	{
		if (timeline.time > oneShotKillTimer) {
			increasePlayerCannonPowerIsActivated = false;
			oneShotKillIsActive = false;
		}
	}

	public void GearCollected()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_gear);

        if (delegatePowerUpManager != null)
        {
            delegatePowerUpManager.DidCollectGear();
        }
    }

    public void DespawnMedalSoundEffect()
	{
		this.GetComponent<AudioSource>().PlayOneShot(powerUpSound_gear);
	}

    // Medal Reward FX
    public void DisplayMedalReward()
    {
        if (GameConstants.player)
        {
            Transform playerTransform = GameConstants.player.transform;
            Vector3 medalRewardPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.7f, playerTransform.position.z);
            GameObject medalRewardClone = medalPoolManager.Spawn(medalRewardPosition);
            medalRewardClone.transform.SetParent(playerTransform);
            this.GetComponent<AudioSource>().PlayOneShot(medalRewardSound);
            StartCoroutine(DespawnMedal(medalRewardClone));

            if (delegatePowerUpManager != null)
            {
                delegatePowerUpManager.DidDisplayRewardMedal();
            }
        }
    }

    IEnumerator DespawnMedal(GameObject medal)
    {
        yield return new WaitForSeconds(2.0f);
        if (GameConstants.gameManager)
        {
            GameConstants.gameManager.SendMessage("DespawnMedalSoundEffect", SendMessageOptions.DontRequireReceiver); // from PowerUpManager 
        }
        medalPoolManager.Despawn(medal);
        SpawnMedalEffects(medal.transform);
    }

    void SpawnMedalEffects(Transform medalTransform)
    {
        Transform fxPos = medalTransform.GetChild(0);
        this.gameObject.GetComponent<EffectsManager>().SpawnEffects(EffectsType.PowerUpFx, fxPos);
    }
}
using UnityEngine;
using Chronos; 
using CMT.AI;

public class SpawnFriendlyTanksManager : SpawnManager {

	// private
	private float spawnRate = 3f; 
	private float nextSpawn = 0f;
	private bool canStartAddingFriendlyTanks = false;

	// public
	public static bool friendlyTanksHadBeenAdded = false; 
	 
	// public GameObject cannonFriendly;
	public Timeline timeline;

    private int powerUpTankQuantity = 0;
    [SerializeField] int maxPowerUpTankQuantity = 2;

    private void Start()
    {
        HealthAI.OnPowerUpTankDestroyed += OnPowerUpTankDestroyed;
    }

    public void StartAddingFriendlyTanks()
    {
        canStartAddingFriendlyTanks = true;
        powerUpTankQuantity = 0;
    }

    void Update () {
		if (canStartAddingFriendlyTanks) {
            if (tanksInScene < maximumNumberOfTanks)
            {
                CreateFriendlyTanks();
            }
		}
	}

	void CreateFriendlyTanks()
	{
        if (GameConstants.GameplayState.GameOver)
        {
            return;
        }

        if (GameConstants.GameplayState.GamePaused)
        {
            return;
        }

        // TODO: refactor in SpawnManager
        if (timeline.time > nextSpawn) {
            nextSpawn = timeline.time + spawnRate; 
            Vector3 spawnPoint = environmentManager.GetSpawnPoint(false);
            Transform clone = tanksPool.Spawn(spawnPoint, Quaternion.identity).transform;
            DisplayParts(clone, true);
            
            HealthAI haiScript = clone.gameObject.GetComponent<HealthAI>();
            haiScript.isEnemyTank = false;
            haiScript.IsPowerUpTank = false;

            FriendlyTankAI fta = clone.gameObject.GetComponent<FriendlyTankAI>();
            fta.IsHidden = false;
            fta.SelectNewEnemyTankTarget();

            tanksInScene++;

            // add friendly tank to radar map
            this.gameObject.GetComponent<GameManager>().AddTankInRadar(clone, false);

			// configure Timeline script
			Timeline tlScript = clone.gameObject.GetComponent<Timeline>();
			tlScript.globalClockKey = "FriendlyTanks";

            // add to the list of gameobjects
            friendlyTanksHadBeenAdded = true;
        }
	}

	public bool AddPowerUpTank()
	{
        bool canAdd = powerUpTankQuantity < maxPowerUpTankQuantity;

        if (canAdd) {
            Vector3 spawnPoint = environmentManager.GetSpawnPoint(false);

            Transform clone = tanksPool.Spawn(spawnPoint, Quaternion.identity).transform;
            DisplayParts(clone);
            
            HealthAI haiScript = clone.gameObject.GetComponent<HealthAI>();
            haiScript.isEnemyTank = false;
            haiScript.IsPowerUpTank = true;

            FriendlyTankAI fta = clone.gameObject.GetComponent<FriendlyTankAI>();
            fta.IsHidden = false;
            fta.SelectNewEnemyTankTarget();

            // add friendly tank to radar map	
            gameObject.GetComponent<GameManager>().AddTankInRadar(clone, false);
            powerUpTankQuantity++;
        }

        return canAdd;
	}

    private void OnPowerUpTankDestroyed()
    {
        powerUpTankQuantity--;
        if (powerUpTankQuantity < 0)
        {
            powerUpTankQuantity = 0;
        }
    }
}

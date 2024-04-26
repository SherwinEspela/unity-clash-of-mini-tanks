using UnityEngine;
using Chronos;
using CMT.AI;

public interface IEnemyAdder
{
    void AddEnemy(int num);
}

public class SpawnEnemyTanksManager : SpawnManager, IEnemyAdder {

	private float enemySpawnRate = 3f; 
	private float nextEnemySpawn = 0f; 
	private bool beginAddingEnemies = false;
    
	public Timeline timeline;

    // for debugging
    [SerializeField] bool attackPlayerTankOnly = false;
    [SerializeField] bool attackFriendlyTanksOnly = false;
    [SerializeField] bool attackBaseOnly = false;

    public void StartAddingEnemies()
	{
		beginAddingEnemies = true;
	}

	void Update () {
		if (beginAddingEnemies) {
            if (tanksInScene < maximumNumberOfTanks)
            {
                CreateEnemy();
            }
		}
	}

	void CreateEnemy()
	{
        if (GameConstants.GameplayState.GameOver)
        {
            return;
        }

        if (GameConstants.GameplayState.GamePaused)
        {
            return;
        }

		if (timeline.time > nextEnemySpawn) {
			nextEnemySpawn = timeline.time + enemySpawnRate; 
            Vector3 spawnPoint = environmentManager.GetSpawnPoint();
            GameObject clone = tanksPool.Spawn(spawnPoint, Quaternion.identity);        
            DisplayParts(clone.transform);
            EnemyTankAI eta = clone.gameObject.GetComponent<EnemyTankAI>();
            eta.AttackPlayerTankOnly = this.attackPlayerTankOnly;
            eta.AttackFriendlyTanksOnly = this.attackFriendlyTanksOnly;
            eta.AttackBaseOnly = this.attackBaseOnly;
            eta.IsHidden = false;
            eta.EnemyTankSelectsNewTarget();

            if (!clone)
            {
                return;
            }

            // add enemy tank to radar map
            gameObject.GetComponent<GameManager>().AddTankInRadar(clone.transform);
            tanksInScene++;
		}
	}

    public void AddEnemy(int num)
    {
        maximumNumberOfTanks += num;
    }
}

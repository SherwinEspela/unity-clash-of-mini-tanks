using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Chronos; 
using CMT.AI;

public class PlayerLoader : MonoBehaviour {

    [SerializeField] protected float movementSpeed = 3f;
    [SerializeField] float exitColliderRadius = 8f;

    private readonly int _totalFriendlyTanks = 12;
    private readonly int _totalEnemyTanks = 36;

    [SerializeField] GameObject[] tanks; 
	[SerializeField] KeyBoardControls keyboardControlsScript;
    [SerializeField] JoystickLeft joystickLeft;
    [SerializeField] JoystickRight joystickRight;
    [SerializeField] TurretControlsTouchPad turretControlsTouchPad;
    [SerializeField] Button buttonFireCannonScript; 
	[SerializeField] CameraSmoothFollow cameraSmoothFollowScript; 
	[SerializeField] SpawnEnemyTanksManager spawnEnemyTanksManagerScript; 
	[SerializeField] SpawnFriendlyTanksManager spawnFriendlyTanksManagerScript;
    [SerializeField] GameObject dustTrailPrefab;

    // Events
    public delegate void PlayerLoaderDelegate();
    public static event PlayerLoaderDelegate OnLoadingTanksComplete;

    public void LoadTanks()
    {
        LoadThePlayerTank();
        LoadTheAITanks();

        if (OnLoadingTanksComplete != null)
        {
            OnLoadingTanksComplete();
        }
    }

    void LoadThePlayerTank()
	{
        foreach (var tank in tanks)
        {
            tank.tag = GameConstants.kTagUntagged;
        }

        // 1. get the player from the tanks array
        GameObject playerTank = Instantiate(tanks[TankSelectionManager.selectedTankPlayer]) as GameObject;
        playerTank.SetActive(true);
        playerTank.tag = GameConstants.kTagPlayerTank;
        playerTank.name = playerTank.name + playerTank.tag;
        tanks[TankSelectionManager.selectedTankPlayer].gameObject.tag = GameConstants.kTagPlayerTank;
        playerTank.transform.position = new Vector3 (0,1,0); 
		
		// 1.1 Hook Player Tank to Camera Smooth follow script
		cameraSmoothFollowScript.target = playerTank.transform;
		
		// 3. Remove AI components
		ArtificialIntelligence aiScript = playerTank.GetComponent<ArtificialIntelligence>();
		aiScript.enabled = false;
		UnityEngine.AI.NavMeshAgent nmaScript = playerTank.GetComponent<UnityEngine.AI.NavMeshAgent>();
		nmaScript.enabled = false;
		SphereCollider sc = playerTank.GetComponent<SphereCollider>();
		sc.enabled = false;

        if (playerTank.GetComponent<HealthAI>())
        {
            playerTank.GetComponent<HealthAI>().enabled = false;
        }

        if (playerTank.GetComponent<EnemyTankAI>())
        {
            playerTank.GetComponent<EnemyTankAI>().enabled = false;
        }

        if (playerTank.GetComponent<FriendlyTankAI>())
        {
            playerTank.GetComponent<FriendlyTankAI>().enabled = false;
        }

        // 4. Add Player components
        HealthPlayer hpScript = playerTank.AddComponent<HealthPlayer>();
		GameObject leftJoystick = GameObject.Find ("LeftJoystick");

        // Add rigid body
        Rigidbody playerRB = playerTank.GetComponent<Rigidbody>() == null ? playerTank.AddComponent<Rigidbody>() : playerTank.GetComponent<Rigidbody>();
        playerRB.mass = 20f;
        playerRB.isKinematic = false;
        
		// 6. Disable the colliderExitDetector
		Transform tankBodyRotation = playerTank.transform.Find (GameConstants.kTankBodyRotation);
		Transform body = tankBodyRotation.Find ("Body");
		Transform colliderExitDetector = body.Find ("colliderExitDetector");
		colliderExitDetector.gameObject.SetActive (false); 
		
		// 7. Configure the colliderHealthBody
		Transform colliderHealthBody = body.Find ("colliderHealthBody");
		HealthSubPart hsScript = colliderHealthBody.gameObject.GetComponent<HealthSubPart>();
		hsScript.enabled = false;
		HealthSubPartPlayer hsppScript = colliderHealthBody.gameObject.AddComponent<HealthSubPartPlayer>();
		hsppScript.mainObject = playerTank;
		AvoidCollisionScript acScript = colliderHealthBody.gameObject.AddComponent<AvoidCollisionScript>();
		acScript.layer1 = 9; 
		acScript.layer2 = 19;
		
		// 8. Configure the Turret
		Transform turret = tankBodyRotation.Find (GameConstants.kTurret); 
		turret.name = "TurretPlayer";
		turret.tag = GameConstants.kTagPlayerTankTurret;
		TurretControls tcScript = turret.gameObject.GetComponent<TurretControls>();
		tcScript.enabled = false;
        tcScript.SetIsThePlayerTank();

        turretControlsTouchPad.TurretControlsScript = tcScript;

        Transform cannon = turret.Find("Cannon");
        Transform firePoint = cannon.Find("FirePoint");
        firePoint.name = "FirePointPlayer";
        tcScript.firePoint = firePoint;
        tcScript.textSpawn = cannon.Find("textSpawn");

        keyboardControlsScript.tcScript = tcScript;
		
		// 9. Configure the colliderHealthTurret
        Transform colliderHealthTurret = turret.Find("colliderHealthTurret");
        HealthSubPart hsTurret = colliderHealthTurret.GetComponent<HealthSubPart>();
        hsTurret.enabled = false;
        HealthSubPartPlayer hsppTurret = colliderHealthTurret.gameObject.AddComponent<HealthSubPartPlayer>();
        hsppTurret.mainObject = playerTank;
        AvoidCollisionScript acTurret = colliderHealthTurret.gameObject.AddComponent<AvoidCollisionScript>();
        acTurret.layer1 = 9;
        acTurret.layer2 = 19;

        // 10. Disable the Timeline script
        Timeline tlScript = playerTank.GetComponent<Timeline>();
        tlScript.enabled = false;

        Timeline tlScriptTurret = turret.gameObject.GetComponent<Timeline>();
        tlScriptTurret.enabled = false;

        // 10. Add the damage FX
        Transform damageEffectController = body.Find ("damageFXLocator");
		Transform damageFX = damageEffectController.Find ("damageFX");
		hpScript.tankDamageFX = damageFX.gameObject.GetComponent<ParticleSystem> ();
        playerTank.SetActive(true);

        // 11. Add the dust trail
        AddDustrail(playerTank);

        // 12. Assign joystick controls
        this.joystickLeft.PlayerTank = playerTank.transform;
        this.joystickRight.PlayerTankTurret = turret;
	}

    void AddDustrail(GameObject tank)
    {
        GameObject dustTrailClone = Instantiate(dustTrailPrefab);
        dustTrailClone.transform.parent = tank.transform;
        dustTrailClone.transform.localPosition = new Vector3(0, 0.12f, -0.5f);
    }

    // TODO: refactor
	void LoadTheAITanks()
	{
        List<GameObject> taggedEnemyTanks = new List<GameObject>();
        List<GameObject> taggedFriendlyTanks = new List<GameObject>();

        int tempCounter = 0;
        foreach (var aTank in tanks) {
            if (aTank.tag.Contains(GameConstants.kTagUntagged) && !aTank.name.Contains("_nt"))
            {
                if (tempCounter == 0)
                {
                    // guarantees 1 enemy tank added
                    taggedEnemyTanks.Add(aTank);
                    tempCounter++;
                    continue;
                }

                if (tempCounter == 1)
                {
                    // guarantees 1 friendly tank added
                    taggedFriendlyTanks.Add(aTank);
                    tempCounter++;
                    continue;
                }

                int randomNumber = Random.Range(0, 100);
                aTank.tag = randomNumber > 40 ? GameConstants.kTagEnemyTank : GameConstants.kTagFriendlyTank;
                if (aTank.tag.Contains(GameConstants.kTagEnemyTank))
                {
                    taggedEnemyTanks.Add(aTank);
                } else
                {
                    taggedFriendlyTanks.Add(aTank);
                }
            }
		}

        int friendlyTankCounter = 0; 
		int enemyTankCounter = 0;

		bool isComplete = false; 
		while (!isComplete) {	 
		
            if (enemyTankCounter < _totalEnemyTanks)
            {
                int randomNumber = Random.Range(0, taggedEnemyTanks.Count - 1);
                GameObject randomTank = Instantiate(taggedEnemyTanks[randomNumber]);
                ConfigEnemyTank(randomTank.transform);
                spawnEnemyTanksManagerScript.AddTankToPool(randomTank);
                enemyTankCounter++;
            }
            else
            {
                if (friendlyTankCounter < _totalFriendlyTanks)
                {
                    int randomNumber = Random.Range(0, taggedFriendlyTanks.Count - 1);
                    GameObject randomTank = Instantiate(taggedFriendlyTanks[randomNumber]);
                    ConfigFriendlyTank(randomTank.transform);
                    spawnFriendlyTanksManagerScript.AddTankToPool(randomTank);
                    friendlyTankCounter++;
                }
                else
                {
                    isComplete = true;
                }
            }
        }
	}

    void ConfigEnemyTank(Transform clone)
    {
        clone.tag = GameConstants.kTagEnemyTank;
        clone.name += "_" + clone.tag + "_" + Utility.CreateRandomSuffix();
        clone.gameObject.GetComponent<ArtificialIntelligence>().enabled = false;

        HealthAI haiScript = clone.gameObject.GetComponent<HealthAI>();
        haiScript.isEnemyTank = true;
        EnemyTankLevelUpdate(haiScript);
       
        // add rigid body
        if (!clone.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rigidBody = clone.gameObject.AddComponent<Rigidbody>();
            rigidBody.mass = 100f;
            rigidBody.isKinematic = true;
        }

        // set layer to ignore collisions
        clone.gameObject.layer = 17;
        Transform tankBodyRotation = clone.Find(GameConstants.kTankBodyRotation);
        Transform body = tankBodyRotation.Find("Body");
        Transform colliderHealthBody = body.Find("colliderHealthBody");
        colliderHealthBody.gameObject.layer = 16;
        Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
        Transform colliderHealthTurret = turret.Find("colliderHealthTurret");
        colliderHealthTurret.gameObject.layer = 16;

        // set new radius for colliderExitDetector sphere collider
        SetExitColliderDetectorRadius(body, exitColliderRadius);

        // add pointerToNewTarget object
        GameObject pointerToNewTargetGO = null;
        if (!tankBodyRotation.Find(GameConstants.kPointerToNewTarget))
        {
            pointerToNewTargetGO = new GameObject();
            pointerToNewTargetGO.name = GameConstants.kPointerToNewTarget;
            pointerToNewTargetGO.transform.position = turret.position;
            pointerToNewTargetGO.transform.rotation = turret.rotation;
            pointerToNewTargetGO.transform.parent = tankBodyRotation;
        }

        if (!clone.gameObject.GetComponent<EnemyTankAI>())
        {
            EnemyTankAI enemyTankAI = clone.gameObject.AddComponent<EnemyTankAI>();
            enemyTankAI.pointerToNewTargetScript = pointerToNewTargetGO.AddComponent<PointerToNewTarget>();
        } else
        {
            EnemyTankAI enemyTankAI = clone.gameObject.GetComponent<EnemyTankAI>();
            enemyTankAI.pointerToNewTargetScript = pointerToNewTargetGO.AddComponent<PointerToNewTarget>();
        }

        ConfigureTurret(clone, pointerToNewTargetGO);

        AddDustrail(clone.gameObject);
    }

    void ConfigureTurret(Transform clone, GameObject pointerToNewTarget)
    {
        Transform tankBodyRotation = clone.Find(GameConstants.kTankBodyRotation);
        Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
        TurretControls tcScript = turret.gameObject.GetComponent<TurretControls>();

        tcScript.isEnemyTank = true;
        tcScript.isFriendlyTank = false;
        tcScript.fireRate = 5f;
        EnemyTankAI enemyTankAI = clone.gameObject.GetComponent<EnemyTankAI>();
        tcScript.artificialIntelligence = enemyTankAI;
        tcScript.mainObjectName = clone.name;
        tcScript.pointerToNewTarget = pointerToNewTarget;

        enemyTankAI.turretControlsScript = tcScript;
        enemyTankAI.SetMovementSpeed(movementSpeed);
    }

    void SetExitColliderDetectorRadius(Transform body, float radius)
    {
        Transform colliderExitDetector = body.Find("colliderExitDetector");
        SphereCollider sphereCollider = colliderExitDetector.gameObject.GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    void EnemyTankLevelUpdate(HealthAI haiScript)
    {
        if (GameConstants.roundCounter > 1 && GameConstants.roundCounter <= 10)
        {
            int randomNumber = Random.Range(0, 3);
            if (randomNumber == 3)
            {
                if (GameConstants.roundCounter % 5 == 0)
                {
                    haiScript.health = 20f + (GameConstants.roundCounter * 2f);
                    if (haiScript.health > 40f)
                    {
                        haiScript.health = 40f;
                    }
                }
                else
                {
                    haiScript.health = 20f;
                }
            }
            else
            {
                haiScript.health = 20f;
            }
        }
        else if (GameConstants.roundCounter > 10)
        {
            int randomNumber = Random.Range(0, 1);
            if (randomNumber == 1)
            {
                if (GameConstants.roundCounter % 5 == 0)
                {
                    haiScript.health = 20f + (GameConstants.roundCounter * 2f);
                    if (haiScript.health > 40f)
                    {
                        haiScript.health = 40f;
                    }
                }
                else
                {
                    haiScript.health = 20f;
                }
            }
            else
            {
                haiScript.health = 20f;
            }
        }
        else
        {
            haiScript.health = 20;
        }
    }

    void ConfigFriendlyTank(Transform clone)
    {
        clone.tag = GameConstants.kTagFriendlyTank;
        clone.name += "_" + clone.tag + "_" + Utility.CreateRandomSuffix();

        ArtificialIntelligence aiScript = clone.gameObject.GetComponent<ArtificialIntelligence>();
        aiScript.enabled = false;

        HealthAI haiScript = clone.gameObject.GetComponent<HealthAI>();
        haiScript.isEnemyTank = false;
        haiScript.health = 30;
        Transform canvasTag = clone.Find("CanvasTag");
        canvasTag.gameObject.SetActive(true);

        // add rigid body
        if (!clone.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rigidBody = clone.gameObject.AddComponent<Rigidbody>();
            rigidBody.mass = 100f;
            rigidBody.isKinematic = true;
        }

        // set layer to ignore collisions
        clone.gameObject.layer = 17;
        Transform tankBodyRotation = clone.Find(GameConstants.kTankBodyRotation);
        Transform body = tankBodyRotation.Find("Body");
        Transform colliderHealthBody = body.Find("colliderHealthBody");
        colliderHealthBody.gameObject.layer = 16;
        Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
        Transform colliderHealthTurret = turret.Find("colliderHealthTurret");
        colliderHealthTurret.gameObject.layer = 16;

        // add pointerToNewTarget object
        GameObject pointerToNewTargetGO = null;
        if (!tankBodyRotation.Find(GameConstants.kPointerToNewTarget))
        {
            pointerToNewTargetGO = new GameObject();
            pointerToNewTargetGO.name = GameConstants.kPointerToNewTarget;
            pointerToNewTargetGO.transform.position = turret.position;
            pointerToNewTargetGO.transform.rotation = turret.rotation;
            pointerToNewTargetGO.transform.SetParent(tankBodyRotation);
        }

        if (!clone.gameObject.GetComponent<FriendlyTankAI>())
        {
            FriendlyTankAI friendlyTankAI = clone.gameObject.AddComponent<FriendlyTankAI>();
            friendlyTankAI.pointerToNewTargetScript = pointerToNewTargetGO.AddComponent<PointerToNewTarget>();
        } else
        {
            FriendlyTankAI friendlyTankAI = clone.gameObject.GetComponent<FriendlyTankAI>();
            friendlyTankAI.pointerToNewTargetScript = pointerToNewTargetGO.AddComponent<PointerToNewTarget>();
        }

        ConfigureFriendlyTurret(clone, pointerToNewTargetGO);

        // remove EnemyTankAI script if it happens to have it
        if (clone.gameObject.GetComponent<EnemyTankAI>())
        {
            clone.gameObject.GetComponent<EnemyTankAI>().enabled = false;
        }

        AddDustrail(clone.gameObject);
    }

    void ConfigureFriendlyTurret(Transform clone, GameObject pointerToNewTarget)
    {
        Transform tankBodyRotation = clone.Find(GameConstants.kTankBodyRotation);
        Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
        TurretControls tcScript = turret.gameObject.GetComponent<TurretControls>();

        tcScript.isEnemyTank = false;
        tcScript.isFriendlyTank = true;
        tcScript.fireRate = 7f;
        FriendlyTankAI friendlyTankAI = clone.gameObject.GetComponent<FriendlyTankAI>();
        tcScript.artificialIntelligence = friendlyTankAI;
        tcScript.mainObjectName = clone.name;
        tcScript.pointerToNewTarget = pointerToNewTarget;

        friendlyTankAI.turretControlsScript = tcScript;
        friendlyTankAI.SetMovementSpeed(movementSpeed);
    }
}

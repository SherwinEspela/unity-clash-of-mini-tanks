using UnityEngine;
using System.Collections;
using Chronos; 
using CMT.AI;

public class TurretControls : MonoBehaviour {

	// private
	private float nextFire;
	private GameObject player;
	private Transform turret; 
	private float distance;
	private float cannonSpeed = 12f;
	private GameObject gameManager;
	private float nextFireRemaining;
	private bool isTurretRotating = false;
    private bool isVisibleInCamera = false;
    private bool isThePlayerTank = false;

    public Transform firePoint;
	public Transform textSpawn; 
	public Timeline timeline; 
	public float fireRate = 0f;

	// properties
	public bool isEnemyTank { get; set; }
	public bool isFriendlyTank { get; set; }
	public ArtificialIntelligence artificialIntelligence { get; set; }
	public Transform selectedTarget { get; set; }
	public string mainObjectName { get; set; }
	public GameObject pointerToNewTarget { get; set; }

	void Start()
	{
		gameManager = GameObject.Find ("GameManager");
		nextFire = 0.0f;
		isTurretRotating = false; 
	}

	void Update()
	{
		if (!GameConstants.GameplayState.GameOver && !isTurretRotating) {
			this.transform.LookAt(selectedTarget);
		}
	}

	void OnDespawned()
	{
		isTurretRotating = false;
		nextFire = 0.0f;
		StopAllCoroutines ();
	}

	void OnDisable(){
		isTurretRotating = false;
		nextFire = 0.0f;
		StopAllCoroutines ();
	}
	
    public void SetIsThePlayerTank()
    {
        this.isThePlayerTank = true;
    }

	public void RotateTurretToNewSelectedTarget()
	{
		if (this.gameObject.activeSelf == true) {
			this.isTurretRotating = true;
			StartCoroutine (GetPointerToNewTargetAngleCoroutine ());
		}
	}

	IEnumerator GetPointerToNewTargetAngleCoroutine ()
	{
		yield return new WaitForSeconds (0.1f);
		if (this.gameObject.activeSelf == true) {
			StartCoroutine (RotateTurretCoroutine ());	
		}
	}

	IEnumerator RotateTurretCoroutine ()
	{
		this.GetPointerToNewTarget ();
		float smoothing = 7f;
		while (this.transform.eulerAngles.y - this.pointerToNewTarget.transform.eulerAngles.y > 0.3f ||
			this.pointerToNewTarget.transform.eulerAngles.y - this.transform.eulerAngles.y > 0.3f) {
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation,
				this.pointerToNewTarget.transform.rotation, 
				Time.deltaTime * smoothing);
			yield return null;
		}

		yield return new WaitForSeconds (0.5f);
			
		this.isTurretRotating = false;
	}

	private void GetPointerToNewTarget()
	{
		if (this.pointerToNewTarget != null) {
			return;
		}

		this.pointerToNewTarget = this.selectedTarget.gameObject;
	}

	public void FireAI()
	{
        if (GameConstants.GameplayState.GamePaused)
        {
            return;
        }

		if (!isTurretRotating && Time.time > nextFire) {
			FireCannonAI();
		}
	}
		
	public void Fire()
	{
		FireCannonPlayer();
	}

	void FireCannonAI()
	{
		nextFire = Time.time + fireRate; 
		FireCannon (); 
	}

	void FireCannonPlayer()
	{
		if (GameConstants.PlayerTankState.CanFireCannon) {
			FireCannon();
            GameConstants.PlayerTankState.CanFireCannon = false;
		}
	}

	void FireCannon()
	{
        Transform cannonclone = null;
        if (isThePlayerTank) {
            GameConstants.gameManager.GetComponent<ScoreManager>().AddTotalShot();
            if (PowerUpManager.increasePlayerCannonPowerIsActivated) {
                GameConstants.gameManager.GetComponent<SoundFXManager>().PlayCannonFireSoundPowerUpOneShotKill();
                cannonclone = GameConstants.gameManager.GetComponent<CannonPoolManager>().SpawnCannon(firePoint.position, firePoint.rotation, true).transform;
            } else {
                GameConstants.gameManager.GetComponent<SoundFXManager>().PlayCannonFireSound();
                cannonclone = GameConstants.gameManager.GetComponent<CannonPoolManager>().SpawnCannon(firePoint.position, firePoint.rotation).transform;
            }
		} else {
            cannonclone = GameConstants.gameManager.GetComponent<CannonPoolManager>().SpawnCannon(firePoint.position, firePoint.rotation).transform;
            GameConstants.gameManager.GetComponent<SoundFXManager>().PlayCannonFireSound();
        }
        
        Cannon cannonScript = cannonclone.gameObject.GetComponent<Cannon>();
        cannonScript.FiredBy = this.transform.root.gameObject;
        cannonclone.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(new Vector3(0,-0.2f,cannonSpeed));

        if (isVisibleInCamera)
        {
            GameConstants.gameManager.GetComponent<EffectsManager>().SpawnEffects(EffectsType.Muzzle, firePoint);
            GameConstants.gameManager.GetComponent<EffectsManager>().SpawnEffects(EffectsType.Smoke, firePoint);
            GameConstants.gameManager.GetComponent<EffectsManager>().SpawnEffects(EffectsType.TextFx, textSpawn);

            var fxType = EffectsType.SmokeTrail;
            Transform clone = GameConstants.gameManager.GetComponent<EffectsManager>().SpawnEffectsWithoutDespawn(fxType, firePoint);
            clone.parent = firePoint;
            GameConstants.gameManager.GetComponent<EffectsManager>().DespawnEffectsDelayed(fxType, clone.gameObject, 7.0f);
        }
    }

    // camera visibility
    private void OnBecameVisible()
    {
        isVisibleInCamera = true;
    }

    private void OnBecameInvisible()
    {
        isVisibleInCamera = false;
    }
}

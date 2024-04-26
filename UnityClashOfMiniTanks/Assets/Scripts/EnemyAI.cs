using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	private enum EnemyBehavior {
		Follow,
		Stop
	}

	private enum AttackTarget {
		Player,
		Base
	}

	private GameObject player;
	private GameObject[] bases;
	public Transform mainTarget; 
	public bool isEnemy;
	private UnityEngine.AI.NavMeshAgent navMeshAgentScript; 
	public TurretControls turretControlsScript; 
	public bool isAttackingPlayer = false;
	private float distanceFromPlayer;
	private Vector3 evadePosition;  
	private float xPos; 
	private float zPos; 
	private Transform turret;

	// Use this for initialization
	void Start () {
		if (isEnemy) {
			player = GameObject.FindGameObjectWithTag(GameConstants.kTagPlayerTank);
			Transform tankBodyRotation = player.transform.Find(GameConstants.kTankBodyRotation); 
			turret = tankBodyRotation.Find(GameConstants.kTurret);
			bases = GameObject.FindGameObjectsWithTag("Base");
			if (bases.Length > 0) {
				//Debug.Log(bases.Length + " bases found........");
			}
			
			int targetToAttack = TargetToAttackDecision(); 
			
			//Debug.Log("targetToAttack value: " + targetToAttack);
			
			if (targetToAttack == 0) {
				//Debug.Log("Targeting the player...");
				mainTarget = player.transform;
				isAttackingPlayer = true; 
			} else {
				//Debug.Log("Targeting the base...");
				mainTarget = bases[Random.Range(0,bases.Length)].transform;
			}
			
			navMeshAgentScript = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}
	}

	void Awake()
	{

	}

	// Update is called once per frame
	void Update () {
		if (isEnemy) {
			distanceFromPlayer = Vector3.Distance(player.transform.position,this.transform.position);
			if (distanceFromPlayer < 5f) {
				Evade(); 
			} else {
				Follow();
			}
		}
	}

	public int TargetToAttackDecision()
	{
		int decision = Random.Range(0,2);
		return decision; 
	}

	void Follow()
	{
		navMeshAgentScript.destination = mainTarget.position; 
		//Debug.Log("Distance: " + navMeshAgentScript.remainingDistance);

		if (navMeshAgentScript.remainingDistance < 9f) {
			//turretControlsScript.SetIsWithinFiringRangeTrue(); 
		} 
//		else if (navMeshAgentScript.remainingDistance < 6f) {
//			hasReachedTarget = true; 
//		}
		else {
			//turretControlsScript.SetIsWithinFiringRangeFalse();  
		}

//		if (hasReachedTarget) {
//			if (navMeshAgentScript.remainingDistance > 12f){
//				// switch target
//				if (mainTarget == player.transform) {
//					hasChangedPlayerAsTarget = false; 
//					mainTarget = bases[Random.Range(0,bases.Length)].transform;
//					hasReachedTarget = false; 
//				}
//			}
//		}
	}

	void Evade()
	{
//		if (!hasChangedPlayerAsTarget) {
//			mainTarget = player.transform; 
//			navMeshAgentScript.destination = mainTarget.position; 
//			hasChangedPlayerAsTarget = true; 
//		}

		if (player.transform.position.x > this.transform.position.x) {
			// player is located east
			xPos = this.transform.position.x - 5f; 
		} else {
			// player is located west
			xPos = this.transform.position.x + 5f; 
		}

		if (player.transform.position.z > this.transform.position.z) {
			// player is located north
			zPos = this.transform.position.z - 5f; 
		} else {
			// player is located south
			zPos = this.transform.position.z + 5f; 
		}

		navMeshAgentScript.destination = new Vector3(xPos,this.transform.position.y,zPos); 
	}

	void ChangeStoppingDistance()
	{

	}

	void Stop()
	{
		navMeshAgentScript.speed = 0f; 
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			//Debug.Log("Player detected!!!");

			ChangeTargetToPlayer(); 
		}
	}

	public void ChangeTargetToPlayer()
	{
		if (mainTarget != player.transform) {
			mainTarget = player.transform;
			isAttackingPlayer = true; 
		}
	}

	public void ChangeTargetToBase()
	{
		mainTarget = bases[Random.Range(0,bases.Length)].transform;
		isAttackingPlayer = false; 
	}

	public Transform GetTurret()
	{
		return turret; 
	}
}

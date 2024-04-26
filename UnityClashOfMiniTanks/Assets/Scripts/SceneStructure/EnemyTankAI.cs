using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace CMT.AI 
{
	public class EnemyTankAI : ArtificialIntelligence {

		private GameObject player;
		private HealthBase healthBaseScript;
		private bool wantsToDestroyBase = false;

        // for debugging
        public bool AttackPlayerTankOnly { get; set; }
        public bool AttackFriendlyTanksOnly { get; set; }
        public bool AttackBaseOnly { get; set; }

        void Start()
		{
			navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

        private void OnDisable()
        {
            if (healthAIScript != null)
            {
                healthAIScript.OnAITankDestroyed -= EnemyTankSelectsNewTarget;
            }
        }

        void Update(){
			if (!GameConstants.GameplayState.GameOver) {
                if (selectedTarget == null)
                {
                    return;
                }

				distanceFromTarget = Vector3.Distance(selectedTarget.position,this.transform.position);

				if (distanceFromTarget < evadeRange) {
					Evade(selectedTarget); 
				}
				else if (distanceFromTarget > kFarRange && isTargetEngaged && !this.wantsToDestroyBase){
					EnemyTankSelectsNewTarget ();
					isTargetEngaged = false;
				}
				else {
					Follow();
				}

				if (distanceFromTarget < firingRange) {
					isTargetEngaged = true;
					this.GetTurretControlScript ();
					this.turretControlsScript.FireAI();	
				}
			}
		}

		void OnTriggerEnter(Collider col)
		{
			if (!this.wantsToDestroyBase) {
				if (col.gameObject.tag == "Player") {
					ChangeTargetToPlayer(); 
				}	
			}
		}

		void ConfigureEnemyTankLevel()
		{
			if (GameConstants.roundCounter > 1 && GameConstants.roundCounter <= 10) {
				int randomNumber = Random.Range (0,2); 
				if (randomNumber == 2) {
					navMeshAgent.speed = 1f + (GameConstants.roundCounter / 10f); //	
					if (navMeshAgent.speed > 2.5f) {
						navMeshAgent.speed = 2.5f; 
					}
				} else {
					navMeshAgent.speed = 1f; 
				}
			}
			else if (GameConstants.roundCounter > 10) {
				int randomNumber = Random.Range (0,1); 
				if (randomNumber == 1) {
					navMeshAgent.speed = 1f + (GameConstants.roundCounter / 10f); //	
					if (navMeshAgent.speed > 2.5f) {
						navMeshAgent.speed = 2.5f; 
					} 
				} else {
					navMeshAgent.speed = 1f; 
				}
			}
		}
			
		private void SelectNewFriendlyTankTarget()
		{
			GameObject[] friendlies = GameObject.FindGameObjectsWithTag("FriendlyTank");
            List<GameObject> activeFriendlies = new List<GameObject>();

            foreach (var friendly in friendlies)
            {
                if (friendly.gameObject.GetComponent<FriendlyTankAI>().IsHidden == false)
                {
                    activeFriendlies.Add(friendly);
                }
            }

            if (activeFriendlies.Count > 0) {
				this.selectedTarget = activeFriendlies[Random.Range(0, activeFriendlies.Count)].transform;

                if (this.selectedTarget)
                {
                    ConfigureNewAITarget();
                    if (this.gameObject.activeSelf == true && !isInitialSelectedTarget)
                    {
                        this.turretControlsScript.RotateTurretToNewSelectedTarget();
                    }
                } else
                {
                    EnemyTankSelectsNewTarget();
                }
			}
            else {
			    EnemyTankSelectsNewTarget(); 
			}
		}

		private void SelectNewBaseTarget()
		{
			if (healthBaseScript != null) {
				healthBaseScript.OnBaseDestroyedByThisTank -= EnemyTankSelectsNewTarget;
			}

			GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
			if (bases.Length > 0) {
				selectedTarget = bases[Random.Range(0,bases.Length)].transform;
				
				this.GetTurretControlScript ();
				this.turretControlsScript.selectedTarget = selectedTarget;
				this.GetPointerToNewTargetScript ();
                if (this.pointerToNewTargetScript)
                {
                    this.pointerToNewTargetScript.selectedTarget = selectedTarget;
                }

				Transform baseRoot = selectedTarget.root;
                Transform rotationTransform = baseRoot.Find("Rotation");
                Transform colliderHealth = rotationTransform.Find("BaseColliderHealth");
				healthBaseScript = colliderHealth.gameObject.GetComponent<HealthBase>();
                healthBaseScript.OnBaseDestroyedByThisTank += EnemyTankSelectsNewTarget;
				if (this.gameObject.activeSelf == true && !isInitialSelectedTarget) {
					this.turretControlsScript.RotateTurretToNewSelectedTarget ();	
				}

				int decisionToDestroyBase = Random.Range (0,2);
				if (decisionToDestroyBase > 1) {
					this.wantsToDestroyBase = true;
				}
			} else {
				EnemyTankSelectsNewTarget(); 
			} 
		}

		public void EnemyTankSelectsNewTarget()
		{
            if (GameConstants.GameplayState.GameOver)
            {
                return;
            }

            if (AttackPlayerTankOnly)
            {
                SelectPlayerAsNewTarget();
                return;
            }

            if (AttackFriendlyTanksOnly)
            {
                if (SpawnFriendlyTanksManager.friendlyTanksHadBeenAdded == true)
                {
                    SelectNewFriendlyTankTarget();
                }
                else
                {
                    SelectNewBaseTarget();
                }
                return;
            }

            if (AttackBaseOnly)
            {
                SelectNewBaseTarget();
                return;
            }

            AttackTarget attackTarget = TargetToAttackDecision(); 
			
            switch (attackTarget)
            {
                case AttackTarget.targetPlayer:
                    SelectPlayerAsNewTarget();
                    break;
                case AttackTarget.targetBase:
                    SelectNewBaseTarget();
                    break;
                case AttackTarget.targetFriendly:
                    if (SpawnFriendlyTanksManager.friendlyTanksHadBeenAdded == true)
                    {
                        SelectNewFriendlyTankTarget();
                    }
                    else
                    {
                        SelectNewBaseTarget();
                    }
                    break;
                default:
                    break;
            }

            isInitialSelectedTarget = false; 
		}

        private AttackTarget TargetToAttackDecision()
        {
            int randomNumber = Random.Range(0,100);
            if (randomNumber >= 40)
            {
                return AttackTarget.targetBase;
            }
            else if (randomNumber >= 20 && randomNumber < 40)
            {
                return AttackTarget.targetFriendly;
            }
            else
            {
                return AttackTarget.targetPlayer;
            }
        }

        private void SelectPlayerAsNewTarget()
        {
            if (player)
            {
                Transform tankBodyRotation = player.transform.Find(GameConstants.kTankBodyRotation);
                selectedTarget = player.transform;
                this.GetTurretControlScript();

                Transform turretPlayer = tankBodyRotation.Find("TurretPlayer");
                if (turretPlayer)
                {
                    this.turretControlsScript.selectedTarget = turretPlayer;
                    this.GetPointerToNewTargetScript();
                    if (this.pointerToNewTargetScript)
                    {
                        this.pointerToNewTargetScript.selectedTarget = turretPlayer;
                    }
                }

                if (this.gameObject.activeSelf == true && !isInitialSelectedTarget)
                {
                    this.turretControlsScript.RotateTurretToNewSelectedTarget();
                }
            } else
            {
                player = GameObject.FindGameObjectWithTag("Player");
                EnemyTankSelectsNewTarget();
            }
        }

		public void ChangeTargetToPlayer()
		{
            if (AttackFriendlyTanksOnly || AttackBaseOnly)
            {
                return;
            }
            
			if (player) {
				if (this.selectedTarget != player.transform) {
					this.selectedTarget = player.transform;
					Transform tankBodyRotation = player.transform.Find(GameConstants.kTankBodyRotation); 
					//isAttackingTank = true; 
					isTargetEngaged = false;

					this.GetTurretControlScript ();
					Transform turretPlayer = tankBodyRotation.Find("TurretPlayer");
					this.turretControlsScript.selectedTarget = turretPlayer;
					this.GetPointerToNewTargetScript ();
                    if (this.pointerToNewTargetScript)
                    {
                        this.pointerToNewTargetScript.selectedTarget = turretPlayer;
                    }

					if (this.gameObject.activeSelf == true && !isInitialSelectedTarget) {
						this.turretControlsScript.RotateTurretToNewSelectedTarget ();	
					}
				}
			}
		}

		public override void ChangeTargetToShooter(Transform shooter)
		{
            if (AttackFriendlyTanksOnly || AttackBaseOnly)
            {
                return;
            }

            if (!wantsToDestroyBase) {
				base.ChangeTargetToShooter (shooter);

				ConfigureNewAITarget ();
				if (this.gameObject.activeSelf == true && !isInitialSelectedTarget) {
					this.turretControlsScript.RotateTurretToNewSelectedTarget ();	
				}	
			}
		}

		public override void ConfigureNewAITarget()
		{
			if (healthAIScript != null) {
				healthAIScript.OnAITankDestroyed -= EnemyTankSelectsNewTarget; 
			}
			GetTurretFromSelectedTankTarget ();
			healthAIScript = this.selectedTarget.gameObject.GetComponent<HealthAI> ();
			healthAIScript.OnAITankDestroyed += EnemyTankSelectsNewTarget; 
		}

		private void GetTurretFromSelectedTankTarget()
		{
			Transform tankBodyRotation = this.selectedTarget.Find (GameConstants.kTankBodyRotation); 
			this.GetTurretControlScript ();
			Transform turretTransform = tankBodyRotation.Find ("Turret");
			this.turretControlsScript.selectedTarget = turretTransform;
			this.GetPointerToNewTargetScript ();
            if (this.pointerToNewTargetScript)
            {
                this.pointerToNewTargetScript.selectedTarget = turretTransform;
            }
			
		}

		private void GetTurretControlScript()
		{
			if (this.turretControlsScript != null) {
				return; 
			}
				
			Transform tankBodyRotation = this.transform.Find(GameConstants.kTankBodyRotation);
			Transform turret = tankBodyRotation.Find("Turret");
			TurretControls tcScript = turret.gameObject.GetComponent<TurretControls>();
			tcScript.isEnemyTank = true;
			tcScript.isFriendlyTank = false;
			tcScript.fireRate = 5f; 
			tcScript.artificialIntelligence = this;
			this.turretControlsScript = tcScript;
		}
			
	} // end class
}
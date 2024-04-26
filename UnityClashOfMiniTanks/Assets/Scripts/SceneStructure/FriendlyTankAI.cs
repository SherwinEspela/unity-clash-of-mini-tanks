using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace CMT.AI
{
	public class FriendlyTankAI : ArtificialIntelligence {

		void Start()
		{
			navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
		}

		void Update () {
			if (!GameConstants.GameplayState.GameOver) {
                if (this.selectedTarget)
                {
                    distanceFromTarget = Vector3.Distance(selectedTarget.position, this.transform.position);

                    if (distanceFromTarget < evadeRange)
                    {
                        Evade(selectedTarget);
                    }
                    else
                    {
                        Follow();
                    }

                    if (distanceFromTarget < firingRange)
                    {
                        isTargetEngaged = true;

                        this.GetTurretControlScript();
                        turretControlsScript.FireAI();
                    }
                }
			}
		}
	
		void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == GameConstants.kTagEnemyTank) {
				ChangeTargetToEnemyTank(col.transform); 
			}
		}

		public void SelectNewEnemyTankTarget()
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag(GameConstants.kTagEnemyTank);
            List<GameObject> activeEnemies = new List<GameObject>();

            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.GetComponent<EnemyTankAI>().IsHidden == false)
                {
                    activeEnemies.Add(enemy);
                }
            }

			if (activeEnemies.Count > 0) {
				this.selectedTarget = activeEnemies[Random.Range (0, activeEnemies.Count)].transform;

                if (this.selectedTarget)
                {
                    this.GetPointerToNewTargetScript();
                    this.pointerToNewTargetScript.selectedTarget = this.selectedTarget;

                    ConfigureNewAITarget();
                    if (this.gameObject.activeSelf == true && !isInitialSelectedTarget)
                    {
                        this.turretControlsScript.RotateTurretToNewSelectedTarget();
                    }
                } else
                {
                    this.SelectNewEnemyTankTarget();
                }
			} else
            {
                this.SelectNewEnemyTankTarget();
            }

			isInitialSelectedTarget = false;
		}

		void ChangeTargetToEnemyTank(Transform newTarget)
		{
			this.selectedTarget = newTarget; 
			this.GetPointerToNewTargetScript ();
			this.pointerToNewTargetScript.selectedTarget = this.selectedTarget;
			ConfigureNewAITarget ();
			if (this.gameObject.activeSelf == true && !isInitialSelectedTarget) {
				this.turretControlsScript.RotateTurretToNewSelectedTarget ();	
			}
		}

		public override void ChangeTargetToShooter(Transform shooter)
		{
			base.ChangeTargetToShooter (shooter);
			ConfigureNewAITarget ();
			if (this.gameObject.activeSelf == true && !isInitialSelectedTarget) {
				this.turretControlsScript.RotateTurretToNewSelectedTarget ();	
			}
		}

        public override void ConfigureNewAITarget()
        {
            if (healthAIScript != null)
            {
                healthAIScript.OnAITankDestroyed -= SelectNewEnemyTankTarget;
            }
            GetTurretFromSelectedTankTarget();
            healthAIScript = this.selectedTarget.gameObject.GetComponent<HealthAI>();
            healthAIScript.OnAITankDestroyed += SelectNewEnemyTankTarget;
        }

        private void GetTurretFromSelectedTankTarget()
        {
            Transform tankBodyRotation = this.selectedTarget.Find(GameConstants.kTankBodyRotation);
            this.GetTurretControlScript();
            this.turretControlsScript.selectedTarget = tankBodyRotation.Find(GameConstants.kTurret);
        }

        private void GetTurretControlScript()
		{
            if (turretControlsScript != null)
            {
                return;
            }

            Transform tankBodyRotation = this.transform.Find(GameConstants.kTankBodyRotation);
            Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
            TurretControls tcScript = turret.gameObject.GetComponent<TurretControls>();
            tcScript.isEnemyTank = false;
			tcScript.isFriendlyTank = true;
			tcScript.fireRate = 3.5f; 
			tcScript.artificialIntelligence = this;
			this.turretControlsScript = tcScript;
		}
	}
}
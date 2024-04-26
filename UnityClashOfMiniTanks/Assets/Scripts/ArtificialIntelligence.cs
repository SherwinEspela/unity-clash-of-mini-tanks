using UnityEngine; 
using UnityEngine.AI;

namespace CMT.AI
{
	public class ArtificialIntelligence : MonoBehaviour {
			
		// private
		private float xPos; 
		private float zPos; 

		// protected
		protected float distanceFromTarget;
		protected NavMeshAgent navMeshAgent;
		protected HealthAI healthAIScript;

        // TODO: set back to protected
        public Transform selectedTarget;
		protected bool isTargetEngaged = false;
		protected float firingRange = 7f;
		protected float evadeRange = 5f;
		protected bool isInitialSelectedTarget = true; 

		// constants
		private const float kEvadeRangeNormal = 5f;
		private const float kEvadeRangeTargetBlocked = 0.0f;
		private const float kFiringRangeNormal = 7f;
		private const float kFiringRangeTargetBlocked = 0.0f;
		protected const float kFarRange = 9f;

		// public
		public TurretControls turretControlsScript { get; set; }
		public PointerToNewTarget pointerToNewTargetScript { get; set; }
		public bool HasEnteredBaseZone { get; set; }
		// public delegate void NewTargetDelegate();

        public bool IsHidden { get; set; }

        public enum AttackTarget
        {
            targetPlayer,
            targetBase,
            targetFriendly
        }

        void Start() {
			navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
		}

		protected void Follow()
		{
			navMeshAgent.destination = this.selectedTarget.position; //
		}
		
		protected void Evade(Transform target)
		{	
			if (target.position.x > this.transform.position.x) {
				// target is located east
				xPos = this.transform.position.x - evadeRange; 
			} else {
				// target is located west
				xPos = this.transform.position.x + evadeRange; 
			}
			
			if (target.position.z > this.transform.position.z) {
				// target is located north
				zPos = this.transform.position.z - evadeRange; 
			} else {
				// target is located south
				zPos = this.transform.position.z + evadeRange; 
			}
			
			navMeshAgent.destination = new Vector3(xPos,this.transform.position.y,zPos); 
		}

		public virtual void ChangeTargetToShooter(Transform shooter)
		{
			this.selectedTarget = shooter;
		}

        public Transform GetSelectedTarget()
        {
            return this.selectedTarget;
        }

		public virtual void ConfigureNewAITarget() {

        }
    
        public void SetEvadeAndFiringRangeToTargetBlocked()
		{
			this.evadeRange = kEvadeRangeTargetBlocked;
			this.firingRange = kFiringRangeTargetBlocked;
		}

		public void SetEvadeAndFiringRangeToNormal()
		{
			this.evadeRange = kEvadeRangeNormal;
			this.firingRange = kFiringRangeNormal;
		}

		protected void GetPointerToNewTargetScript()
		{
			if (this.pointerToNewTargetScript != null) {
				return;
			}

			Transform tankBodyRotation = this.transform.Find(GameConstants.kTankBodyRotation);
			Transform pointerGO = tankBodyRotation.Find (GameConstants.kPointerToNewTarget);

			if (pointerGO) {
				this.pointerToNewTargetScript = pointerGO.gameObject.GetComponent<PointerToNewTarget> ();	
			} else {
				// add pointerGO
				GameObject pointerToNewTargetGO = new GameObject();
				pointerToNewTargetGO.name = GameConstants.kPointerToNewTarget;

				Transform turret = tankBodyRotation.Find (GameConstants.kTurret);
				pointerToNewTargetGO.transform.position = turret.position;
				pointerToNewTargetGO.transform.rotation = turret.rotation;
				pointerToNewTargetGO.transform.SetParent (tankBodyRotation);

				this.pointerToNewTargetScript = pointerToNewTargetGO.gameObject.AddComponent<PointerToNewTarget> ();	
			}
		}

        public void SetMovementSpeed(float newSpeed)
        {
            if (!navMeshAgent)
            {
                navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
            }
            navMeshAgent.speed = newSpeed;
        }

	} // end class
}
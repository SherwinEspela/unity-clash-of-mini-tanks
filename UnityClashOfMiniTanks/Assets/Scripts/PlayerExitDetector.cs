using UnityEngine;
using System.Collections;
using CMT.AI;

public class PlayerExitDetector : MonoBehaviour {
	
	public ArtificialIntelligence aiScript;

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player") {

			if (aiScript) {
				if (aiScript is EnemyTankAI) {
					EnemyTankAI enemyTankAI = aiScript as EnemyTankAI;
					enemyTankAI.EnemyTankSelectsNewTarget ();
				}
			}
		}
	}
}

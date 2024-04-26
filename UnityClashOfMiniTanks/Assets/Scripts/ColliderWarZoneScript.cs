using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMT.AI;

public class ColliderWarZoneScript : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals (GameConstants.kTagEnemyTank)) {
			EnemyTankAI aiScript = col.gameObject.GetComponent<EnemyTankAI> ();
			aiScript.HasEnteredBaseZone = true;
		} else if (col.tag.Equals (GameConstants.kTagFriendlyTank)) {
			FriendlyTankAI aiScript = col.gameObject.GetComponent<FriendlyTankAI> ();
			aiScript.HasEnteredBaseZone = true;	
		}
	}
}

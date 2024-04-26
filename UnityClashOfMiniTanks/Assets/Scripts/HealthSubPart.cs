using UnityEngine;
using System.Collections;
using CMT.AI;

public class HealthSubPart : MonoBehaviour {

//	private const float cannonDamage = 10f; 
//	private const float cannonDamageOneShot = 500f; 
//
//	public GameObject mainObject; 
//
//	void OnCollisionEnter(Collision col)
//	{
//		if (col.gameObject.name.Contains ("cannonPlayer") || col.gameObject.name.Contains ("cannonPlayer_powerUpOneShot")) {
//
////			Debug.Log ("Player cannon hit...");
////
////			ConstantValues.isShotByPlayer = true; 
////			ScoreManager.enemyHit++;
////
////			if (PowerUpManager.increasePlayerCannonPowerIsActivated) {
////				mainObject.SendMessage ("ReduceHealthValue", cannonDamageOneShot, SendMessageOptions.DontRequireReceiver);		
////			} else {
////				ReduceHealthFromMainObject ();
////			}	
//		} else if (col.gameObject.name.Contains ("cannonAI") || col.gameObject.name.Contains ("cannonFriendly")) {
//			
//			ConstantValues.isShotByPlayer = false; 
//			ReduceHealthFromMainObject ();
//		}
//	}
//
//	void ReduceHealthFromMainObject()
//	{
//		mainObject.SendMessage("ReduceHealthValue",cannonDamage,SendMessageOptions.DontRequireReceiver); 
//	}
}
using UnityEngine;
using System.Collections;

public class HealthSubPartPlayer : MonoBehaviour {

	public GameObject mainObject; 
	private float cannonDamage = 20f; 
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name.Contains("cannon")) {

			if (GameConstants.roundCounter > 1 && GameConstants.roundCounter <= 10) {
				int randomNumber = Random.Range (0, 2); 
				if (randomNumber == 2) {
					cannonDamage += GameConstants.roundCounter;
					if (cannonDamage > 35f){
						cannonDamage = 35f; 
					}
				}
			}
			else if (GameConstants.roundCounter > 10) {
				int randomNumber = Random.Range (0, 1); 
				if (randomNumber == 1) {
					cannonDamage += GameConstants.roundCounter;
					if (cannonDamage > 35f){
						cannonDamage = 35f; 
					}
				}
			}

			ReduceHealthFromMainObject();
		}
	}
	
	void ReduceHealthFromMainObject()
	{
		//print (cannonDamage);
		//Debug.Break (); 
		mainObject.SendMessage("ReduceHealthValue",cannonDamage,SendMessageOptions.DontRequireReceiver); 
	}
}
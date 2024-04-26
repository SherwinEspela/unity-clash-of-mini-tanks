using UnityEngine;
// using System.Collections;
// using PathologicalGames; 

public class DespawnExploderObject : MonoBehaviour {
	
	public void InvokeDespawnThisObject()
	{
		// Invoke("DespawnThisObject",2f); 
	}
	
	void DespawnThisObject()
	{  
		//Debug.Log (this.gameObject.name + " has been despawned..."); 
		// PoolManager.Pools ["ExploderObjects"].Despawn (this.transform); 
	}

	void OnDespawned()
	{
        /*
		Transform tankBodyRotation = this.transform.Find ("TankBodyRotation");
		tankBodyRotation.gameObject.SetActive (true);
		int children = tankBodyRotation.childCount; 
		for (int i = 0; i < children; i++) {
			tankBodyRotation.GetChild(i).gameObject.SetActive(true); 	
		}
        */
	}
}

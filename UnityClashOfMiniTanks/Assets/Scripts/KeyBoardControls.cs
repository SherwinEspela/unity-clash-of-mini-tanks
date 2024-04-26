using UnityEngine;

public class KeyBoardControls : MonoBehaviour {

	public TurretControls tcScript; 
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			tcScript.Fire();
		}
	}
}

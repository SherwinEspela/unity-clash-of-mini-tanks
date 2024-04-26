using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerToNewTarget : MonoBehaviour {

	public Transform selectedTarget;
	
	// Update is called once per frame
	void Update () {
		if (this.selectedTarget) {
			this.transform.LookAt (this.selectedTarget);	
		}
	}
}

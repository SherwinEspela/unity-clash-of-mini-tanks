using UnityEngine;
using System.Collections;

public class TagLookAtScript : MonoBehaviour {

	private Transform target; 

	void Start()
	{
		target = GameObject.Find ("Main Camera").transform; 
	}

	// Update is called once per frame
	void Update () {
		if (target) {
			this.transform.LookAt (target);	
		}
	}
}

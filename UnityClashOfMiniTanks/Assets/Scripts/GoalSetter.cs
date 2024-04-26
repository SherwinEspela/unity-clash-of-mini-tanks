using UnityEngine;
using System.Collections;

public class GoalSetter : MonoBehaviour {

	private Transform goal; 

	// Use this for initialization
	void Start () {
		goal = GameObject.FindGameObjectWithTag("Player").transform; 
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<UnityEngine.AI.NavMeshAgent>().destination = goal.position; 
	}
}

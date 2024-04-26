using UnityEngine;
using System.Collections;

public class AvoidCollisionScript : MonoBehaviour {
	
	public int layer1; 
	public int layer2; 
	
	void Start()
	{
		Physics.IgnoreLayerCollision(layer1,layer2, true);
	}
}

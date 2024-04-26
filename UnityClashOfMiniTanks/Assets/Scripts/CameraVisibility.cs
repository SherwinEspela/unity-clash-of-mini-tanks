using UnityEngine;
using System.Collections;

public class CameraVisibility : MonoBehaviour {

	public bool isVisibleInCamera = false; 

	void OnBecameVisible()
	{
		isVisibleInCamera = true; 
	}
	
	void OnBecameInvisible()
	{
		isVisibleInCamera = false; 
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CMT.AI;

public class BorderTrigger : MonoBehaviour {

	public GameObject gameManager; 

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals ("Player")) { 	
			gameManager.SendMessage ("HidePanelCurtain", SendMessageOptions.DontRequireReceiver); // from GameMenuManager.cs
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag.Equals("Player")) {
			gameManager.SendMessage("ShowPanelCurtain",SendMessageOptions.DontRequireReceiver); // from GameMenuManager.cs
		}

	}
}
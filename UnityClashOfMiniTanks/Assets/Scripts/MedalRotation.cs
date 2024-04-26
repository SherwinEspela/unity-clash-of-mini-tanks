using UnityEngine;

public class MedalRotation : MonoBehaviour {

	private float rotateSpeed = 3f;

	// Update is called once per frame
	void Update () {
		this.transform.Rotate(0,rotateSpeed,0);
	}
}
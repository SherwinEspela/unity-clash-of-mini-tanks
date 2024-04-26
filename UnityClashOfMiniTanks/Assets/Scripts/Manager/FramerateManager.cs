using UnityEngine;
using System.Collections;

public class FramerateManager : MonoBehaviour {

	void Start () {
#if UNITY_EDITOR
		// do nothing
#else
		Application.targetFrameRate = 30;
#endif
	}
}
using UnityEngine;
using System.Collections;

public class MoveTrackScript : MonoBehaviour {

	public int currentTTIdx = 0;
	
	public Texture[] trackTexturs;
	
	public float speed = 40f; //  km/h
	
	public float moveTick = 0.1f;
	
	public static int gearStatus = 0;
	
	void Update()
	{
		switch (gearStatus) {
		case 0 : 
			// Neutral. do nothing
			break;
			
		case 1 : 
			// forward
			
			if (speed < 1)
				speed = 1;
			
			if (Time.time > moveTick) {
				currentTTIdx++;
				if (currentTTIdx >= trackTexturs.Length)
					currentTTIdx = 0;
				
				GetComponent<Renderer>().material.mainTexture = trackTexturs[currentTTIdx]; 
				
				// One Texture made 4cm move
				moveTick = Time.time + 4 / (speed * 1000 / (60 * 60) * 100);
			}
			
			break;
			
		case 2 : 
			// backward
			if (speed < 1)
				speed = 1;
			
			if (Time.time > moveTick) {
				currentTTIdx--;
				if (currentTTIdx < 0)
					currentTTIdx = trackTexturs.Length - 1;
				
				GetComponent<Renderer>().material.mainTexture = trackTexturs[currentTTIdx]; 
				
				moveTick = Time.time + 4 / (speed * 1000 / (60 * 60) * 100);
			}
			
			break;	
		}	
	}
}

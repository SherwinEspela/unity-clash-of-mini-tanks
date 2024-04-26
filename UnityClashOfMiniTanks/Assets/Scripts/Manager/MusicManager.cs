using UnityEngine;
using System.Collections.Generic; 
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] musicTracks;

	void Start()
	{
		SelectMusic();         
	}

	void SelectMusic()
	{
		AudioClip music = musicTracks[Random.Range(0,musicTracks.Length)];
		float musicLengthInSeconds = music.length; 
		this.gameObject.GetComponent<AudioSource>().PlayOneShot(music);
		Invoke("SelectMusic",musicLengthInSeconds); 
	}
}

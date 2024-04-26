using UnityEngine;
using System.Collections;

public class MissionOutcomeAudioManager : MonoBehaviour {

	public AudioClip[] gameOverClips;
	public AudioClip[] victoryClips;

    public void PlayGameOverDelayed()
    {
        StartCoroutine(PlayGameOverCouroutine());
    }

    IEnumerator PlayGameOverCouroutine()
    {
        yield return new WaitForSeconds(2.0f);
        PlayGameOver();
    }

    public void PlayGameOver()
	{
		if (gameObject.GetComponent<AudioSource>()) {
			AudioClip clip = gameOverClips[Random.Range(0,gameOverClips.Length)]; 
			gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
		} 
	}
	
	public void PlayVictory()
	{
		if (gameObject.GetComponent<AudioSource>()) {
			AudioClip clip = victoryClips[Random.Range(0,victoryClips.Length)]; 
			gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
		} 
	}
}

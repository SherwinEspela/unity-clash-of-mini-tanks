using UnityEngine;
using System.Collections;

public class VoiceResponseManager : MonoBehaviour {

	public AudioClip[] playerIntroClips;
	public AudioClip[] cannonReadyClips; 
	public AudioClip[] playerShotClips; 
	public AudioClip[] playerHappyClips; 
	public AudioClip[] baseUnderAttackClips; 
	public AudioClip[] baseDestroyedClips;
	public AudioClip[] friendlyTankShotClips; 
	public AudioClip[] friendlyTankSpawnClips; 
	public AudioClip[] friendlyTankDespawnClips;
	public AudioClip borderWarningClip; 
	public AudioClip getReadyClip; 
	public AudioClip goClip; 

	private float nextPlayerTalk;
	private float playerTalkRate = 0.7f; 

	private float nextBaseTalk; 
	private float baseTalkRate = 15f; 

	private float nextFriendlyTankTalk; 
	private float friendlyTankTalkRate = 5f;

	private float nextBorderWarningTalk; 
	private float borderWarningTalkRate = 7f;

	void Start()
	{
		Invoke("PlayGetReadyClip",4.4f); 
		Invoke("PlayGoClip",6.28f); 
	}

	void PlayGetReadyClip()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot(getReadyClip); 	
	}

	void PlayGoClip()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot(goClip); 	
	}

	private void PlayPlayerIntro()
	{
		if (Time.time > nextPlayerTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = playerIntroClips[Random.Range(0,playerIntroClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}
			nextPlayerTalk = Time.time + playerTalkRate; 
		}
	}

	public void PlayCannonReady()
	{
		if (!GameConstants.GameplayState.GameOver){
			if (Time.time > nextPlayerTalk) {
				if (gameObject.GetComponent<AudioSource>()) {
					AudioClip clip = cannonReadyClips[Random.Range(0,cannonReadyClips.Length)]; 
					gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
				}
				nextPlayerTalk = Time.time + playerTalkRate; 
			}
		}
	}

	public void PlayPlayerShot()
	{
		if (Time.time > nextPlayerTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = playerShotClips[Random.Range(0,playerShotClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}
			nextPlayerTalk = Time.time + playerTalkRate;
		}
	}

	public void PlayPlayerHappy()
	{
		if (!GameConstants.GameplayState.GameOver) {
			if (Time.time > nextPlayerTalk) {
				if (gameObject.GetComponent<AudioSource>()) {
					AudioClip clip = playerHappyClips[Random.Range(0,playerHappyClips.Length)]; 
					gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
				}
				nextPlayerTalk = Time.time + playerTalkRate;
			}
		}
	}

	public void PlayBaseUnderAttack()
	{
		if (Time.time > nextBaseTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = baseUnderAttackClips[Random.Range(0,baseUnderAttackClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}
			nextBaseTalk = Time.time + baseTalkRate;
		}
	}

	public void PlayBaseDestroyed()
	{
		if (gameObject.GetComponent<AudioSource>()) {
			AudioClip clip = baseDestroyedClips[Random.Range(0,baseDestroyedClips.Length)]; 
			gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
		} 
	}

	public void PlayFriendlyTankShot()
	{
		if (Time.time > nextFriendlyTankTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = friendlyTankShotClips[Random.Range(0,friendlyTankShotClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}

			nextFriendlyTankTalk = Time.time + friendlyTankTalkRate;
		}
	}

	public void PlayFriendlyTankSpawn()
	{
		if (Time.time > nextFriendlyTankTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = friendlyTankSpawnClips[Random.Range(0,friendlyTankSpawnClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}
			nextFriendlyTankTalk = Time.time + friendlyTankTalkRate;
		}
	}

	public void PlayFriendlyTankDespawn()
	{
		if (Time.time > nextFriendlyTankTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				AudioClip clip = friendlyTankDespawnClips[Random.Range(0,friendlyTankDespawnClips.Length)]; 
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip); 	
			}
			nextFriendlyTankTalk = Time.time + friendlyTankTalkRate;
		}
	}

	public void PlayBorderWarning()
	{
		if (Time.time > nextBorderWarningTalk) {
			if (gameObject.GetComponent<AudioSource>()) {
				gameObject.GetComponent<AudioSource>().PlayOneShot(borderWarningClip); 	
			}
			nextBorderWarningTalk = Time.time + borderWarningTalkRate;
		}
	}
}

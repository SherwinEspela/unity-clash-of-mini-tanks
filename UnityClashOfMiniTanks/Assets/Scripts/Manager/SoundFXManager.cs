using UnityEngine;
using System.Collections;

public class SoundFXManager : MonoBehaviour {

	public AudioClip[] cannonFireSounds; 
	public AudioClip[] cannonHitSounds; 
	public AudioClip[] explosionSounds; 
	public AudioClip[] baseExplosionSounds; 
	public AudioClip[] cannonReloadSounds; 
	public AudioClip cannonFireSound_powerUpOneShotKill; 
	public GameObject warningLowHealth; 
	public AudioSource audioSourceWarningLowBaseHealth; 
	public AudioSource audioSourceTrophyRewardMusic;
	public AudioSource audioSourceTrophyRewardAudienceCheer;
	public AudioSource audioSourceEventMessage;

    private void OnEnable()
    {
        Cannon.OnCannonCollided += PlayCannonHitSound;
    }

    private void OnDisable()
    {
        Cannon.OnCannonCollided -= PlayCannonHitSound;
    }

    public void PlayCannonFireSound()
	{
		AudioClip clip = cannonFireSounds[Random.Range(0,cannonFireSounds.Length)]; 
		gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public void PlayCannonHitSound()
	{
		AudioClip clip = cannonHitSounds[Random.Range(0,cannonHitSounds.Length)]; 
		gameObject.GetComponent<AudioSource>().PlayOneShot(clip); //
	}

	public void PlayExplosionSound()
	{
		AudioClip clip = explosionSounds[Random.Range(0,explosionSounds.Length)]; 
		gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public void PlayBaseExplosionSound()
	{
		AudioClip clip = baseExplosionSounds[Random.Range(0,baseExplosionSounds.Length)]; 
		gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public void PlayCannonReloadSound()
	{
		AudioClip clip = cannonReloadSounds[Random.Range(0,cannonReloadSounds.Length)];
		gameObject.GetComponent<AudioSource>().PlayOneShot (clip); 
	}

	public void PlayCannonFireSoundPowerUpOneShotKill()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot (cannonFireSound_powerUpOneShotKill); 
	}

	public void PlayWarningLowHealth()
	{
		if (!GameConstants.GameplayState.GameOver) {
			warningLowHealth.SetActive (true);	
		}
	}

	public void StopWarningLowHealth()
	{
		warningLowHealth.SetActive (false); 
	}

	public void PlayWarningLowBaseHealth()
	{
		if (!GameConstants.GameplayState.GameOver) {
			audioSourceWarningLowBaseHealth.Play();
		}
	}

    public void PlayTrophyRewardMusic()
    {
        audioSourceTrophyRewardMusic.Play();
        StartCoroutine(PlayTrophyRewardAudienceCheer());
    }

	IEnumerator PlayTrophyRewardAudienceCheer()
	{
        yield return new WaitForSeconds(0.75f);

		audioSourceTrophyRewardAudienceCheer.Play ();
	}

	public void PlayEventMessage()
	{
		audioSourceEventMessage.Play ();
	}
}

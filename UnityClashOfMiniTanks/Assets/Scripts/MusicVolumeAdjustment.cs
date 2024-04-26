using UnityEngine;

public class MusicVolumeAdjustment : MonoBehaviour {

	public Animator animatorMusic; 

	public void TriggerTurnOnMusic()
	{
		animatorMusic.SetTrigger ("TriggerTurnOn"); 
	}

	public void TriggerTurnOffMusic()
	{
		animatorMusic.SetTrigger ("TriggerTurnOff"); 
	}

	public void TurnDownMusic()
	{
		animatorMusic.SetTrigger ("TriggerTurnDown"); 
	}

	public void TriggerTurnDownToOffMusic()
	{
		animatorMusic.SetTrigger ("TriggerTurnDownToOff"); 
	}

	public void TurnUpMusic()
	{
		animatorMusic.SetTrigger ("TriggerTurnUp"); 
	}
}

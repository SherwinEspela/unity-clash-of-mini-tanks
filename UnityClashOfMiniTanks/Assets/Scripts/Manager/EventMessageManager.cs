using UnityEngine;
using TMPro;

public class EventMessageManager : MonoBehaviour {

    [Header("General")]
    [SerializeField] Animation animationTextEventMessage;
    [SerializeField] AnimationClip animationClipTextEventMessage; 
    [SerializeField] TextMeshProUGUI textProEventMessage;
    
	private bool hasNotShownEventMessage = true; 

	public void ShowEventMessage(string eventMessage)
	{
		if (!GameConstants.GameplayState.GameOver) {
			
            if (hasNotShownEventMessage)
            {
                this.gameObject.GetComponent<SoundFXManager>().PlayEventMessage();
                textProEventMessage.text = eventMessage;
                animationTextEventMessage.Play(animationClipTextEventMessage.name);
                hasNotShownEventMessage = false;
                Invoke("CanShowMessageAgain", 2.0f);
            }
        }
	}

	void CanShowMessageAgain()
	{
		hasNotShownEventMessage = true; 
	}
}

using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class SelectTankManager : MonoBehaviour {
	
	public Animator[] tankAnimators; 
	private int tankIndex = 0; 
	public Camera cameraTanks; 

	public TankUnlockManager tankUnlockManagerScript;
    public Button buttonBack;
    public Button buttonForward;
    public Button buttonSelect; 

	void Start()
	{
		tankIndex = 0; 
	}

	public void ShowTheFirstTank()
	{
		tankAnimators [tankIndex].SetTrigger ("TriggerRotateLoop");
		tankAnimators [tankIndex].SetTrigger ("TriggerRightToCenter"); 
	}

	public void DismissCurrentSelectedTank()
	{
		tankAnimators [tankIndex].SetTrigger ("TriggerCenterToRight");

		tankIndex = 0;
		tankUnlockManagerScript.SetCurrentSelectedTank (tankIndex); 
		TankSelectionManager.selectedTankPlayer = 1;

		Invoke ("HideCameraForTanks",0.34f); 
	}

	public void ShowCameraForTanks()
	{
		cameraTanks.gameObject.SetActive (true); 
	}

	void HideCameraForTanks()
	{
		cameraTanks.gameObject.SetActive (false);
		foreach (var item in tankAnimators) {
			item.SetTrigger("TriggerCenterToRight");
		}
	}

	public void NextTank()
	{
		TankSelectionManager.selectedTankPlayer++; 
		if (TankSelectionManager.selectedTankPlayer > tankAnimators.Length - 1) {
			TankSelectionManager.selectedTankPlayer = tankAnimators.Length - 1; 
		}

		if (tankIndex < tankAnimators.Length - 1) {
			tankIndex++;
			if (tankIndex > tankAnimators.Length - 1) {
				tankIndex = tankAnimators.Length - 1; 	
			}
			
			for (int i = 0; i < tankAnimators.Length; i++) {
				if (tankIndex == i) {
					tankAnimators[i-1].SetTrigger("TriggerCenterToLeft");
					StartCoroutine(SetTankToRotateStart(tankAnimators[i-1])); 
					tankAnimators[i].SetTrigger("TriggerRightToCenter");
					tankAnimators[i].SetTrigger("TriggerRotateLoop");
					break; 
				}
			}	
		}
	
		tankUnlockManagerScript.ShowTankStatus (tankIndex);
	}

	public void PreviousTank()
	{
		TankSelectionManager.selectedTankPlayer--; 
		if (TankSelectionManager.selectedTankPlayer < 0) {
			TankSelectionManager.selectedTankPlayer = 0; 
		}

		if (tankIndex > 0) {
			tankIndex--;
			if (tankIndex < 0) {
				tankIndex = 0; 	
			}
			
			for (int i = 0; i < tankAnimators.Length; i++) {
				if (tankIndex == i) {
					tankAnimators[i+1].SetTrigger("TriggerCenterToRight");
					StartCoroutine(SetTankToRotateStart(tankAnimators[i+1]));
					tankAnimators[i].SetTrigger("TriggerLeftToCenter");
					tankAnimators[i].SetTrigger("TriggerRotateLoop");
					break; 
				}
			}	
		}


		tankUnlockManagerScript.ShowTankStatus (tankIndex);
    }

	IEnumerator SetTankToRotateStart(Animator tankAnimator)
	{
		yield return new WaitForSeconds(0.2f);

		tankAnimator.SetTrigger ("TriggerRotateStart"); 
	}

    public void TemporarilyDisableButtons()
    {
        buttonBack.enabled = false;
        buttonForward.enabled = false;
        buttonSelect.enabled = false; 
        StartCoroutine(ReenableTheButtons());
    }

    IEnumerator ReenableTheButtons()
    {
        yield return new WaitForSeconds(0.42f);

        buttonBack.enabled = true;
        buttonForward.enabled = true;
        buttonSelect.enabled = true;
    }
}

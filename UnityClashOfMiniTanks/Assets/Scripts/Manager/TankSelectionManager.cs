using UnityEngine;

public class TankSelectionManager : MonoBehaviour {

	public static int selectedTankPlayer = 0; 

	void Start()
	{
		selectedTankPlayer = 0; 
	}

	public void SelectTank(int tank)
	{
		selectedTankPlayer = tank;
	}
}
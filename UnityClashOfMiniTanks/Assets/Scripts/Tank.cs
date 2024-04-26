using UnityEngine;
using System.Collections;

public class Tank
{
    public string tankName { get; set; }
    public int indexNumber { get; set; }
    public int gearValue { get; set; }
    public int medalValue { get; set; }
    public int trophyValue { get; set; }
    public bool isLocked { get; set; }

    public Tank()
    {

    }

	public Tank(string tankName, int indexNumber, int gearValue, int medalValue, int trophyValue, bool isLocked)
	{
		this.tankName = tankName; 
		this.indexNumber = indexNumber; 
		this.gearValue = gearValue; 
		this.medalValue = medalValue; 
		this.trophyValue = trophyValue; 
		this.isLocked = isLocked; 
	}
}

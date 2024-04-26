using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainViewController mainViewController;

    private void Start()
    {
    }

    public void AddTankInRadar(Transform tank, bool isEnemy = true)
    {
        mainViewController.AddTankInRadar(tank, isEnemy);
    }
}

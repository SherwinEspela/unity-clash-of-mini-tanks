using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100f;

    protected GameObject gameManager;

    void Start()
    {
        this.gameManager = GameObject.Find("GameManager");
    }
}

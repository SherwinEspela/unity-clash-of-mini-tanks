using UnityEngine;

public class CmtFxScript : MonoBehaviour
{
    [SerializeField] protected float offsetScrollSpeed = 0.5f;
    [SerializeField] protected float scaleTime = 2.2f;
    [SerializeField] protected bool debugMode = false;

    protected bool isEnabled = false;
    protected bool isDisplaying = false;

    public delegate void CmtFxEvent();

    private void Start()
    {
        if (debugMode)
        {
            Display();
        }
    }

    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        this.isDisplaying = true;
        this.isEnabled = true;
    }

    public void HideShield()
    {
        this.isDisplaying = false;
    }
}

using System.Collections;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);    
    }

    public virtual void Display()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        StartCoroutine(HideDelayed());
    }

    IEnumerator HideDelayed()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public interface IntroDelegate
{
    void IntroDidFinish();
}

public class IntroViewController : MonoBehaviour
{
    [SerializeField] Text textRound;
    [SerializeField] Animator animatorIntro;

    public IntroDelegate delegateIntro;

    private void Start()
    {
        textRound.text = GameConstants.GetRound(); 
        StartCoroutine(DisablePanelIntro());
    }

    IEnumerator DisablePanelIntro()
    {
        yield return new WaitForSeconds(6.0f);

        this.gameObject.SetActive(false);
        if (delegateIntro != null)
        {
            delegateIntro.IntroDidFinish();
        }
    }
}

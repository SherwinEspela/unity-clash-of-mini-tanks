using UnityEngine;
using UnityEngine.UI;

public interface PauseViewControllerDelegate
{
    void DidClickResumeButton();
    void DidClickRestartButton();
    void DidClickMainMenuButton();
}

public class PauseViewController : ViewController
{
    [Header("UI")]
    [SerializeField] Button buttonResume;
    [SerializeField] Button buttonRestart;
    [SerializeField] Button buttonMainMenu;

    [Header("Animation")]
    [SerializeField] Animation animationPauseVC;
    [SerializeField] AnimationClip clipSlideIn;
    [SerializeField] AnimationClip clipSlideOut;

    public PauseViewControllerDelegate delegatePauseVC;

    private void Start()
    {
        this.gameObject.SetActive(false);
        buttonResume.onClick.AddListener(OnButtonResumeClicked);
        buttonRestart.onClick.AddListener(OnButtonRestartClicked);
        buttonMainMenu.onClick.AddListener(OnButtonMainMenuClicked);
    }

    public override void Display()
    {
        base.Display();

        buttonResume.enabled = true;
        buttonRestart.enabled = true;
        buttonMainMenu.enabled = true;

        animationPauseVC.gameObject.SetActive(true);
        animationPauseVC.Play(clipSlideIn.name);
    }

    public override void Hide()
    {
        animationPauseVC.Play(clipSlideOut.name);

        buttonResume.enabled = false;
        buttonRestart.enabled = false;
        buttonMainMenu.enabled = false;

        base.Hide();
    }

    private void OnButtonResumeClicked()
    {
        this.Hide();

        if (delegatePauseVC != null)
        {
            delegatePauseVC.DidClickResumeButton();
        }
    }

    private void OnButtonRestartClicked()
    {
        this.Hide();

        if (delegatePauseVC != null)
        {
            delegatePauseVC.DidClickRestartButton();
        }
    }

    private void OnButtonMainMenuClicked()
    {
        this.Hide();

        if (delegatePauseVC != null)
        {
            delegatePauseVC.DidClickMainMenuButton();
        }
    }
}

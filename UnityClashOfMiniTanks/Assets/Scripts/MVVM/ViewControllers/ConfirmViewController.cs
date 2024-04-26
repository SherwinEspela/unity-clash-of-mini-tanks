using UnityEngine;
using UnityEngine.UI;

public interface ConfirmViewControllerDelegate
{
    void DidClickYesButton(ConfirmMessageType messageType);
    void DidClickNoButton();
}

public enum ConfirmMessageType
{
    Restart,
    MainMenu,
    Continue
}

public class ConfirmViewController : ViewController
{
    [SerializeField] Animation animationPanelConfirmation;
    [SerializeField] AnimationClip clipPanelConfirmationSlideIn;
    [SerializeField] AnimationClip clipPanelConfirmationSlideOut;

    [SerializeField] Text textMessage;

    [SerializeField] Button buttonYes;
    [SerializeField] Button buttonNo;

    public ConfirmViewControllerDelegate delegateConfirmVC;

    private ConfirmMessageType _messageType;

    private void Start()
    {
        this.gameObject.SetActive(false);

        buttonYes.onClick.AddListener(ButtonYesClicked);
        buttonNo.onClick.AddListener(ButtonNoClicked);
    }

    public void DisplayWithMessageType(ConfirmMessageType messageType)
    {
        base.Display();
        _messageType = messageType;

        switch (messageType)
        {
            case ConfirmMessageType.Restart:
                textMessage.text = "Are you sure you want to Restart the game?";
                break;
            case ConfirmMessageType.MainMenu:
                textMessage.text = "Are you sure you want to go to the Main Menu?";
                break;
            case ConfirmMessageType.Continue:
                textMessage.text = "Are you sure you want to continue?";
                break;
            default:
                break;
        }

        buttonYes.enabled = true;
        buttonNo.enabled = true;

        animationPanelConfirmation.Play(clipPanelConfirmationSlideIn.name);
    }

    public override void Hide()
    {
        animationPanelConfirmation.Play(clipPanelConfirmationSlideOut.name);

        buttonYes.enabled = false;
        buttonNo.enabled = false;

        base.Hide();
    }

    void ButtonYesClicked()
    {
        if (delegateConfirmVC != null)
        {
            delegateConfirmVC.DidClickYesButton(_messageType);
        }
    }

    void ButtonNoClicked()
    {
        if (delegateConfirmVC != null)
        {
            delegateConfirmVC.DidClickNoButton();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public interface ITrophyRewardDelegate
{
    void TrophyRewardDidClickOkButton();
}

public class TrophyRewardViewController : ViewController
{
    [Header("UI")]
    [SerializeField] Button buttonOk;

    [Header("Animation")]
    [SerializeField] AnimationClip clipPanelTrophyRewardSlideIn;
    [SerializeField] AnimationClip clipPanelTrophyRewardSlideOut;
    [SerializeField] Animation animationPanelTrophyReward;

    public ITrophyRewardDelegate delegateTrophyRewardVC;

    private void Start()
    {
        this.gameObject.SetActive(false);

        buttonOk.onClick.AddListener(OnButtonOkClicked);
    }

    public override void Display()
    {
        base.Display();

        buttonOk.enabled = true;
        TriggerShowPanelTrophyReward();
    }

    public override void Hide()
    {
        buttonOk.enabled = false;
    }

    private void TriggerShowPanelTrophyReward()
    {
        animationPanelTrophyReward.gameObject.SetActive(true);
        animationPanelTrophyReward.Play(clipPanelTrophyRewardSlideIn.name);
    }

    private void OnButtonOkClicked()
    {
        if (delegateTrophyRewardVC != null)
        {
            delegateTrophyRewardVC.TrophyRewardDidClickOkButton();
        }
    }
}

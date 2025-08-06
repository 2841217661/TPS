using UnityEngine;

public class QS_升至50级 : QuestStep
{
    private void OnEnable()
    {
        EventManager.Instance.playerEvent.onPlayerLevelUp += PlayerLevelUp;
    }

    private void OnDisable()
    {
        EventManager.Instance.playerEvent.onPlayerLevelUp -= PlayerLevelUp;
    }

    //玩家等级提升事件方法
    private void PlayerLevelUp()
    {
        if(GameManager.Instance.playerManager.currentLevel >= 50)
        {
            FinishQuestStep();

            QuestManager.Instance.TryFinishQuest(quest.id);

            QuestManager.Instance.StartDelayedQuest(QuestName.Quest_升级10, 2f);
        }
    }

    protected override void FinishThisStepReward()
    {
        base.FinishThisStepReward();

        UIManager.Instance.OpenPanel("BuffSelectPanel", UIManager.Instance.UIRoot);
    }
}

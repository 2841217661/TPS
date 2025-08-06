using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public QuestInfoSO quest;
    private bool isFinished = false;
    private string questId;

    public void InitalizeQuestStep(string questId)
    {
        this.questId = questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            FinishThisStepReward();
            EventManager.Instance.questEvent.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }
    }

    //该步骤完成后的奖励
    protected virtual void FinishThisStepReward()
    {
        Debug.Log("奖励："  + quest.rewardDescription);
    }
}

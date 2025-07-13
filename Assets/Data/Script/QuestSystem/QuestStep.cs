using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
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
            EventManager.Instance.questEvent.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }
    }
}

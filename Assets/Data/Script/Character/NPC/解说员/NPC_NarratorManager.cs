using UnityEngine;

public class NPC_NarratorManager : NPCManager
{

    public override void InteractableFinish()
    {
        base.InteractableFinish();


    }

    public override void InteractableStart()
    {
        base.InteractableStart();


        if (QuestManager.Instance.GetQuestById(QuestName.Quest_村庄许可证).state == QuestState.FINISHED)
        {
            questTip.SetActive(false);
        }
    }
}

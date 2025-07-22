using UnityEngine;

public class QS_询问规则 : QuestStep
{
    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Narrator += FinishConversation_Narrator;


    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Narrator -= FinishConversation_Narrator;

    }

    private void FinishConversation_Narrator(NPCManager _)
    {
        FinishQuestStep();
        QuestManager.Instance.TryFinishQuest(quest.id);

        QuestManager.Instance.TryStartQuest(QuestName.Quest_开启挑战);
    }
}

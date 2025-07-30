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

        //完成任务后，将当前任务的小地图索引移除
        Minmap.Instance.RemoveMinmapIcon(FindFirstObjectByType<NPC_NarratorManager>().transform, MinmapIconType.quest);

        //开启下一个任务
        QuestManager.Instance.TryStartQuest(QuestName.Quest_开启挑战);
    }
}

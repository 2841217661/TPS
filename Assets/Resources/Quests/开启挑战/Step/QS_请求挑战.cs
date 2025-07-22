using UnityEngine;

public class QS_请求挑战 : QuestStep
{
    private NPC_AdministratorManager manager;
    private void Awake()
    {
        manager = FindFirstObjectByType<NPC_AdministratorManager>();
        manager.questTip.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Administrator += FinishConversation_Administrator;
    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Administrator -= FinishConversation_Administrator;
    }

    private void FinishConversation_Administrator(NPCManager _manager)
    {
        if (manager.isReady)
        {
            FinishQuestStep();
            QuestManager.Instance.TryFinishQuest(quest.id);
        }
        else
        {
            Debug.Log("未完成任务");
        }
    }
}

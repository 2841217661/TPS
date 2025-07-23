using UnityEngine;

public class QS_请求挑战 : QuestStep
{
    private NPC_AdministratorManager manager;
    private void Awake()
    {
        //任务开启后，激活npc头上的小地图任务索引图标
        manager = FindFirstObjectByType<NPC_AdministratorManager>();
        manager.questTip.SetActive(true);

        //添加小地图索引
        Minmap.Instance.AddMinmapIcon(manager.transform,MinmapIconType.quest);
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

            //移除小地图索引
            Minmap.Instance.RemoveMinmapIcon(manager.transform, MinmapIconType.quest);
        }
        else
        {
            Debug.Log("未完成任务");
        }
    }
}

using UnityEngine;

public class QS_进入村庄 : QuestStep
{
    [SerializeField] private GameObject airWall;
    private GameObject airWallObj;
    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Adventure += FinishConversation_Adventure;

        airWallObj = Instantiate(airWall);
    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Adventure -= FinishConversation_Adventure;

        Destroy(airWallObj);
    }

    private void FinishConversation_Adventure(NPCManager _)
    {
        FinishQuestStep();
        QuestManager.Instance.TryFinishQuest(quest.id);

        //任务完成，开启下一个任务
        QuestManager.Instance.StartDelayedQuest(QuestName.Quest_赛前准备,1f);

        //移除当前任务的小地图索引
        Minmap.Instance.RemoveMinmapIcon(FindFirstObjectByType<NPC_AdventureManager>().transform, MinmapIconType.quest);

        //寻找下一个任务的npc，并实例小地图索引，激活npc头上的索引图标
        NPC_NarratorManager manager = FindFirstObjectByType<NPC_NarratorManager>();
        manager.questTip.SetActive(true);
        Minmap.Instance.AddMinmapIcon(manager.transform,MinmapIconType.quest);
    }
}

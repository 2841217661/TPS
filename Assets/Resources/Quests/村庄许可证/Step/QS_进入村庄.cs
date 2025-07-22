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

        QuestManager.Instance.StartDelayedQuest(QuestName.Quest_赛前准备,1f);

        NPC_NarratorManager manager = FindFirstObjectByType<NPC_NarratorManager>();
        manager.questTip.SetActive(true);
    }
}

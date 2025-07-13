using UnityEngine;

public class QS_进入村庄 : QuestStep
{
    [SerializeField] private QuestInfoSO quest;
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
        //Instantiate(初次沟通_开启器);
    }
}

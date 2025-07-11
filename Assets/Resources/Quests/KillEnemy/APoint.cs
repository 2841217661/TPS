using UnityEngine;

public class APoint : MonoBehaviour
{
    [SerializeField] private QuestInfoSO questInfoForPoint;
    private string questId;
    private QuestState currentQuestState;

    public bool startPoint;
    public bool finishPoint;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    //private void OnEnable()
    //{
    //    EventManager.Instance.questEvent.onQuestStateChange += QuestStateChange;
    //}

    //private void OnDisable()
    //{
    //    EventManager.Instance.questEvent.onQuestStateChange -= QuestStateChange;
    //}

    //private void QuestStateChange(Quest quest)
    //{
    //    if (quest.info.id.Equals(questId))
    //    {
    //        currentQuestState = quest.state;
    //        Debug.Log($"����Id: {questId}�� ����״̬Ϊ�� {currentQuestState}");
    //    }
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //EventManager.Instance.questEvent.StartQuest(questId);
            //EventManager.Instance.questEvent.AdvanceQuest(questId);
            //EventManager.Instance.questEvent.FinishQuest(questId);

            if(currentQuestState.Equals(QuestState.CAN_START))
            {
                EventManager.Instance.questEvent.StartQuest("KillEnemyQuest");
            }
            else
            {
                Debug.Log("�����ǰ������");
            }
            //else if(currentQuestState.Equals(QuestState.CAN_FINISH))
            //{
            //    EventManager.Instance.questEvent.FinishQuest(questId);
            //}
        }
    }
}

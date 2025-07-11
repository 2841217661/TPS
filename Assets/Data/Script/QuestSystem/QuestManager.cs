using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{
    private Dictionary<string, Quest> questMap;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        questMap = CreatQuestMap();
    }

    private void OnEnable()
    {
        EventManager.Instance.questEvent.onStartQuest += StartQuest;
        EventManager.Instance.questEvent.onAdvanceQuest += AdvanceQuest;
        EventManager.Instance.questEvent.onFinishQuest += FinishQuest;

        //ֻҪ�������״̬�����ı��ˢ��һ��(��Ϊ���������ǰ�������Լ��)�����Է���update...
        EventManager.Instance.questEvent.onQuestStateChange += RefreshQuestStateCAN_START; 
    }

    private void OnDisable()
    {
        EventManager.Instance.questEvent.onStartQuest -= StartQuest;
        EventManager.Instance.questEvent.onAdvanceQuest -= AdvanceQuest;
        EventManager.Instance.questEvent.onFinishQuest -= FinishQuest;
        EventManager.Instance.questEvent.onQuestStateChange -= RefreshQuestStateCAN_START;
    }

    private void Start()
    {
        RefreshQuestStateCAN_START(null); //��Ϸ��ʼʱ��Ҫˢ��һ��״̬
    }

    private void StartQuest(string id)
    {
        Debug.Log("����ʼ: " +  id);
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("�����ƽ���: " +  id);
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("�������: " +  id);
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    /// <summary>
    /// ˢ�������б���ÿ�������Ƿ���Ա�����
    /// </summary>
    private void RefreshQuestStateCAN_START(Quest _)
    {
        //Debug.Log("ˢ��һ�ζ�CAN_START�ļ��");
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        //Debug.Log($"��ǰ״̬ {id}:{quest.state}");
        quest.state = state;
        //Debug.Log($"�ı�״̬ {id}:{quest.state}");
        EventManager.Instance.questEvent.QuestStateChange(quest);
    }

    /// <summary>
    /// ��ȡQuests�ļ��µ���������
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, Quest> CreatQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach(QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("�ظ�id: " +  questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    /// <summary>
    /// ͨ��id���������ֵ��ж�Ӧ������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogWarning("û���ҵ�id��Ӧ������ : " + id);
        }
        return quest;
    }

    #region ��������
    /// <summary>
    /// ���Կ���ָ������
    /// </summary>
    /// <param name="questId">����id</param>
    /// <returns></returns>
    public bool TryStartQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        //��������Ƿ����
        if (quest == null)
        {
            Debug.Log($"����{questId}������,Resources/Quests�Ƿ��ж�Ӧ���������ݣ�");
            return false;
        }
        //��������״̬
        switch (quest.state)
        {
            //ǰ������δ���
            case QuestState.REQUIREMENTS_NOT_MET:
                Debug.LogWarning($"����{questId} ����δ��ɵ�ǰ�����񣬾�������:");
                Debug.Log(GetQuestPreQuestRequirement(quest));
                return false;
            case QuestState.CAN_START:
                Debug.Log("�ɹ���������:" +  questId);
                EventManager.Instance.questEvent.StartQuest(questId);
                return true;
            case QuestState.IN_PROGRESS:
                Debug.LogWarning($"����{questId} �Ѿ�����ִ��״̬���޷��ٴο���!");
                return false;
            case QuestState.CAN_FINISH:
                Debug.LogWarning($"����{questId} ���ڿ����״̬���޷��ٴο���!");
                return false;
            case QuestState.FINISHED:
                Debug.LogWarning($"���� {questId} �Ѿ���ɣ��޷����¿���!");
                return false;
            default:
                Debug.LogError("δ֪���ʹ���!!!");
                return false;
        }
    }

    /// <summary>
    /// �������ָ������
    /// </summary>
    /// <param name="questId">����id</param>
    /// <returns></returns>
    public bool TryFinishQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        //��������Ƿ����
        if (quest == null)
        {
            Debug.Log($"����{questId}������,�ǲ�������ID����ˣ�����");
            return false;
        }

        //��������״̬
        switch (quest.state)
        {
            //ǰ������δ���
            case QuestState.REQUIREMENTS_NOT_MET:
                Debug.LogWarning($"����{questId} �����޷��������㻹��ֱ��������");
                return false;
            case QuestState.CAN_START:
                Debug.LogWarning($"����{questId} ��û�п���������ֱ������������̣�");
                return false;
            case QuestState.IN_PROGRESS:
                Debug.LogWarning($"����{questId} ��û������ء�");
                return false;
            case QuestState.CAN_FINISH:
                Debug.Log($"����{questId} ������ɣ����������������...");
                EventManager.Instance.questEvent.FinishQuest(questId);
                return true;
            case QuestState.FINISHED:
                Debug.LogWarning($"���� {questId} �Ѿ�������ˣ�������е�̰�ˡ�");
                return false;
            default:
                Debug.LogError("δ֪����״̬���ʹ���!!! ��" + quest.state);
                return false;
        }
    }


    /// <summary>
    /// ��ȡ�ض�״̬�������б�
    /// </summary>
    /// <param name="questType">��������</param>
    /// <param name="states">��Ҫɸѡ������״̬</param>
    /// <returns></returns>
    public List<Quest> GetQuestsByState(QuestType questType, params QuestState[] states)
    {
        List<Quest> quests = new List<Quest>(10);
        foreach (Quest quest in questMap.Values)
        {
            if (System.Array.Exists(states, s => s == quest.state) && quest.info.type == questType)
            {
                quests.Add(quest);
            }
        }
        return quests;
    }




    /// <summary>
    /// ������������ȡָ������ɿ�����ǰ��������ɶ�
    /// </summary>
    private string GetQuestPreQuestRequirement(Quest quest)
    {
        string description = "";
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.startRequestPrefabs)
        {
            string finishDescription = "";
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                finishDescription = prerequisiteQuestInfo.id + ":" + "δ���";
            }
            else
            {
                finishDescription = prerequisiteQuestInfo.id + ":" + "�����";
            }
            description = description + finishDescription + "\n";
        }
        return description;
    }
    /// <summary>
    /// �������������ָ�������ǰ�������Ƿ��Ѿ������
    /// </summary>
    /// <param name="quest">����</param>
    /// <returns></returns>
    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.startRequestPrefabs)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }

        return meetsRequirements;
    }
    #endregion
}

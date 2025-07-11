using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreatQuestMap(); 
    }

    private void OnEnable()
    {
        EventManager.Instance.questEvent.onStartQuest += StartQuest;
        EventManager.Instance.questEvent.onAdvanceQuest += AdvanceQuest;
        EventManager.Instance.questEvent.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        EventManager.Instance.questEvent.onStartQuest -= StartQuest;
        EventManager.Instance.questEvent.onAdvanceQuest -= AdvanceQuest;
        EventManager.Instance.questEvent.onFinishQuest -= FinishQuest;
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
        ClaimReward(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    //private void Start()
    //{
    //    foreach (Quest quest in questMap.Values)
    //    {
    //        EventManager.Instance.questEvent.QuestStateChange(quest);
    //    }
    //}

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        EventManager.Instance.questEvent.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        foreach(QuestInfoSO prerequisiteQuestInfo in quest.info.startRequestPrefabs)
        {
            if(GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        //������������б����Ƿ�����������ɿ�������
        foreach(Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }



    private void ClaimReward(Quest quest)
    {
        Debug.Log($"�������: {quest.info.id};��������һö...");
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
        if(quest == null)
        {
            Debug.LogWarning("û���ҵ�id��Ӧ������ : " + id);
        }
        return quest;
    }
}

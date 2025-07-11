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

        //只要有任务的状态发生改变就刷新一次(因为任务可能有前置任务的约束)，可以放入update...
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
        RefreshQuestStateCAN_START(null); //游戏开始时需要刷新一次状态
    }

    private void StartQuest(string id)
    {
        Debug.Log("任务开始: " +  id);
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("任务推进中: " +  id);
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
        Debug.Log("任务完成: " +  id);
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    /// <summary>
    /// 刷新任务列表中每个任务是否可以被开启
    /// </summary>
    private void RefreshQuestStateCAN_START(Quest _)
    {
        //Debug.Log("刷新一次对CAN_START的检查");
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
        //Debug.Log($"当前状态 {id}:{quest.state}");
        quest.state = state;
        //Debug.Log($"改变状态 {id}:{quest.state}");
        EventManager.Instance.questEvent.QuestStateChange(quest);
    }

    /// <summary>
    /// 获取Quests文件下的所有任务
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
                Debug.LogWarning("重复id: " +  questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    /// <summary>
    /// 通过id查找任务字典中对应的任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogWarning("没有找到id对应的任务 : " + id);
        }
        return quest;
    }

    #region 公共方法
    /// <summary>
    /// 尝试开启指定任务
    /// </summary>
    /// <param name="questId">任务id</param>
    /// <returns></returns>
    public bool TryStartQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        //检查任务是否存在
        if (quest == null)
        {
            Debug.Log($"任务{questId}不存在,Resources/Quests是否有对应的任务数据？");
            return false;
        }
        //检查任务的状态
        switch (quest.state)
        {
            //前置任务未完成
            case QuestState.REQUIREMENTS_NOT_MET:
                Debug.LogWarning($"任务：{questId} 存在未完成的前置任务，具体如下:");
                Debug.Log(GetQuestPreQuestRequirement(quest));
                return false;
            case QuestState.CAN_START:
                Debug.Log("成功开启任务:" +  questId);
                EventManager.Instance.questEvent.StartQuest(questId);
                return true;
            case QuestState.IN_PROGRESS:
                Debug.LogWarning($"任务：{questId} 已经处于执行状态，无法再次开启!");
                return false;
            case QuestState.CAN_FINISH:
                Debug.LogWarning($"任务：{questId} 处于可完成状态，无法再次开启!");
                return false;
            case QuestState.FINISHED:
                Debug.LogWarning($"任务： {questId} 已经完成，无法重新开启!");
                return false;
            default:
                Debug.LogError("未知类型错误!!!");
                return false;
        }
    }

    /// <summary>
    /// 尝试完成指定任务
    /// </summary>
    /// <param name="questId">任务id</param>
    /// <returns></returns>
    public bool TryFinishQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        //检查任务是否存在
        if (quest == null)
        {
            Debug.Log($"任务{questId}不存在,是不是任务ID输错了？？？");
            return false;
        }

        //检查任务的状态
        switch (quest.state)
        {
            //前置任务未完成
            case QuestState.REQUIREMENTS_NOT_MET:
                Debug.LogWarning($"任务：{questId} 现在无法开启，你还想直接跳过？");
                return false;
            case QuestState.CAN_START:
                Debug.LogWarning($"任务：{questId} 还没有开启，你想直接跳过任务过程？");
                return false;
            case QuestState.IN_PROGRESS:
                Debug.LogWarning($"任务：{questId} 还没有完成呢。");
                return false;
            case QuestState.CAN_FINISH:
                Debug.Log($"任务：{questId} 可以完成！接下来将完成任务...");
                EventManager.Instance.questEvent.FinishQuest(questId);
                return true;
            case QuestState.FINISHED:
                Debug.LogWarning($"任务： {questId} 已经被完成了，你多少有点贪了。");
                return false;
            default:
                Debug.LogError("未知任务状态类型错误!!! ：" + quest.state);
                return false;
        }
    }


    /// <summary>
    /// 获取特定状态的任务列表
    /// </summary>
    /// <param name="questType">任务类型</param>
    /// <param name="states">需要筛选的任务状态</param>
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
    /// 辅助方法：获取指定任务可开启的前置任务完成度
    /// </summary>
    private string GetQuestPreQuestRequirement(Quest quest)
    {
        string description = "";
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.startRequestPrefabs)
        {
            string finishDescription = "";
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                finishDescription = prerequisiteQuestInfo.id + ":" + "未完成";
            }
            else
            {
                finishDescription = prerequisiteQuestInfo.id + ":" + "已完成";
            }
            description = description + finishDescription + "\n";
        }
        return description;
    }
    /// <summary>
    /// 辅助方法：检查指定任务的前置任务是否都已经完成了
    /// </summary>
    /// <param name="quest">任务</param>
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

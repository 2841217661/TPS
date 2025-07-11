using System;

public class QuestEvent
{
    public event Action<string> onStartQuest; //任务开始事件
    public void StartQuest(string id)
    {
        if(onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest; //任务推进事件
    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string> onFinishQuest; //任务完成事件
    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange; //任务状态改变事件
    public void QuestStateChange(Quest quest)
    {
        if(onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
}

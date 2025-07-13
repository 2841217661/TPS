using System;

public class QuestEvent
{
    public event Action<string> onStartQuest; //任务开始事件
    public void StartQuest(string id)
    {
        onStartQuest?.Invoke(id);
    }

    public event Action<string> onAdvanceQuest; //任务推进事件
    public void AdvanceQuest(string id)
    {
        onAdvanceQuest?.Invoke(id);
    }

    public event Action<string> onFinishQuest; //任务完成事件
    public void FinishQuest(string id)
    {
        onFinishQuest?.Invoke(id);
    }

    public event Action<Quest> onQuestStateChange; //任务状态改变事件
    public void QuestStateChange(Quest quest)
    {
        onQuestStateChange?.Invoke(quest);
    }
}

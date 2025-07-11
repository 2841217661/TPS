using System;

public class QuestEvent
{
    public event Action<string> onStartQuest; //����ʼ�¼�
    public void StartQuest(string id)
    {
        onStartQuest?.Invoke(id);
    }

    public event Action<string> onAdvanceQuest; //�����ƽ��¼�
    public void AdvanceQuest(string id)
    {
        onAdvanceQuest?.Invoke(id);
    }

    public event Action<string> onFinishQuest; //��������¼�
    public void FinishQuest(string id)
    {
        onFinishQuest?.Invoke(id);
    }

    public event Action<Quest> onQuestStateChange; //����״̬�ı��¼�
    public void QuestStateChange(Quest quest)
    {
        onQuestStateChange?.Invoke(quest);
    }
}

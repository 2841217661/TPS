using System;

public class QuestEvent
{
    public event Action<string> onStartQuest; //����ʼ�¼�
    public void StartQuest(string id)
    {
        if(onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest; //�����ƽ��¼�
    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string> onFinishQuest; //��������¼�
    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange; //����״̬�ı��¼�
    public void QuestStateChange(Quest quest)
    {
        if(onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
}

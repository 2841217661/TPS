using UnityEngine;

//��Ҫ���ýű�ִ��˳������ǰ��
public class EventManager : MonoSingleton<EventManager>
{
    public EnemyEvent enemyEvent;
    public QuestEvent questEvent;
    public NPCEvent npcEvent;
    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        enemyEvent = new EnemyEvent();
        questEvent = new QuestEvent();
        npcEvent = new NPCEvent();
    }
}

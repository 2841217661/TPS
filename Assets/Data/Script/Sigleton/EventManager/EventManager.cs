using UnityEngine;

//需要设置脚本执行顺序在最前面
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

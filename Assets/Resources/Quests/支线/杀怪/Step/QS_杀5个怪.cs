using UnityEngine;

public class QS_杀5个怪 : QuestStep
{
    private void OnEnable()
    {
        EventManager.Instance.enemyEvent.onDeath_Enemy += Death_Enemy;
    }

    private void OnDisable()
    {
        EventManager.Instance.enemyEvent.onDeath_Enemy -= Death_Enemy;
    }

    private void Death_Enemy(EnemyManager _enemy)
    {
        if(GameManager.Instance.playerKillEnemyCount >= 5)
        {
            FinishQuestStep();

            QuestManager.Instance.TryFinishQuest(quest.id);

            QuestManager.Instance.StartDelayedQuest(QuestName.Quest_杀怪2, 2f);
        }
    }

    protected override void FinishThisStepReward()
    {
        base.FinishThisStepReward();

        GameManager.Instance.GetConsumableItem("手雷", 10);
    }
}

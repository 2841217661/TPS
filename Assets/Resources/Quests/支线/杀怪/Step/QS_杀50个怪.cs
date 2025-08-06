using UnityEngine;

public class QS_杀50个怪 : QuestStep
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
        if (GameManager.Instance.playerKillEnemyCount >= 50)
        {
            FinishQuestStep();

            QuestManager.Instance.TryFinishQuest(quest.id);

            QuestManager.Instance.StartDelayedQuest(QuestName.Quest_杀怪4, 2f);
        }
    }

    protected override void FinishThisStepReward()
    {
        base.FinishThisStepReward();

        GameManager.Instance.GetConsumableItem("随机Buff", 1);
    }
}

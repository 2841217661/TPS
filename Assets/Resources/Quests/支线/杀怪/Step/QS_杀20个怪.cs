using UnityEngine;

public class QS_杀20个怪 : QuestStep
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
        if (GameManager.Instance.playerKillEnemyCount >= 20)
        {
            FinishQuestStep();

            QuestManager.Instance.TryFinishQuest(quest.id);

            QuestManager.Instance.StartDelayedQuest(QuestName.Quest_杀怪3, 2f);
        }
    }

    protected override void FinishThisStepReward()
    {
        base.FinishThisStepReward();

        GameManager.Instance.GetConsumableItem("生命药水", 5);
    }
}

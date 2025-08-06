using UnityEngine;

public class QS_杀100个怪 : QuestStep
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
        if (GameManager.Instance.playerKillEnemyCount >= 100)
        {
            FinishQuestStep();

            QuestManager.Instance.TryFinishQuest(quest.id);
        }
    }

    protected override void FinishThisStepReward()
    {
        base.FinishThisStepReward();

        GameManager.Instance.GetConsumableItem("手雷", 20);
    }
}

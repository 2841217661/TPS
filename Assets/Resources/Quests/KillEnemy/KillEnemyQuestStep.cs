using UnityEngine;

public class KillEnemyQuestStep : QuestStep
{
    private int killEnemyCount = 0;
    private int killEnemyCompleteCount = 3;

    private void OnEnable()
    {
        EventManager.Instance.enemyEvent.onDeath_Enemy += KillEnemy;
    }

    private void OnDisable()
    {
        if (EventManager.Instance == null) return;
        
        EventManager.Instance.enemyEvent.onDeath_Enemy -= KillEnemy;
    }

    private void KillEnemy(EnemyManager _enemyManager)
    {
        if(killEnemyCount <  killEnemyCompleteCount)
        {
            killEnemyCount++;
        }
        
        if(killEnemyCount >= killEnemyCompleteCount)
        {
            FinishQuestStep();
        }
    }
}

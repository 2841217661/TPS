using UnityEngine;

public class EnemyEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.enemyEvent.onDeath_Enemy += Death_Enemy; 
        EventManager.Instance.enemyEvent.onDeath_Zombie += Death_Zombie; 
    }

    private void OnDisable()
    {

        if (EventManager.Instance == null) return;


        EventManager.Instance.enemyEvent.onDeath_Enemy -= Death_Enemy;
        EventManager.Instance.enemyEvent.onDeath_Zombie -= Death_Zombie;
    }


    private void Death_Enemy(EnemyManager _enemyManager)
    {
        Debug.Log("敌人死亡");
        if(_enemyManager is TestEnemyManager)
        {
            EventManager.Instance.enemyEvent.Death_Zombie(_enemyManager);
        }
        //...更多敌人的单独死亡事件
    }

    private void Death_Zombie(EnemyManager _enemyManager)
    {
        Debug.Log("僵尸死亡");
    }
}

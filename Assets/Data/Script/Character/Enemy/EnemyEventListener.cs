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
        Debug.Log("��������");
        if(_enemyManager is TestEnemyManager)
        {
            EventManager.Instance.enemyEvent.Death_Zombie(_enemyManager);
        }
        //...������˵ĵ��������¼�
    }

    private void Death_Zombie(EnemyManager _enemyManager)
    {
        Debug.Log("��ʬ����");
    }
}

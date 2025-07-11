using System;
using UnityEngine;

public class EnemyEvent
{
    public Action<EnemyManager> onDeath_Enemy;
    public void Death_Enemy(EnemyManager _enemyManager)
    {
        if(onDeath_Enemy != null)
        {
            onDeath_Enemy(_enemyManager);
        }
    }

    public Action<EnemyManager> onDeath_Zombie;
    public void Death_Zombie(EnemyManager _enemyManager)
    {
        if(onDeath_Enemy != null)
        {
            onDeath_Zombie(_enemyManager);
        }
    }
}

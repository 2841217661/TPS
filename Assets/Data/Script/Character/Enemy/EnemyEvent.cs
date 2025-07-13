using System;
using UnityEngine;

public class EnemyEvent
{
    public Action<EnemyManager> onDeath_Enemy;
    public void Death_Enemy(EnemyManager _enemyManager)
    {
        onDeath_Enemy?.Invoke(_enemyManager);
    }

    public Action<EnemyManager> onDeath_Zombie;
    public void Death_Zombie(EnemyManager _enemyManager)
    {
        onDeath_Zombie?.Invoke(_enemyManager);
    }
}

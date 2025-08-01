using UnityEngine;

public class B_淘汰回放 : BuffBase
{
    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        EventManager.Instance.enemyEvent.onDeath_Enemy += Death_Enemy;
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        EventManager.Instance.enemyEvent.onDeath_Enemy -= Death_Enemy;
    }


    private void Death_Enemy(EnemyManager _manager)
    {
        if(Random.Range(0f, 1f) < 0.2f)
        {
            PoolManager.Instance.Spawn(PoolManager.Instance.淘汰回放爆炸球.name, _manager.transform.position + Vector3.up, Quaternion.identity);
        }
    }
}

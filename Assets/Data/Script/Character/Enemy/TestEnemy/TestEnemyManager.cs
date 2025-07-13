using UnityEngine;

public class TestEnemyManager : EnemyManager
{
    protected override void Awake()
    {
        base.Awake();

    }


    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        //按一次K触发一次死亡事件
        if(Input.GetKeyDown(KeyCode.K))
        {
            EventManager.Instance.enemyEvent.Death_Enemy(this);
        }
    }
}

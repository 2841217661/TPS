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

        //��һ��K����һ�������¼�
        if(Input.GetKeyDown(KeyCode.K))
        {
            EventManager.Instance.enemyEvent.Death_Enemy(this);
        }
    }
}

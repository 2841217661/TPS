using UnityEngine;

public class B_追踪子弹 : BuffBase
{
    private float probability = 0.1f; //每级提升的概率
    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        EventManager.Instance.bulletEvent.onBulletSpawn += NormalBulletSpawn;//添加子弹生成事件方法回调
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        EventManager.Instance.bulletEvent.onBulletSpawn -= NormalBulletSpawn;
    }
    private void NormalBulletSpawn(BulletType _type)
    {
        if (_type != BulletType.normal) return;

        float random = Random.Range(0f, 1f);

        if (random > probability *  CurrentLevel) return;

        Transform shooter = GameManager.Instance.playerManager.shooter;
        //向左向右15°分别生成一颗追踪子弹
        Vector3 dir1 = Quaternion.AngleAxis(15f, Vector3.up) * shooter.forward;
        Vector3 dir2 = Quaternion.AngleAxis(-15f, Vector3.up) * shooter.forward;
        GameObject obj1 = PoolManager.Instance.Spawn(PoolManager.Instance.bulletTrack_Ordinary.name, shooter.position, shooter.rotation);
        obj1.transform.rotation = Quaternion.LookRotation(dir1);
        GameObject obj2 = PoolManager.Instance.Spawn(PoolManager.Instance.bulletTrack_Ordinary.name, shooter.position, shooter.rotation);
        obj2.transform.rotation = Quaternion.LookRotation(dir2);
    }
}

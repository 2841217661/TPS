using UnityEngine;
public enum NormalBulletType
{
    ordinary, //最普通的子弹
    flame, //火焰子弹
}
public class NormalBulletManager : BulletManager
{
    public NormalBulletType normalBulletType;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

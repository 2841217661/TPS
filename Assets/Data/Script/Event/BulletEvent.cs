using System;
using UnityEngine;

public class BulletEvent
{
    public Action<BulletType> onBulletSpawn; //子弹生成时的事件

    public void BulletSpawn(BulletType _type)
    {
        onBulletSpawn?.Invoke(_type);
    }

    public Action<BulletType> onBulletHitObject; //子弹击中目标的事件

    public void BulletHitObject(BulletType _type)
    {
        onBulletHitObject?.Invoke(_type);
    }

}

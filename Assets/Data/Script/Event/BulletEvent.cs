using System;
using UnityEngine;

public class BulletEvent
{
    public Action<BulletType> onBulletSpawn; //子弹生成时的事件

    public void BulletSpawn(BulletType _type)
    {
        onBulletSpawn?.Invoke(_type);
    }

    public Action<BulletType,CharacterManager> onBulletHitObject; //子弹击中目标的事件

    public void BulletHitObject(BulletType _type, CharacterManager _manaer)
    {
        onBulletHitObject?.Invoke(_type, _manaer);
    }

}

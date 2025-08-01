using UnityEngine;

public class B_火焰弹头 : BuffBase
{
    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        GameManager.Instance.playerManager.currentUseBulletType = PlayerManager.CurrentUseBulletType.BulletNormal_Flame;
        EventManager.Instance.bulletEvent.onBulletHitObject += BulletHitObject;
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        GameManager.Instance.playerManager.currentUseBulletType = PlayerManager.CurrentUseBulletType.BulletNormal_Ordinary;
        EventManager.Instance.bulletEvent.onBulletHitObject -= BulletHitObject;
    }

    private void BulletHitObject(BulletType _, CharacterManager _manaer)
    {
        if(_manaer != null) //有可能命中的是障碍物
        {
            _manaer.buffSystem.AddBuff<B_火焰印记>();
        }
    }
}

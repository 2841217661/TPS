using UnityEngine;

public class B_战术扩容 : BuffBase
{
    private PlayerManager m_playerManager;

    public int effectCount = 10; //每层提升的弹夹容量数量

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        m_playerManager = characterManager as PlayerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        m_playerManager.maxBulletCount += 10 *  change;

        if(change > 0) //增加层数
        {
            //刷新弹夹容量
            m_playerManager.currentBulletCount = m_playerManager.maxBulletCount;
        }
        else
        {
            //防止弹夹超过最大容量
            m_playerManager.currentBulletCount = Mathf.Clamp(m_playerManager.currentBulletCount, 0, m_playerManager.maxBulletCount);
        }
    }
}

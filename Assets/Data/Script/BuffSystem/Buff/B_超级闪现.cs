using UnityEngine;

public class B_超级闪现 : BuffBase
{
    private float m_effectSize = 0.1f; //每级减少10%的闪避冷却时间
    private PlayerManager m_playerManager;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        m_playerManager = characterManager as PlayerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        m_playerManager.dodgeCooldown -= m_playerManager.baseDodgeCooldown * m_effectSize * change;
    }
}

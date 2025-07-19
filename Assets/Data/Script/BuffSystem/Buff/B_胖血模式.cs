using UnityEngine;

public class B_胖血模式 : BuffBase
{
    private PlayerManager m_playerManager;

    public float effectSize = 0.1f;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        m_playerManager = characterManager as PlayerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        m_playerManager.maxHealthValue += m_playerManager.baseHealthValue * effectSize *  change;
    }
}

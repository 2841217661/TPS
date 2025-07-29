using UnityEngine;

public class B_狂暴火力 : BuffBase
{
    private PlayerManager m_playerManager;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        m_playerManager = characterManager as PlayerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        m_playerManager.currentShootSpeed -= m_playerManager.baseShootSpeed * 0.5f *  change;
    }
}

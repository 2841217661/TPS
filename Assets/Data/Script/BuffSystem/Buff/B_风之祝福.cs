using UnityEngine;

//移速Buff
public class B_风之祝福 : BuffBase
{
    private PlayerManager m_playerManager;

    public float effectSize = 0.1f; //效果强度

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        m_playerManager = characterManager as PlayerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        //float currentMoveAnimSpeedMul = m_playerManager.animator.GetFloat("MoveAnimSpeedMul");
        //m_playerManager.animator.SetFloat("MoveAnimSpeedMul", currentMoveAnimSpeedMul + effectSize * change);

        m_playerManager.animationMovementMul += effectSize * change;
    }
}

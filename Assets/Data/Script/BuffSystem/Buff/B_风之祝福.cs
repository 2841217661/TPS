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


    public override string GetDescription1()
    {
        return BuffData.buffDescript1;
    }

    public override string GetDescription2()
    {
        return BuffData.buffDescript2;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        float currentMoveAnimSpeedMul = m_playerManager.animator.GetFloat("MoveAnimSpeedMul");
        //Debug.Log("当前移动倍率：" + currentMoveAnimSpeedMul);
        //Debug.Log("增加的倍率: " + effectSize * change);
        m_playerManager.animator.SetFloat("MoveAnimSpeedMul", currentMoveAnimSpeedMul + effectSize * change);
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();
    }
}

using UnityEngine;

public class B_枪枪重击 : BuffBase
{
    private float effectSize = 0.1f;
    private PlayerManager playerManager;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        playerManager = GameManager.Instance.playerManager;
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        playerManager.currentCrticalMul += effectSize * change;
    }
}

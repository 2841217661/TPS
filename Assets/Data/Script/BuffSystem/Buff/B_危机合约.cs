using UnityEngine;

public class B_危机合约 : BuffBase
{
    private float effectSize = 0.5f;
    PlayerManager playerManager;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        playerManager = GameManager.Instance.playerManager;

        playerManager.currentExperienceIncreaseMul += playerManager.baseExperienceIncreaseMul * effectSize;
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        playerManager.currentExperienceIncreaseMul -= playerManager.baseExperienceIncreaseMul * effectSize;
    }
}

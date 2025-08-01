using UnityEngine;

public class CI_超级加速 : ConsumableItem
{
    protected override void MakeItem()
    {

    }

    private void Awake()
    {
        MakeItem();
        //GameManager.Instance.playerManager.buffSystem.AddBuff
    }
}

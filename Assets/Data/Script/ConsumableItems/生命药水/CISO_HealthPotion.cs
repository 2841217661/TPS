using UnityEngine;

[CreateAssetMenu(fileName = "生命药水", menuName = "Scriptable/ConItem/生命药水")]

public class CISO_HealthPotion : ConsumableItemSO
{


    public override GameObject MakeItem()
    {
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_生命药水>();
        return null;
    }
}

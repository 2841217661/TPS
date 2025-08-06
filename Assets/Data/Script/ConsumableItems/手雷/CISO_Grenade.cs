using UnityEngine;

[CreateAssetMenu(fileName = "ÊÖÀ×", menuName = "Scriptable/ConItem/ÊÖÀ×")]
public class CISO_Grenade : ConsumableItemSO
{
    public override GameObject MakeItem()
    {

        return Instantiate(Resources.Load<GameObject>("Prefabs/ConsumableItems/Item/ÊÖÀ×"));
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Ëæ»úBuff", menuName = "Scriptable/ConItem/Ëæ»úBuff")]
public class CISO_Randombuff : ConsumableItemSO
{
    public override GameObject MakeItem()
    {
        UIManager.Instance.OpenPanel("BuffSelectPanel",UIManager.Instance.UIRoot);
        return null;
    }
}

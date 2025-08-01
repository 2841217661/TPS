using UnityEngine;

public enum ConsumableItemType
{
    Prop, //一次性道具：治疗
    Buff, //Buff道具：短时间增伤
    Throw, //伤害类道具：手雷、燃烧瓶
}

public abstract class ConsumableItem:MonoBehaviour
{
    public string itemId;
    public ConsumableItemType itemType;
    public Sprite icon;
    public int count;

    protected abstract void MakeItem();
}

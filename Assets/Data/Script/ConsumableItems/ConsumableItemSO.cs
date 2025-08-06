using System;
using UnityEngine;

public enum ConsumableItemType
{
    Prop, //一次性道具：治疗
    Buff, //Buff道具：短时间增伤
    Throw, //伤害类道具：手雷、燃烧瓶
}

public abstract class ConsumableItemSO : ScriptableObject
{
    public string itemId;

    public int count;
    public ConsumableItemType itemType;
    public Sprite icon;

    public abstract GameObject MakeItem();

    private void OnValidate()
    {
#if UNITY_EDITOR
        itemId = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}



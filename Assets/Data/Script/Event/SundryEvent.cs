using System;
using UnityEngine;

public class SundryEvent
{
    #region 消耗物品道具
    public Action<string> onConItemUsed; //消耗物品事件

    public void ConItemUsed(string _itemId)
    {
        onConItemUsed(_itemId);
    }

    public Action<string,int> onConItemGet; //获得物品事件

    public void ConItemGet(string _itemId,  int _count)
    {
        onConItemGet(_itemId, _count);
    }
    #endregion
}

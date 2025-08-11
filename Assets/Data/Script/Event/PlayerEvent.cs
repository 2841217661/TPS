using System;
using UnityEngine;

public class PlayerEvent
{
    //玩家升级事件
    public event Action onPlayerLevelUp;
    public void PlayerLevelUp()
    {
        onPlayerLevelUp?.Invoke();
    }

    //玩家死亡事件
    public event Action onPlayerDeath;
    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }
}

using System;
using UnityEngine;

public class NPCEvent
{
    public event Action<NPCManager> onFinishConversation_Anyone; //任意npc结束对话事件
    public void FinishConversation_Anyone(NPCManager _npcManager)
    {
        onFinishConversation_Anyone?.Invoke(_npcManager);
    }

    public event Action<NPCManager> onFinishConversation_Adventure; //与冒险者结束对话事件
    public void FinishConversation_Adventure(NPCManager _npcManager)
    {
        onFinishConversation_Adventure?.Invoke(_npcManager);
    }
}

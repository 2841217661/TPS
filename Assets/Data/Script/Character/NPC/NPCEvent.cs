using System;
using UnityEngine;

public class NPCEvent
{
    public event Action<NPCManager> onFinishConversation; //结束对话事件
    public void FinishConversation(NPCManager _npcManager)
    {
        onFinishConversation?.Invoke(_npcManager);
    }
}

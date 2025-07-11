using System;
using UnityEngine;

public class NPCEvent
{
    public event Action<NPCManager> onFinishConversation; //�����Ի��¼�
    public void FinishConversation(NPCManager _npcManager)
    {
        onFinishConversation?.Invoke(_npcManager);
    }
}

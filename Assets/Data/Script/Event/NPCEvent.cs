using System;
using UnityEngine;

public class NPCEvent
{
    //任意npc结束对话事件
    public event Action<NPCManager> onFinishConversation_Anyone; 
    public void FinishConversation_Anyone(NPCManager _npcManager)
    {
        onFinishConversation_Anyone?.Invoke(_npcManager);
    }

    //任意npc开始对话事件
    public event Action<NPCManager> onStartConversation_Anyone;
    public void StartConversation_Anyone(NPCManager _npcManager)
    {
        onStartConversation_Anyone?.Invoke(_npcManager);
    }

    
    //与冒险者结束对话事件
    public event Action<NPCManager> onFinishConversation_Adventure;
    public void FinishConversation_Adventure(NPCManager _npcManager)
    {
        onFinishConversation_Adventure?.Invoke(_npcManager);
    }

    //与冒险者开始对话事件
    public event Action<NPCManager> onStartConversation_Adventure;
    public void StartConversation_Adventure(NPCManager _npcManager)
    {
        onStartConversation_Adventure?.Invoke(_npcManager);
    }

    //与解说员结束对话事件
    public event Action<NPCManager> onFinishConversation_Narrator; 
    public void FinishConversation_Narrator(NPCManager _npcManager)
    {
        onFinishConversation_Narrator?.Invoke(_npcManager);
    }

    //与解说员开始对话事件
    public event Action<NPCManager> onStartConversation_Narrator;
    public void StartConversation_Narrator(NPCManager _npcManager)
    {
        onStartConversation_Narrator?.Invoke(_npcManager);
    }


    //与管理员结束对话事件
    public event Action<NPCManager> onFinishConversation_Administrator;
    public void FinishConversation_Administrator(NPCManager _npcManager)
    {
        onFinishConversation_Administrator?.Invoke(_npcManager);
    }

    //与管理员开始对话事件
    public event Action<NPCManager> onStartConversation_Administrator;
    public void StartConversation_Administrator(NPCManager _npcManager)
    {
        onStartConversation_Administrator?.Invoke(_npcManager);
    }
}

using UnityEngine;

public class NPCEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Anyone += FinishConversation_Anyone;
        EventManager.Instance.npcEvent.onStartConversation_Anyone += StartConversation_Anyone;
    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Anyone -= FinishConversation_Anyone;
        EventManager.Instance.npcEvent.onStartConversation_Anyone -= StartConversation_Anyone;
    }

    private void StartConversation_Anyone(NPCManager _npcManager)
    {
        Debug.Log($"与 《{_npcManager.characterName}》 对话开始");

        if (_npcManager is NPC_AdventureManager)
        {
            EventManager.Instance.npcEvent.StartConversation_Adventure(_npcManager);
        }
        else if (_npcManager is NPC_NarratorManager)
        {
            EventManager.Instance.npcEvent.StartConversation_Narrator(_npcManager);
        }
        else if (_npcManager is NPC_AdministratorManager)
        {
            EventManager.Instance.npcEvent.StartConversation_Administrator(_npcManager);
        }
    }


    private void FinishConversation_Anyone(NPCManager _npcManager)
    {
        Debug.Log($"与 《{_npcManager.characterName}》 对话结束");

        if(_npcManager is NPC_AdventureManager)
        {
            EventManager.Instance.npcEvent.FinishConversation_Adventure(_npcManager);
        }
        else if(_npcManager is NPC_NarratorManager)
        {
            EventManager.Instance.npcEvent.FinishConversation_Narrator(_npcManager);
        }
        else if (_npcManager is NPC_AdministratorManager)
        {
            EventManager.Instance.npcEvent.FinishConversation_Administrator(_npcManager);
        }
    }
}

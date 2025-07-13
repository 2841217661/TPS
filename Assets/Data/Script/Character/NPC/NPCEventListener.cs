using UnityEngine;

public class NPCEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Anyone += FinishConversation_Anyone;
    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation_Anyone -= FinishConversation_Anyone;
    }


    private void FinishConversation_Anyone(NPCManager _npcManager)
    {
        Debug.Log($"与 《{_npcManager.characterName}》 对话结束");

        if(_npcManager is NPC_AdventureManager)
        {
            EventManager.Instance.npcEvent.FinishConversation_Adventure(_npcManager);
        }
    }
}

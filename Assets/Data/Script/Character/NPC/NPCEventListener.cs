using UnityEngine;

public class NPCEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.npcEvent.onFinishConversation += FinishConversation;
    }

    private void OnDisable()
    {
        EventManager.Instance.npcEvent.onFinishConversation -= FinishConversation;
    }


    private void FinishConversation(NPCManager _npcManager)
    {
        Debug.Log($"�� ��{_npcManager.characterName}�� �Ի�����");
    }
}

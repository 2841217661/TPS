using TMPro;
using UnityEngine;

public class NPCManager : CharacterManager
{
    public DialogueGraph currentGraph;

    protected GameObject buttonGroup;

    protected NPCState m_state = NPCState.Idle;

    protected float m_interactableDis = 3f; //�ɽ�����������
    protected enum NPCState
    {
        Idle, //����״̬
        Interactable, //��̸״̬
    }

    protected override void Update()
    {
        base.Update();

        if (m_state != NPCState.Idle) return; //�Ǵ���״̬�²��ܽ��н�̸

        if (PlayerDetected())
        {
            if (buttonGroup == null)
            {
                //��ʾ�ɽ���Button
                buttonGroup = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/DialoguePanel/SelectButton/Button_SelectGroup"), GameObject.Find("Canvas").transform);
                GameObject buttonItem = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/DialoguePanel/SelectButton/Button_SelectItem"), buttonGroup.transform);
                buttonItem.GetComponentInChildren<TextMeshProUGUI>().text = "F ���н�̸";
            }

            if (GameManager.Instance.playerManager.inputManager.inputActions.UI.EnterDialogue.WasPressedThisFrame())
            {
                DialoguePanel dialoguePanel = (UIManager.Instance.OpenPanel("DialoguePanel")) as DialoguePanel;
                dialoguePanel.npcManager = this;
                dialoguePanel.graph = currentGraph;
                m_state = NPCState.Interactable;

                Destroy(buttonGroup);
            }
        }
        else
        {
            if (buttonGroup != null)
                Destroy(buttonGroup);
        }
    }

    //�ж�����Ƿ��ڿɽ����ķ�Χ��
    private bool PlayerDetected()
    {
        return Vector3.Distance(GameManager.Instance.playerManager.transform.position, transform.position) < m_interactableDis;
    }


    public virtual void InteractableFinish()
    {
        m_state = NPCState.Idle;
        EventManager.Instance.npcEvent.FinishConversation(this);
    }
}

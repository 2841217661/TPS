using TMPro;
using UnityEngine;

public class NPCManager : CharacterManager
{
    public DialogueGraph currentGraph;

    protected GameObject buttonGroup;

    protected NPCState m_state = NPCState.Idle;

    protected float m_interactableDis = 3f; //可交互的最大距离
    protected enum NPCState
    {
        Idle, //闲置状态
        Interactable, //交谈状态
    }

    protected override void Update()
    {
        base.Update();

        if (m_state != NPCState.Idle) return; //非待机状态下不能进行交谈

        if (PlayerDetected())
        {
            if (buttonGroup == null)
            {
                //显示可交互Button
                buttonGroup = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/DialoguePanel/SelectButton/Button_SelectGroup"), GameObject.Find("Canvas").transform);
                GameObject buttonItem = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/DialoguePanel/SelectButton/Button_SelectItem"), buttonGroup.transform);
                buttonItem.GetComponentInChildren<TextMeshProUGUI>().text = "F 进行交谈";
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

    //判断玩家是否在可交换的范围内
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

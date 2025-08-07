using System;
using TMPro;
using UnityEngine;

public class NPCManager : CharacterManager
{
    public GameObject questTip;
    public DialogueGraph currentGraph;

    protected GameObject buttonGroup;

    public NPCState state = NPCState.Idle;
    
    protected float m_interactableDis = 3f; //可交互的最大距离
    public enum NPCState
    {
        Idle, //闲置状态
        Interactable, //交谈状态
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if(questTip != null)
        {
            QuestTipToFaceCamera();
        }

        if (state != NPCState.Idle) return; //非待机状态下不能进行交谈

        if (PlayerDetected())
        {
            if (buttonGroup == null)
            {
                //显示可交互Button
                buttonGroup = Instantiate(Resources.Load<GameObject>(UIPanelPath.Button_SelectGroup), NormalPanel.Instance.DynamicUI_Canvas);
                GameObject buttonItem = Instantiate(Resources.Load<GameObject>(UIPanelPath.Button_SelectItem), buttonGroup.transform);
                buttonItem.GetComponentInChildren<TextMeshProUGUI>().text = "F 进行交谈";
            }

            if (GameManager.Instance.playerManager.inputManager.inputActions.UI.EnterDialogue.WasPressedThisFrame())
            {
                var dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel",UIManager.Instance.UIRoot);
                (dialoguePanel as DialoguePanel).npcManager = this;
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

    //剧情对话结束
    public virtual void InteractableFinish()
    {
        state = NPCState.Idle;
        EventManager.Instance.npcEvent.FinishConversation_Anyone(this);
    }

    //剧情对话开始
    public virtual void InteractableStart()
    {
        state = NPCState.Interactable;
        EventManager.Instance.npcEvent.StartConversation_Anyone(this);
    }

    public void ShowQuestTip()
    {
        questTip.SetActive(true);
    }

    public void HideQuestTip()
    {
        questTip.SetActive(false);
    }

    private void QuestTipToFaceCamera()
    {
        if (questTip.activeSelf)
        {
            questTip.transform.rotation = Quaternion.LookRotation(questTip.transform.position - Camera.main.transform.position);
        }
    }


}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XNode;
using UnityEngine.EventSystems;

public class DialoguePanel : BasePanel
{
    [HideInInspector] public DialogueGraph graph; // �Ի�ͼ
    private Node currentNode;

    [Header("�Ի�����")]
    public NPCManager npcManager;

    [Header("�Ի�����UI����")]
    public TextMeshProUGUI Text_SpeakerName;
    public TextMeshProUGUI Text_SpeakerTitle;
    public TextMeshProUGUI Text_SpeakContent;
    public Button Button_Next;

    [Header("ѡ��Button")]
    public GameObject Pre_Button_SelectGroup;
    public GameObject Pre_Button_SelectItem;

    private void Awake()
    {
        Button_Next.onClick.AddListener(Button_Onclick_Next);
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        //���"����"��ť����˸����
        AddButtonNextAnimation();
    }

    private void OnDisable()
    {
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true,true);
        DOTween.KillAll();
    }

    private void Start()
    {
        //����Ի�ʱ�������������
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        if (npcManager == null)
        {
            Debug.LogError("�Ի���δ���룿����");
        }
        if (!CheckGraphIsInstance())
        {
            Debug.LogError("�Ի�ͼΪ�գ������Ƿ��Ѿ����룡");
        }

        currentNode = graph.nodes[0];
        ShowCurrentNodeContent();
    }

    private void Update()
    {
        if (GameManager.Instance.playerManager.inputManager.inputActions.UI.Dialogue.WasPressedThisFrame())
        {
            if(currentNode is DialogueNode)
                Advance();
        }
    }

    private void Button_Onclick_Next()
    {
        Advance();
    }

    private void Advance(int _choiceIndex = 0)
    {
        if (currentNode is DialogueNode)
        {
            //���õ�ǰ�Ի��ڵ�ķ���
            RunCurrentDialogueMethod((currentNode as DialogueNode).methodName);

            // DialogueNode����output
            NodePort outputPort = (currentNode as DialogueNode).GetOutputPort("output");
            if (outputPort != null && outputPort.Connection != null)
            {
                currentNode = outputPort.Connection.node;
                ShowCurrentNodeContent();
            }
            else //�Ի��������ر����
            {
                npcManager.InteractableFinish();
                ClosePanel();
            }
        }
        else if (currentNode is ChoiceNode)
        {
            NodePort choiceNode = (currentNode as ChoiceNode).GetOutputPort($"outputs {_choiceIndex}");
            currentNode = choiceNode.Connection.node;
            ShowCurrentNodeContent();
        }
    }

    private void ShowCurrentNodeContent()
    {
        if(currentNode is DialogueNode)
        {
            DialogueNode dn = currentNode as DialogueNode;
            Debug.Log($"{dn.speaker}: {dn.content}");
            if(dn.speaker == Speaker.Player)
            {
                Text_SpeakerName.text = GameManager.Instance.playerManager.characterName;
                Text_SpeakerTitle.text = GameManager.Instance.playerManager.characterTitle;
            }
            else
            {
                Text_SpeakerName.text = npcManager.characterName;
                Text_SpeakerTitle.text = npcManager.characterTitle;
            }
                
            Text_SpeakContent.text = dn.content;
        }
        else if(currentNode is ChoiceNode)
        {
            ChoiceNode choiceNode = currentNode as ChoiceNode;

            Button_Next.GetComponent<CanvasGroup>().interactable = false; //����ѡ�����nextbutton����
            Transform canvas = GameObject.Find("Canvas").transform;
            //ʵ��ѡ��ѡ��Button
            GameObject buttonGroup = Instantiate(Pre_Button_SelectGroup, canvas);
            for (int i = 0; i < choiceNode.outputs.Count; i++)
            {
                int index = i; // ����ǳ���Ҫ�������ֲ���������

                GameObject buttonItem = Instantiate(Pre_Button_SelectItem, buttonGroup.transform);
                buttonItem.GetComponentInChildren<TextMeshProUGUI>().text = choiceNode.outputs[index];
                buttonItem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    RunCurrentDialogueMethod(choiceNode.methodName[index]);
                    CanvasGroup canvasGroup = Button_Next.GetComponent<CanvasGroup>();
                    canvasGroup.interactable = true;
                    Advance(index);
                    Destroy(buttonGroup);
                });
            }
            EventSystem.current.SetSelectedGameObject(buttonGroup.transform.GetChild(0).gameObject);

        }
    }

    private void RunCurrentDialogueMethod(string _methodName)
    {
        if (_methodName == "") return;

        var method = npcManager.GetType().GetMethod(
            _methodName,
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
        );

        if (method != null)
        {
            if (method.GetParameters().Length == 0)
            {
                method.Invoke(npcManager, null);
            }
            else
            {
                Debug.LogWarning($"���� {_methodName} ���ڲ������޷����ã���֧���޲Σ���");
            }
        }
        else
        {
            Debug.LogWarning($"���� {_methodName} �� {npcManager.GetType().Name} ��δ�ҵ������� public����");
        }
    }



    private bool CheckGraphIsInstance()
    {
        return graph != null;
    }

    private void AddButtonNextAnimation()
    {
        // ��ȡ��ť��CanvasGroup��������ڵ��뵭����
        CanvasGroup cg = Button_Next.GetComponent<CanvasGroup>();

        // ÿ�ε���/����ʱ��
        float fadeDuration = 0.5f;
        // ͣ��ʱ��
        float interval = 0.3f;

        // ����Sequence
        Sequence seq = DOTween.Sequence();
        seq.Append(cg.DOFade(0, fadeDuration))
           .AppendInterval(interval)
           .Append(cg.DOFade(1, fadeDuration))
           .AppendInterval(interval)
           .SetLoops(-1);
    }
}

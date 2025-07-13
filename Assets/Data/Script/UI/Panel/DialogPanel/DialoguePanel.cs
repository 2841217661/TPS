using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XNode;
using UnityEngine.EventSystems;

public class DialoguePanel : BasePanel
{
    [HideInInspector] public DialogueGraph graph; // 对话图
    private Node currentNode;

    [Header("对话对象")]
    public NPCManager npcManager;

    [Header("对话面板的UI对象")]
    public TextMeshProUGUI Text_SpeakerName;
    public TextMeshProUGUI Text_SpeakerTitle;
    public TextMeshProUGUI Text_SpeakContent;
    public Button Button_Next;

    [Header("选项Button")]
    public GameObject Pre_Button_SelectGroup;
    public GameObject Pre_Button_SelectItem;

    private void Awake()
    {
        Button_Next.onClick.AddListener(Button_Onclick_Next);
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        //添加"继续"按钮的闪烁动画
        AddButtonNextAnimation();
    }

    private void OnDisable()
    {
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true,true);
        DOTween.KillAll();
    }

    private void Start()
    {
        //进入对话时，禁用玩家输入
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        if (npcManager == null)
        {
            Debug.LogError("对话者未传入？？？");
        }
        if (!CheckGraphIsInstance())
        {
            Debug.LogError("对话图为空，请检测是否已经传入！");
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
            //调用当前对话节点的方法
            RunCurrentDialogueMethod((currentNode as DialogueNode).methodName);

            // DialogueNode：走output
            NodePort outputPort = (currentNode as DialogueNode).GetOutputPort("output");
            if (outputPort != null && outputPort.Connection != null)
            {
                currentNode = outputPort.Connection.node;
                ShowCurrentNodeContent();
            }
            else //对话结束，关闭面板
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

            Button_Next.GetComponent<CanvasGroup>().interactable = false; //进入选项，禁用nextbutton交互
            Transform canvas = GameObject.Find("Canvas").transform;
            //实例选项选项Button
            GameObject buttonGroup = Instantiate(Pre_Button_SelectGroup, canvas);
            for (int i = 0; i < choiceNode.outputs.Count; i++)
            {
                int index = i; // 这里非常重要，创建局部变量副本

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
                Debug.LogWarning($"方法 {_methodName} 存在参数，无法调用（仅支持无参）。");
            }
        }
        else
        {
            Debug.LogWarning($"方法 {_methodName} 在 {npcManager.GetType().Name} 中未找到（仅搜 public）。");
        }
    }



    private bool CheckGraphIsInstance()
    {
        return graph != null;
    }

    private void AddButtonNextAnimation()
    {
        // 获取按钮的CanvasGroup组件（用于淡入淡出）
        CanvasGroup cg = Button_Next.GetComponent<CanvasGroup>();

        // 每次淡入/淡出时长
        float fadeDuration = 0.5f;
        // 停留时间
        float interval = 0.3f;

        // 创建Sequence
        Sequence seq = DOTween.Sequence();
        seq.Append(cg.DOFade(0, fadeDuration))
           .AppendInterval(interval)
           .Append(cg.DOFade(1, fadeDuration))
           .AppendInterval(interval)
           .SetLoops(-1);
    }
}

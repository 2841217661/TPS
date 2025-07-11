using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchQuestPanel : BasePanel
{
    [Header("UI����")]
    public RectTransform content;
    public Button Button_All;
    public Button Button_CanStart;
    public Button Button_InProgress;
    public Button Button_CanFinish;
    public Button Button_Finished;

    public TextMeshProUGUI Text_TaskName;
    public TextMeshProUGUI Text_Introduce;
    public TextMeshProUGUI Text_Reward;

    public GameObject branchQuestItem;
    public GameObject noQuestItem;
    public GameObject right;

    private List<Quest> quests;
    private GameObject noDisplayQuestItem;
    private Button currentSelectedButton;

    // ѡ�а�ťʱ����ɫ
    private Color selectedColor = new Color(1f, 0.85f, 0.4f);
    // δѡ�а�ť����ɫ
    private Color normalColor = Color.white;


    private void Start()
    {
        //��ʼ��quests
        quests = QuestManager.Instance.GetQuestsByState(
            QuestType.Branch,
            QuestState.CAN_START,
            QuestState.IN_PROGRESS,
            QuestState.CAN_FINISH,
            QuestState.FINISHED);

        Button_All.onClick.AddListener(() => { AnimateAndHighlight(Button_All); DisplayQuestList("û��֧������", q => true); });
        Button_CanStart.onClick.AddListener(() => { AnimateAndHighlight(Button_CanStart); DisplayQuestList("û�пɿ�����֧������", q => q.state == QuestState.CAN_START); });
        Button_InProgress.onClick.AddListener(() => { AnimateAndHighlight(Button_InProgress); DisplayQuestList("û�н����е�֧������", q => q.state == QuestState.IN_PROGRESS); });
        Button_CanFinish.onClick.AddListener(() => { AnimateAndHighlight(Button_CanFinish); DisplayQuestList("û�п���ɵ�֧������", q => q.state == QuestState.CAN_FINISH); });
        Button_Finished.onClick.AddListener(() => { AnimateAndHighlight(Button_Finished); DisplayQuestList("û������ɵ�֧������", q => q.state == QuestState.FINISHED); });

        // Ĭ��ѡ�е�һ��
        AnimateAndHighlight(Button_All);
        DisplayQuestList("û��֧������", q => true);
    }

    private void AnimateAndHighlight(Button btn)
    {
        // ������һ����ť��ɫ
        if (currentSelectedButton != null)
        {
            currentSelectedButton.GetComponent<Image>().color = normalColor;
        }

        // ������ǰ
        btn.GetComponent<Image>().color = selectedColor;
        currentSelectedButton = btn;

        // ִ�е������
        AnimateButton(btn);
    }

    private void AnimateButton(Button btn)
    {
        // DOTween����
        btn.transform.DOKill(); // ��ֹ��ε�����Ӷ���
        btn.transform.DOScale(1.1f, 0.1f)
            .OnComplete(() => btn.transform.DOScale(1f, 0.1f));
    }

    private void DisplayQuestList(string emptyText, Predicate<Quest> filter)
    {
        ClearAllQuestDisplayItems();

        var filteredQuests = quests.FindAll(filter);

        if (filteredQuests.Count == 0)
        {
            noDisplayQuestItem = Instantiate(noQuestItem, this.transform);
            noDisplayQuestItem.GetComponent<TextMeshProUGUI>().text = emptyText;
            right.SetActive(false);
            return;
        }
        right.SetActive(true);
        foreach (Quest quest in filteredQuests)
        {
            GameObject questDisplayItem = Instantiate(branchQuestItem, content);
            questDisplayItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = quest.info.id;
        }
    }

    private void ClearAllQuestDisplayItems()
    {
        if (noDisplayQuestItem != null)
        {
            Destroy(noDisplayQuestItem);
            noDisplayQuestItem = null;
        }
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}

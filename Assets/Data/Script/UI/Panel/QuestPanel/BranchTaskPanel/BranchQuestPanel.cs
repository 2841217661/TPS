using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchQuestPanel : BasePanel
{
    [Header("UI设置")]
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

    // 选中按钮时的颜色
    private Color selectedColor = new Color(1f, 0.85f, 0.4f);
    // 未选中按钮的颜色
    private Color normalColor = Color.white;


    private void Start()
    {
        //初始化quests
        quests = QuestManager.Instance.GetQuestsByState(
            QuestType.Branch,
            QuestState.CAN_START,
            QuestState.IN_PROGRESS,
            QuestState.CAN_FINISH,
            QuestState.FINISHED);

        Button_All.onClick.AddListener(() => { AnimateAndHighlight(Button_All); DisplayQuestList("没有支线任务", q => true); });
        Button_CanStart.onClick.AddListener(() => { AnimateAndHighlight(Button_CanStart); DisplayQuestList("没有可开启的支线任务", q => q.state == QuestState.CAN_START); });
        Button_InProgress.onClick.AddListener(() => { AnimateAndHighlight(Button_InProgress); DisplayQuestList("没有进行中的支线任务", q => q.state == QuestState.IN_PROGRESS); });
        Button_CanFinish.onClick.AddListener(() => { AnimateAndHighlight(Button_CanFinish); DisplayQuestList("没有可完成的支线任务", q => q.state == QuestState.CAN_FINISH); });
        Button_Finished.onClick.AddListener(() => { AnimateAndHighlight(Button_Finished); DisplayQuestList("没有已完成的支线任务", q => q.state == QuestState.FINISHED); });

        // 默认选中第一个
        AnimateAndHighlight(Button_All);
        DisplayQuestList("没有支线任务", q => true);
    }

    private void AnimateAndHighlight(Button btn)
    {
        // 重置上一个按钮颜色
        if (currentSelectedButton != null)
        {
            currentSelectedButton.GetComponent<Image>().color = normalColor;
        }

        // 高亮当前
        btn.GetComponent<Image>().color = selectedColor;
        currentSelectedButton = btn;

        // 执行点击动画
        AnimateButton(btn);
    }

    private void AnimateButton(Button btn)
    {
        // DOTween缩放
        btn.transform.DOKill(); // 防止多次点击叠加动画
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

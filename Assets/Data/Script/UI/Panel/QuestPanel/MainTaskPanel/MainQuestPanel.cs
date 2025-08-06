using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainQuestPanel : BasePanel
{
    [Header("UI设置")]
    public RectTransform content;
    public Button Button_InProgress;
    public Button Button_Finished;
    public TextMeshProUGUI Text_QuestShowState;


    public TextMeshProUGUI Text_TaskName;
    public TextMeshProUGUI Text_Introduce;
    public TextMeshProUGUI Text_Reward;

    public GameObject questItem;
    public GameObject noQuestItem;
    public GameObject right;

    private List<Quest> quests;
    private GameObject noDisplayQuestItem;
    private Button currentSelectedButton;

    // 选中按钮时的颜色
    private Color selectedColor = new Color(1f, 0.85f, 0.4f);
    // 未选中按钮的颜色
    private Color normalColor = Color.white;

    //当前选中的QuestItem
    private Color selectedQuestItemColor = new Color(1f, 0.85f, 0.4f);
    // 未选中QuestItem的颜色
    private Color normalQuestItemColor = Color.white;
    private Button currentSelectItemButton;


    private void Start()
    {
        //初始化quests
        quests = QuestManager.Instance.GetQuestsByState(
            QuestType.Main,
            QuestState.CAN_START,
            QuestState.IN_PROGRESS,
            QuestState.CAN_FINISH,
            QuestState.FINISHED);

        Button_InProgress.onClick.AddListener(() => { AnimateAndHighlight(Button_InProgress); DisplayQuestList("没有进行中的主线任务", "进行中", q => q.state == QuestState.IN_PROGRESS); });
        Button_Finished.onClick.AddListener(() => { AnimateAndHighlight(Button_Finished); DisplayQuestList("没有已完成的主线任务","已完成", q => q.state == QuestState.FINISHED); });

        // 默认选中第一个
        AnimateAndHighlight(Button_InProgress);
        DisplayQuestList("没有进行中的主线任务","进行中", q => q.state == QuestState.IN_PROGRESS);
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

    private void DisplayQuestList(string emptyText, string showState,Predicate<Quest> filter)
    {
        ClearAllQuestDisplayItems();

        var filteredQuests = quests.FindAll(filter);

        Text_QuestShowState.text = showState;


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
            GameObject questDisplayItem = Instantiate(questItem, content);

            //为每个QuestItem添加点击事件
            QuestItem item = questDisplayItem.GetComponent<QuestItem>();
            //如果任务已经完成，则就没有Step了
            try
            {
                item.title = quest.GetCurrentQuestStepPrefab().name.Substring(3);
            }
            catch
            {
                //已经完成的任务没有questStep,自然GetCurrentQuestStepPrefab会超出索引
                item.title = quest.info.id;
            }
            item.description = quest.info.questDescription;
            item.reward = quest.info.rewardDescription;
            //默认选中第一个
            if (quest == filteredQuests[0])
            {
                ShowRightInfo(item);
            }
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowRightInfo(item);
            });

            //显示左边Item的信息
            item.UI_chapter.text = quest.info.chapter;
            item.UI_chapter.text = quest.info.id;
        }
    }

    private void ShowRightInfo(QuestItem _item)
    {
        // 还原上一个按钮颜色和缩放（如果有）
        if (currentSelectItemButton != null)
        {
            currentSelectItemButton.image.color = normalQuestItemColor;
            currentSelectItemButton.transform.DOScale(Vector3.one, 0.1f);
        }

        // 设置当前按钮
        currentSelectItemButton = _item.GetComponent<Button>();

        // 修改颜色
        currentSelectItemButton.image.color = selectedQuestItemColor;

        // 播放点击缩放动画
        currentSelectItemButton.transform
            .DOScale(0.95f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                currentSelectItemButton.transform
                    .DOScale(1f, 0.2f)
                    .SetEase(Ease.OutBack);
            });

        // 设置右侧信息
        Text_TaskName.text = _item.title;
        Text_Introduce.text = _item.description;
        Text_Reward.text = _item.reward;
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

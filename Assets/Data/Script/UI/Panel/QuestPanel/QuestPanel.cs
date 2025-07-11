using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class QuestPanel : BasePanel
{
    [Header("UI设置")]
    public Button Button_MainQuest;
    public Button Button_BranchQuest;
    public TextMeshProUGUI Text_Title;
    public Button Button_Cancle;

    private Button currentSelectedButton;
    private GameObject currentQuestPanel;

    // 自定义颜色
    private Color selectedColor = new Color(1f, 0.85f, 0.4f);
    private Color normalColor = Color.white;

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        // 同步打开MainTaskPanel
        currentQuestPanel = UIManager.Instance.OpenPanel("MainQuestPanel", this.transform).gameObject;

        Text_Title.text = "主线任务";

        // 给按钮加监听
        Button_MainQuest.onClick.AddListener(() =>
        {
            if (AnimateAndHighlight(Button_MainQuest))
                OnMainQuestClicked();
        });

        Button_BranchQuest.onClick.AddListener(() =>
        {
            if (AnimateAndHighlight(Button_BranchQuest))
                OnBranchQuestClicked();
        });

        Button_Cancle.onClick.AddListener(ClosePanel);

        // 默认选中主线
        AnimateAndHighlight(Button_MainQuest);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
    }

    /// <summary>
    /// 播放按钮动效和高亮逻辑
    /// 返回true表示是新点击，需要切换面板
    /// 返回false表示重复点击，无需再切换
    /// </summary>
    private bool AnimateAndHighlight(Button btn)
    {
        // 如果重复点击，直接return false
        if (currentSelectedButton == btn)
        {
            Debug.Log("重复点击了 " + btn.name);
            return false;
        }

        // 重置上一个按钮
        if (currentSelectedButton != null)
        {
            currentSelectedButton.GetComponent<Image>().color = normalColor;
        }

        // 设置当前按钮
        btn.GetComponent<Image>().color = selectedColor;
        currentSelectedButton = btn;

        // 播放动效
        btn.transform.DOKill(); // 防止叠加
        btn.transform.DOScale(1.1f, 0.1f).OnComplete(() =>
        {
            btn.transform.DOScale(1f, 0.1f);
        });

        return true;
    }

    private void OnMainQuestClicked()
    {
        Debug.Log("切换为 Main");

        if (currentQuestPanel != null)
            Destroy(currentQuestPanel);

        currentQuestPanel = UIManager.Instance.OpenPanel("MainQuestPanel", this.transform).gameObject;
        Text_Title.text = "主线任务";
    }

    private void OnBranchQuestClicked()
    {
        Debug.Log("切换为 Branch");

        if (currentQuestPanel != null)
            Destroy(currentQuestPanel);

        currentQuestPanel = UIManager.Instance.OpenPanel("BranchQuestPanel", this.transform).gameObject;
        Text_Title.text = "支线任务";
    }
}

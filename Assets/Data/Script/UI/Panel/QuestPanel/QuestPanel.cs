using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class QuestPanel : BasePanel
{
    [Header("UI����")]
    public Button Button_MainQuest;
    public Button Button_BranchQuest;
    public TextMeshProUGUI Text_Title;
    public Button Button_Cancle;

    private Button currentSelectedButton;
    private GameObject currentQuestPanel;

    // �Զ�����ɫ
    private Color selectedColor = new Color(1f, 0.85f, 0.4f);
    private Color normalColor = Color.white;

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        // ͬ����MainTaskPanel
        currentQuestPanel = UIManager.Instance.OpenPanel("MainQuestPanel", this.transform).gameObject;

        Text_Title.text = "��������";

        // ����ť�Ӽ���
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

        // Ĭ��ѡ������
        AnimateAndHighlight(Button_MainQuest);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
    }

    /// <summary>
    /// ���Ű�ť��Ч�͸����߼�
    /// ����true��ʾ���µ������Ҫ�л����
    /// ����false��ʾ�ظ�������������л�
    /// </summary>
    private bool AnimateAndHighlight(Button btn)
    {
        // ����ظ������ֱ��return false
        if (currentSelectedButton == btn)
        {
            Debug.Log("�ظ������ " + btn.name);
            return false;
        }

        // ������һ����ť
        if (currentSelectedButton != null)
        {
            currentSelectedButton.GetComponent<Image>().color = normalColor;
        }

        // ���õ�ǰ��ť
        btn.GetComponent<Image>().color = selectedColor;
        currentSelectedButton = btn;

        // ���Ŷ�Ч
        btn.transform.DOKill(); // ��ֹ����
        btn.transform.DOScale(1.1f, 0.1f).OnComplete(() =>
        {
            btn.transform.DOScale(1f, 0.1f);
        });

        return true;
    }

    private void OnMainQuestClicked()
    {
        Debug.Log("�л�Ϊ Main");

        if (currentQuestPanel != null)
            Destroy(currentQuestPanel);

        currentQuestPanel = UIManager.Instance.OpenPanel("MainQuestPanel", this.transform).gameObject;
        Text_Title.text = "��������";
    }

    private void OnBranchQuestClicked()
    {
        Debug.Log("�л�Ϊ Branch");

        if (currentQuestPanel != null)
            Destroy(currentQuestPanel);

        currentQuestPanel = UIManager.Instance.OpenPanel("BranchQuestPanel", this.transform).gameObject;
        Text_Title.text = "֧������";
    }
}

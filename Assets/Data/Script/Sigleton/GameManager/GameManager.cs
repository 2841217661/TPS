using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    public PlayerManager playerManager;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //���ԣ����������
        //UIManager.Instance.OpenPanel("QuestPanel");

        //���ԣ�����һ��
        QuestManager.Instance.TryStartQuest(QuestName.��ׯ���֤);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.OpenPanel("QuestPanel");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UIManager.Instance.CloseCurrentPanel();
        }
    }
}

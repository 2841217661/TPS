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
        //测试：打开任务面板
        //UIManager.Instance.OpenPanel("QuestPanel");

        //测试：开启一个
        QuestManager.Instance.TryStartQuest(QuestName.村庄许可证);
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

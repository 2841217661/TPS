using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

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
        //3s后开启第一个任务
        //StartCoroutine(QuestManager.Instance.DelayedQuestStart(3f,QuestName.Quest_村庄许可证));
    }

    private void Update()
    {
        //测试：打开任务面板
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.OpenPanel("QuestPanel");
        }

        //测试：关闭最近打开的面板
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseCurrentPanel();
        }
    }
}

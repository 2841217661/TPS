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
    }

    private void Update()
    {
        //测试：开启与蒙面男的对话任务
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}

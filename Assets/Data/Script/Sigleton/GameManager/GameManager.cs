using DG.Tweening;
using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    public PlayerManager playerManager;
    public Transform[] enemyPatrolPoints;

    [Header("测试")]
    public int maxEnemyCount;
    public int currentEnemyCount;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 随机获取一个巡逻点
    /// </summary>
    /// <param name="exclude">排除该巡逻点</param>
    /// <returns></returns>
    public Transform RandomSetPatrolPoint(Transform exclude)
    {
        Transform next;
        do
        {
            next = enemyPatrolPoints[Random.Range(0, enemyPatrolPoints.Length)];
        }
        while (next == exclude);

        return next;
    }




    private void Start()
    {
        //测试----------------------------------
        // 打开黑幕面板
        ScreenFadePanel panel = UIManager.Instance.OpenPanel("ScreenFadePanel", UIManager.Instance.UIRoot) as ScreenFadePanel;

        panel.FadeIn(
            onStart: () =>
            {
                GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, false);
            },
            onComplete: () =>
            {
                Debug.Log("实例敌人。。。");

                // 2秒后执行渐出
                DOVirtual.DelayedCall(2f, () =>
                {
                    panel.FadeOut(
                        onStart: () =>
                        {

                        },
                        onComplete: () =>
                        {
                            GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
                            panel.ClosePanel();
                        }
                    );
                });
            });


    }

    private void Update()
    {
        // 测试：打开任务面板
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.OpenPanel("QuestPanel");
        }

        // 测试：关闭最近打开的面板
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseCurrentPanel();
        }

        // 切换鼠标显示/隐藏
        if (Input.GetMouseButtonDown(2)) // 鼠标中键
        {
            // 切换状态
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

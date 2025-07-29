using DG.Tweening;
using TMPro;
using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    public PlayerManager playerManager;
    public Transform[] enemyPatrolPoints;

    [Header("测试")]
    public int maxEnemyCount;
    public int currentEnemyCount;

    [Header("伤害数值颜色")]
    public Color color_Physical;
    public Color color_Fire;

    [Header("受伤屏幕闪烁")]
    public PlayerDamageScreenEffect playerDamageScreenEffect;

    [Header("玩家信息面板")]
    public PlayerInfo playerInfo;

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

    /// <summary>
    /// 生成一个伤害数字飘动效果
    /// </summary>
    /// <param name="_value">伤害数值</param>
    /// <param name="_element">伤害类型</param>
    /// <param name="_position">生成点</param>
    /// <param name="_isCritical">是否是暴击伤害</param>
    public void GenerateDamageTextEffect(float _value,  Vector3 _position, DamageElement _element, bool _isCritical)
    {
        //伤害值太小不显示
        //if (_value < 5f) return;

        //转换为屏幕坐标
        Vector3 screentPosition = Camera.main.WorldToScreenPoint(_position);

        if (screentPosition.z < 0)
            return;

        screentPosition -= new Vector3(Screen.width / 2f, Screen.height / 2f);

        //现在得到的是实际伤害点的位置，添加一定的偏移量会使视觉效果更佳,长以50像素作为偏差，宽以30像素作为偏差
        Vector3 randomOffset = new Vector2(
            Random.Range(-50, 50),
            Random.Range(-30, 30)
        );

        screentPosition += randomOffset;

        //从池中取出一个
        var obj = PoolManager.Instance.Spawn(PoolManager.Instance.damageText.name, Vector3.zero, Quaternion.identity);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = screentPosition;

        // 获取脚本并设置文字样式
        DamageText dt = obj.GetComponent<DamageText>();
        Color color = Color.black;
        switch (_element)
        {
            case DamageElement.Physical:
                color = color_Physical;
                break;
            case DamageElement.Fire:
                color = color_Fire;
                break;
            default:
                Debug.LogWarning("类型未设置");
                break;
        }

        dt.Setup(_value.ToString(), color);
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

        // 测试：打开buff选择面板
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIManager.Instance.OpenPanel("BuffSelectPanel",UIManager.Instance.UIRoot);
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

        //测试：持续增加经验
        playerManager.currentExperienceValue += 1f;
    }
}

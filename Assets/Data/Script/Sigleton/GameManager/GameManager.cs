using DG.Tweening;
using TMPro;
using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    public PlayerManager playerManager;
    public Transform[] enemyPatrolPoints;

    [Header("怪物生成数量控制")]
    public int maxEnemyCount;
    public int currentEnemyCount;

    [Header("怪物传送门")]
    public GameObject 怪物传送门;

    [Header("NPC")]
    public Transform npcRoot;

    [Header("补给")]
    public float supplyInterval; //补给的时间间隔
    public float supplyIntervalTimer; //补给倒计时
    public Transform[] supplyPoints;  //补给掉落点数组

    //辅助方法：随机获取一个补给点位置
    private Transform RandomSelectSupplyPoint()
    {
        return supplyPoints[Random.Range(0,supplyPoints.Length)];
    }

    //辅助方法：判断补给间隔是否达到
    private bool IsCaculateSupplyIntervalEnd()
    {
        supplyIntervalTimer += Time.deltaTime;

        if(supplyIntervalTimer > supplyInterval)
        {
            supplyIntervalTimer = 0f;
            return true;
        }

        return false;
    }

    private void TryAddSupply()
    {
        if (IsCaculateSupplyIntervalEnd())
        {
            Transform supplyPoint = RandomSelectSupplyPoint();

            //TODO:实例补给
        }
    }

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
        if (_value < 2f) return;

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
                color = new Color(255f/255f, 255f/255f, 255f/255f,1f);
                break;
            case DamageElement.Fire:
                color = new Color(255f/255f, 100f/255f, 0f/255f,1f);
                break;
            default:
                Debug.LogWarning("类型未设置");
                break;
        }

        dt.Setup(_value.ToString(), color, _isCritical);
    }


    //辅助方法：通过消耗物品的id来查找Resources下对应的资源
    private GameObject GetConsumableItemById(string _itemName)
    {
        string itemPath = "Prefabs/ConsumableItems/" + _itemName;
        return Resources.Load<GameObject>(itemPath);
    }

    /// <summary>
    /// 增加消耗品
    /// </summary>
    /// <param name="_itemId">物品</param>
    /// <param name="_count">个数</param>
    public void AddConsumableItem(string _itemId, int _count = 1)
    {
        //查找playermanager是否含有该物品（通过物品名）：
        //没有：新添加一个
        //有：数量加一
        foreach (var item in playerManager.consumableItems)
        {
            ConsumableItem ci = item.GetComponent<ConsumableItem>();
            if (ci.itemId == _itemId) //已有，数量加一
            {
                ci.count++;
            }
            else //没有，加入
            {
                var obj = GetConsumableItemById(_itemId);
                obj.GetComponent<ConsumableItem>().count = _count;
                playerManager.consumableItems.Add(obj);
                //TODO:同步ui
            }
        }
    }

    /// <summary>
    /// 使用一次当前选中的消耗物品
    /// </summary>
    /// <param name="_count">消耗数量</param>
    /// <returns></returns>
    public GameObject UseCurrentSelectItem()
    {
        var obj = Instantiate(playerManager.currentSelectConsumableItem);

        ConsumableItem ci = playerManager.currentSelectConsumableItem.GetComponent<ConsumableItem>();
        ci.count--;
        if(ci.count <= 0) //消耗完了
        {
            playerManager.consumableItems.Remove(playerManager.currentSelectConsumableItem);
        }

        if(playerManager.consumableItems.Count > 0)
        {
            playerManager.currentSelectConsumableItem = playerManager.consumableItems[0];
        }
        else
        {
            playerManager.currentSelectConsumableItem = null;
        }

        return obj;
    }


    private void Start()
    {
        //测试
        UIUtils.ScreenFadeTransition(
            delay: 2f,
            onFadeInStart: () =>
            {
                GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, false);
            },
            onFadeInComplete: () =>
            {
                GameManager.Instance.怪物传送门.SetActive(true);
                GameManager.Instance.npcRoot.gameObject.SetActive(false);
            },
            onFadeOutStart: () =>
            {
                Debug.LogWarning("战斗开始！！！");
            },
            onFadeOutComplete: () =>
            {
                GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
            }
        );

        //测试
        QuestManager.Instance.TryStartQuest(QuestName.Quest_升级1);
    }

    private void Update()
    {

        //TryAddSupply();

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
    }
}

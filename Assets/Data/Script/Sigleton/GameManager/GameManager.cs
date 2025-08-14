using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
    public float buffSupplyInterval; //buff补给的时间间隔
    private float buffSupplyIntervalTimer; //buff补给倒计时
    public float storeSupplyInterval; //商人补给的时间间隔
    private float storeSupplyIntervalTimer; //商人补给倒计时
    public Transform traderPoint; //商人刷新位置
    public Transform[] supplyPoints;  //buff补给掉落点数组
    public GameObject buffSupplyPre; //buff补给预制体

    [Header("玩家总共击杀的怪物数量")]
    public int playerKillEnemyCount;


    #region 补给
    //辅助方法：随机获取一个buff补给点位置
    private Transform RandomSelectSupplyPoint()
    {
        return supplyPoints[Random.Range(0, supplyPoints.Length)];
    }

    //辅助方法：判断buff补给间隔是否达到
    private bool IsCaculateBuffSupplyIntervalEnd()
    {
        buffSupplyIntervalTimer += Time.deltaTime;

        if (buffSupplyIntervalTimer > buffSupplyInterval)
        {
            buffSupplyIntervalTimer = 0f;
            return true;
        }

        return false;
    }

    //辅助方法：判断商人补给间隔是否达到
    private bool IsCaculateStoreSupplyIntervalEnd()
    {
        storeSupplyIntervalTimer += Time.deltaTime;

        if (storeSupplyIntervalTimer > storeSupplyInterval)
        {
            storeSupplyIntervalTimer = 0f;
            return true;
        }

        return false;
    }

    //实例神秘商店补给,位置是固定的
    private void TryAddStoreSupply()
    {
        if (!IsCaculateStoreSupplyIntervalEnd()) return;

        GameObject go = Instantiate(NormalPanel.Instance.RandomEventNoticeItemPre, NormalPanel.Instance.RandomEventNoticePoint);
        go.GetComponentInChildren<TextMeshProUGUI>().text = "神秘商店已刷新";
        Instantiate(Resources.Load<GameObject>("Prefabs/NPC/NPC_商人"), traderPoint.position, Quaternion.identity);
    }

    //实例一个buff，随机位置
    public void TryBuffSelectSupply()
    {
        if (!IsCaculateBuffSupplyIntervalEnd()) return;

        GameObject go = Instantiate(NormalPanel.Instance.RandomEventNoticeItemPre, NormalPanel.Instance.RandomEventNoticePoint);
        go.GetComponentInChildren<TextMeshProUGUI>().text = "Buff补给已刷新";
        Instantiate(Resources.Load<GameObject>("Prefabs/Supply/BuffSupply"), RandomSelectSupplyPoint().position, Quaternion.identity);
    }
    #endregion



    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
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


    /// <summary>
    /// 增加消耗品
    /// </summary>
    /// <param name="_itemId">物品id</param>
    /// <param name="_count">个数</param>
    public void GetConsumableItem(string _itemId, int _count = 1)
    {

        foreach(var ci in playerManager.consumableItems)
        {
            if(ci.itemId == _itemId)
            {
                ci.count += _count;

                //杂项事件回调
                EventManager.Instance.sundryEvent.ConItemGet(_itemId,_count);

                return;
            }
        }
        Debug.LogError("添加的消耗物品道具Id不存在");
    }

    /// <summary>
    /// 使用一次当前选中的消耗物品
    /// </summary>
    public GameObject UseCurrentSelectItem(string _itemId)
    {
        ConsumableItemSO ci = playerManager.currentSelectConItemSO;
        if (ci.count <= 0)
        {
            Debug.LogWarning("道具已经使用完了: " + ci.itemId);
            return null;
        }

        ci.count--;

        //杂项事件回调
        EventManager.Instance.sundryEvent.ConItemUsed(_itemId);

        return ci.MakeItem();
    }

    //切换下一个道具
    public void ChangeCurrentSelectConsumableItem()
    {
        if (!playerManager.canChangeConItem)
        {
            Debug.LogWarning("当前状态无法切换道具");
            return;
        }

        NormalPanel.Instance.consumableItemCarouselView.OnClickNext();

        //playerManager.currentSelectConsumableItem = NormalPanel.Instance.consumableItemCarouselView.m_itemDic[0].GetComponent<UIConsumableItemView>().consumableItem;
    }

    private void Start()
    {

        ////测试
        //UIUtils.ScreenFadeTransition(
        //    delay: 2f,
        //    onFadeInStart: () =>
        //    {
        //        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, false);
        //    },
        //    onFadeInComplete: () =>
        //    {
        //        GameManager.Instance.怪物传送门.SetActive(true);
        //        GameManager.Instance.npcRoot.gameObject.SetActive(false);
        //    },
        //    onFadeOutStart: () =>
        //    {
        //        Debug.LogWarning("战斗开始！！！");
        //    },
        //    onFadeOutComplete: () =>
        //    {
        //        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
        //    }
        //);

        ////测试
        //QuestManager.Instance.TryStartQuest(QuestName.Quest_升级1);
        //QuestManager.Instance.TryStartQuest(QuestName.Quest_杀怪1);
    }

    private void Update()
    {
        TryAddStoreSupply(); 
        TryBuffSelectSupply();


        // 测试：打开任务面板
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.OpenPanel("QuestPanel");
        }

        // 测试：打开buff选择面板
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIManager.Instance.OpenPanel("BuffSelectPanel",UIManager.Instance.UIRoot);
            //playerManager.buffSystem.AddBuff<B_生命药水>();
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

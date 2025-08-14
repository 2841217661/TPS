using DG.Tweening;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BuffSelectPanel : BasePanel
{

    public GameObject buffSelectItemPre;
    public Transform content;
    public TextMeshProUGUI tip;
    public float blinkDuration = 0.5f; // 每次淡入或淡出的时间
    private Tween blinkTween;
    public Button cancle;

    public override void ClosePanel()
    {
        base.ClosePanel();

        GameManager.Instance.Continue();

        StopBlinking();

        //恢复ui和player输入
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.Pause();

        //禁用ui和player输入
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false,false);

        //为文本添加闪烁效果
        if (tip != null)
        {
            StartBlinking();
        }

        //为取消button添加事件
        cancle.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        //随机实例三个buff选项
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate<GameObject>(buffSelectItemPre, content);

            // 4. 使用返回值

            BuffData _buffData = GetRandomBuffDataByType();
            ClickEffect clickEffect = obj.GetComponent<ClickEffect>();
            clickEffect.buffData = _buffData;

            var (canSelect, info)= CheckBuffCanBeSelect(_buffData);
            if (!canSelect)
            {
                clickEffect.GetComponent<CanvasGroup>().interactable = false;
                clickEffect.dontSelect.SetActive(true);
                clickEffect.dontSelect.GetComponentInChildren<TextMeshProUGUI>().text = info.ToString();
            }
            else
            {
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    string typeName = "B_" + obj.GetComponent<ClickEffect>().buffData.buffName;

                    Type type = Type.GetType(typeName); // 你需要确保类型名是完整的，包括命名空间

                    if (type == null)
                    {
                        Debug.LogError($"无法找到类型 {typeName}");
                        return;
                    }

                    // 检查是否是 BuffBase 的子类
                    if (!typeof(BuffBase).IsAssignableFrom(type))
                    {
                        Debug.LogError($"{typeName} 不是 BuffBase 的子类");
                        return;
                    }

                    // 获取泛型方法定义
                    MethodInfo method = typeof(BuffSystem).GetMethod("AddBuff", BindingFlags.Public | BindingFlags.Instance);

                    // 构造泛型方法
                    MethodInfo generic = method.MakeGenericMethod(type);

                    // 调用（this 是你调用 AddBuff 的实例）
                    generic.Invoke(GameManager.Instance.playerManager.buffSystem, new object[] { 1 }); // 参数 heap = 1

                    obj.GetComponent<ClickEffect>().buffSelectPanel = this;
                });
            }
        }
    }

    //随机获取一个buff类型
    private BuffData GetRandomBuffDataByType()
    {
        Type thisBuffType = BuffDataManager.GetRandomBuffType();

        // 1. 获取泛型方法定义
        MethodInfo method = typeof(BuffDataManager).GetMethod("GetBuffData", BindingFlags.Static | BindingFlags.Public);

        // 2. 构造泛型方法（用 thisBuffType 替代 T）
        MethodInfo genericMethod = method.MakeGenericMethod(thisBuffType);

        // 3. 执行方法并获取结果（null 表示静态方法，没有参数）
        BuffData buff = (BuffData)genericMethod.Invoke(null, null);

        return buff;
    }

    //检查PlayerSystem中是否可以添加该buff，或者说满足添加这个buff的条件
    //例如：需要添加的buff需要前置buff(此时应当无法选择)；
    //例如：当前buff层数已经达到上限(此时应该提升玩家当前选择这个buff没有效果，但是可以选择)
    private (bool canSelect,string info) CheckBuffCanBeSelect(BuffData _buffData)
    {
        BuffSystem playerBuffsystem = GameManager.Instance.playerManager.buffSystem;
        switch (_buffData.conflictResolution)
        {
            /*该buff是独立存在的：目前没有限制*/
            case ConflictResolution.separate:
                return (true, null);
            /*该buff是进行合并的：如果达到最大层数，则提升继续添加无效*/
            case ConflictResolution.combine:
                //准备添加的buff在buffSystem中不存在，则说明是新buff，可以添加：
                BuffBase buffBase = playerBuffsystem.CheckBuffIsExist(_buffData);

                //当前不存在该buff
                if (buffBase == null)
                {
                    //判断当前添加的buff是否含有前置buff：
                    if (_buffData.PreBuffData.Length > 0)
                    {
                        BuffData[] pre_buff_datas = _buffData.PreBuffData; //获取当前buff的前置buff

                        //判断当前buffs中是否包含该buff的所有前置buff                                              
                        bool allContained = pre_buff_datas.All(preBuff =>
                            GameManager.Instance.playerManager.buffSystem.buffs.Any(b => b.buffData == preBuff)
                        );

                        //没有包含所有前置buff，无法添加buff
                        if (!allContained)
                        {
                            return (false, "无法添加该Buff");
                        }
                        else
                        {
                            //包含所有前置条件，可以添加
                            return (true, null);
                        }
                    }
                    //当前添加的buff没有前置buff
                    else 
                    {
                        //直接添加
                        return (true, null);
                    }
                }

                //既然已经有了该buff，判断是否处于满级状态
                if (buffBase.CurrentLevel != buffBase.buffData.maxLevel) return (true, null);

                //已经满级了...无法添加，就算添加也不会有效果
                return (false,"当前buff已达到满级");
            /*该buff是覆盖类型的：可以直接添加*/
            case ConflictResolution.cover:
                return (true,null);
            default:
                Debug.LogError("没有类型：" + _buffData.conflictResolution);
                return (false, null);
        }
    }

    public void StartBlinking()
    {
        // 停止之前的 Tween（如果有）
        StopBlinking();

        // 设置循环淡入淡出
        blinkTween = tip.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }

    public void StopBlinking()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
            tip.alpha = 1f; // 恢复透明度
        }
    }
}

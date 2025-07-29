using DG.Tweening;
using System;
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

        StopBlinking();

        //恢复ui和player输入
        GameManager.Instance.playerManager.inputManager.ApplyActionMap(true, true);
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

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
            obj.GetComponent<ClickEffect>().buffData = GetBuffDataByType();

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

    private BuffData GetBuffDataByType()
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

    public void StartBlinking()
    {
        // 停止之前的 Tween（如果有）
        StopBlinking();

        // 设置循环淡入淡出
        blinkTween = tip.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
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

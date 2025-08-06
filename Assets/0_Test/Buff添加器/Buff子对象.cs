using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buff子对象 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public BuffData buffData;

    [Header("UI设置")]
    public TextMeshProUGUI Name;
    public Image Icon;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Buff添加器.Instance.描述1.text = buffData.buffDescript;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Buff添加器.Instance.描述1.text = "请先将鼠标放置Buff上！";
    }

    private void Start()
    {
        Name.text = buffData.buffName;
        Icon.sprite = buffData.icon;

        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            AddBuffByBuffName(buffData.buffName);
        });
    }

    private void AddBuffByBuffName(string _buffName)
    {
        switch (_buffName)
        {
            case "风之祝福":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_风之祝福>();
                break;
            case "狂暴火力":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_狂暴火力>();
                break;
            case "胖血模式":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_胖血模式>();
                break;
            case "怒火焚身":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_怒火焚身>();
                break;
            case "追踪子弹":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_追踪子弹>();
                break;
            case "火焰弹头":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_火焰弹头>();
                break;
            case "淘汰回放":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_淘汰回放>();
                break;
            case "危机合约":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_危机合约>();
                break;
            case "枪枪重击":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_枪枪重击>();
                break;
            case "生命药水":
                Buff添加器.Instance.actorBuffSystem.AddBuff<B_生命药水>();
                break;
            default:
                Debug.LogWarning("buff类型没有找到: " + "B_" + _buffName);
                break;
        }
    }
}

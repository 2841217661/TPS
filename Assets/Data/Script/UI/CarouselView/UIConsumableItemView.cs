using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIConsumableItemView : MonoBehaviour
{
    public ConsumableItemSO consumableItem;
    public Image icon;
    public TextMeshProUGUI count;

    private void Awake()
    {
        icon.sprite = consumableItem.icon;
        count.text = consumableItem.count.ToString();
    }

    private void OnEnable()
    {
        EventManager.Instance.sundryEvent.onConItemUsed += ConItemUsed;
        EventManager.Instance.sundryEvent.onConItemGet += ConItemGet; 
    }

    private void OnDisable()
    {
        if (EventManager.Instance == null) return;

        EventManager.Instance.sundryEvent.onConItemGet -= ConItemGet; 
    }

    private void ConItemUsed(string _itemId)
    {
        if(consumableItem.itemId != _itemId) return;

        Debug.Log("消耗物品事件回调: " +  _itemId);
        count.text = consumableItem.count.ToString();
    }

    //获得物品事件回调
    private void ConItemGet(string _itemId, int _count)
    {
        if (consumableItem.itemId != _itemId) return;

        Debug.Log($"获得物品{_itemId}:{_count}");
        count.text = consumableItem.count.ToString();
    }
}

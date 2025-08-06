using UnityEngine;

public class ConItemCarouseView : CarouselView
{
    protected override void OnAfterInif()
    {
        base.OnAfterInif();

        GameManager.Instance.playerManager.currentSelectConItemSO = m_itemDic[0].GetComponent<UIConsumableItemView>().consumableItem;
        for (int i = 0; i < m_itemCount; i++)
        {
            GameManager.Instance.playerManager.consumableItems.Add(m_itemDic[i].GetComponent<UIConsumableItemView>().consumableItem);
        }
    }

    protected override void OnNextClick()
    {
        base.OnNextClick();

        GameManager.Instance.playerManager.currentSelectConItemSO = m_itemDic[0].GetComponent<UIConsumableItemView>().consumableItem;
    }

    protected override void OnPreClick()
    {
        base.OnNextClick();

        GameManager.Instance.playerManager.currentSelectConItemSO = m_itemDic[0].GetComponent<UIConsumableItemView>().consumableItem;
    }
}

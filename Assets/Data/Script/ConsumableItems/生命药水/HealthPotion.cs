using UnityEngine;

public class HealthPotion : ConsumableItem
{
    private float effectSize = 50f;

    private void Awake()
    {
        MakeItem();
    }

    protected override void MakeItem()
    {
        GameManager.Instance.playerManager.currentHealthValue += effectSize;
        Destroy(gameObject);
    }
}

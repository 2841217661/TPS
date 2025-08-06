using UnityEngine;

public class B_生命药水 : BuffBase
{
    private float effectSize = 10f; //每秒恢复多少生命值
    private GameObject healthEffect;
    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        //实例生命恢复特效
        Transform player = GameManager.Instance.playerManager.transform;
        healthEffect = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ConsumableItems/Item/生命药水"),
            player.position + Vector3.up * 0.5f,
            player.rotation,
            player);
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        GameObject.Destroy(healthEffect);
    }

    public override void Update()
    {
        base.Update();

        characterManager.currentHealthValue += effectSize * Time.deltaTime;
    }
}

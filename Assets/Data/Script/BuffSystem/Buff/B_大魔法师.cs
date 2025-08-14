using UnityEngine;

public class B_大魔法师 : BuffBase
{
    private GameObject go;
    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Pet/魔法师/魔法师"), GameManager.Instance.playerManager.transform.position, Quaternion.identity);
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        GameObject.Destroy(go);
    }
}

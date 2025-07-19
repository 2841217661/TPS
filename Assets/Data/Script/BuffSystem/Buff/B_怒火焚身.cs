using UnityEngine;

public class B_怒火焚身 : BuffBase
{
    public int orbitObjectCount = 1; //每级增加的火球数量
    private GameObject orbitSpawner;

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        orbitSpawner = Object.Instantiate(Resources.Load<GameObject>("Prefabs/BuffObject/怒火焚身环绕器"));
    }

    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        orbitSpawner.GetComponent<怒火焚烧环绕器>().AddOrbitObject(orbitObjectCount);
    }
}

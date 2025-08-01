using UnityEngine;

public class B_火焰印记 : BuffBase
{
    protected override void OnCurrentLevelChange(int change)
    {
        base.OnCurrentLevelChange(change);

        if(CurrentLevel >= 50)
        {
            //进行一次爆炸
            Debug.Log("爆炸一次");
            var explode = PoolManager.Instance.Spawn(PoolManager.Instance.火焰印记爆炸.name,characterManager.transform.position, characterManager.transform.rotation);
            characterManager.buffSystem.RemoveBuff<B_火焰印记>();
        }
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        Debug.Log("火焰印记被移除");
    }
}

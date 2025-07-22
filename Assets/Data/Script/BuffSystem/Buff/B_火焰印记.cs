using UnityEngine;

public class B_火焰印记 : BuffBase
{
    protected override void OnCurrentLevelChange(int change)
    {
        if(CurrentLevel >= 2)
        {
            //进行一次爆炸
            Debug.Log("爆炸一次");
            var explode = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BuffObject/火焰印记爆炸"), characterManager.transform.position, characterManager.transform.rotation);
            characterManager.buffSystem.RemoveBuff<B_火焰印记>();
        }
    }

    public override void AfterBeRemoved()
    {
        Debug.Log("火焰印记被移除");
    }
}

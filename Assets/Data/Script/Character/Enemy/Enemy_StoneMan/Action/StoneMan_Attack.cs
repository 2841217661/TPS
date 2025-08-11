using UnityEngine;

public class StoneMan_Attack : Ordinary_Attack
{
    public override void OnStart()
    {
        base.OnStart();

        //80%¸ÅÂÊµ¥È­¹¥»÷£¬20%Ë«È­´¸»÷
        self.Value.animator.SetFloat("Attack", Random.Range(0, 1f) > 0.8f ? 1 : 0);
    }
}

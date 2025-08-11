using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class StoneMan_Death : Ordinary_Death
{
    public override void OnStart()
    {
        base.OnStart();

        self.Value.animator.CrossFade("Death", 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = self.Value.animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1f)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}

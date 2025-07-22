using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_KnockBack : Action
{
    public SharedEnemyManager self;
    private bool isEnterDamageAnim;
    public override void OnStart()
    {
        isEnterDamageAnim = false;

        self.Value.animator.CrossFade("KnockBack", 0.1f);
        self.Value.animator.applyRootMotion = true;
    }

    public override TaskStatus OnUpdate()
    {
        //渡入/渡出/播放中 不能再次进入

        AnimatorStateInfo stateInfo = self.Value.animator.GetCurrentAnimatorStateInfo(0);
        if (!isEnterDamageAnim && stateInfo.IsName("KnockBack")) //说明过渡到Damage已经完成
        {
            isEnterDamageAnim = true;
            return TaskStatus.Running;

        }
        else if (stateInfo.IsName("Idle") && isEnterDamageAnim) //说明受击动画已经过渡Idle完毕
        {
            self.Value.state = EnemyState.Idle;
            return TaskStatus.Success;
        }

        //渡入和渡出只能让动画播放完
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        isEnterDamageAnim = false;

        self.Value.animator.applyRootMotion = false;
    }
} 
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_Follow : Action
{
    public SharedPetManager self;
    public float stopDistance;

    public override void OnStart()
    {
        base.OnStart();

        self.Value.agent.enabled = true;

        self.Value.animator.CrossFade("Walk", 0.1f);
    }

    //public override TaskStatus OnUpdate()
    //{
    //    //跟随目标，直到达到指定距离
    //    // 计算与目标的距离
    //    float distance = Vector3.Distance(transform.position, self.Value.followTarget.position);

    //    if (distance > stopDistance)
    //    {
    //        // 距离太远，移动到目标
    //        self.Value.agent.SetDestination(self.Value.followTarget.position);
    //    }
    //    else
    //    {
    //        // 距离足够近，停止移动
    //        self.Value.agent.ResetPath();
    //    }
    //}
}

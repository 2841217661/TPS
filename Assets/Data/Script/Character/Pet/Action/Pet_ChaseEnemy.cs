using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_ChaseEnemy : Action
{
    public SharedPetManager self;

    public float canAttackDistance;

    public override void OnStart()
    {
        base.OnStart();

        self.Value.animator.CrossFade("Walk", 0.1f);
        self.Value.agent.enabled = true;
    }

    public override TaskStatus OnUpdate()
    {
        //判断是否到达了可攻击范围
        float currentDis = Vector3.Distance(self.Value.attackTarget.position,self.Value.transform.position);
        if (currentDis < canAttackDistance)
        {
            return TaskStatus.Success;
        }

        self.Value.agent.SetDestination(self.Value.attackTarget.position);

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();

        self.Value.agent.enabled = false;
    }
}

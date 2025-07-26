using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_KnockUp : Action
{
    public SharedEnemyManager self;
    private Rigidbody rb;

    public override void OnStart()
    {
        self.Value.animator.CrossFade("KnockBack", 0.1f);
        rb = self.Value.GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        if (rb.linearVelocity.y <= 0 && self.Value.isGrounded)
        {
            self.Value.state = EnemyState.Idle;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.LogWarning(99999);
        self.Value.agent.Warp(self.Value.transform.position);
    }
}

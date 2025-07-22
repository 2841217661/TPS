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
        //ApplyExplosionForce(transform.position, 500f, 10f);
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

    //public void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5f)
    //{
    //    Rigidbody rb = GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
    //    }
    //}

}

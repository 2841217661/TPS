using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Zombie_Patrol : Action
{
    public SharedEnemyManager self;
    private Rigidbody rb;

    public override void OnStart()
    {
        base.OnStart();

        rb = self.Value.GetComponent<Rigidbody>();

        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Patrol;

        self.Value.agent.updatePosition = false;
        self.Value.agent.updateRotation = false;

        rb.useGravity = false;

        // 设置新巡逻点并设定路径
        self.Value.currentPatrolPoint = GameManager.Instance.RandomSetPatrolPoint(self.Value.currentPatrolPoint);

        if (self.Value.currentPatrolPoint != null)
        {
            self.Value.agent.SetDestination(self.Value.currentPatrolPoint.position);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (self.Value.currentPatrolPoint == null)
        {
            return TaskStatus.Failure;
        }

        Vector3 desiredVelocity = self.Value.agent.desiredVelocity;
        Vector3 moveDir = desiredVelocity.normalized;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Vector3 newPosition = rb.position + moveDir * self.Value.moveSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);

            Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z);
            if (flatDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDir);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime));
            }

            rb.useGravity = moveDir.y <= 0f;
        }

        self.Value.agent.nextPosition = self.Value.transform.position;

        if (Vector3.Distance(self.Value.transform.position, self.Value.currentPatrolPoint.position) <= self.Value.reachDistance)
        {
            self.Value.animator.CrossFade("Idle", 0.2f);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        rb.useGravity = true;
    }
}

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_Chase : Action
{
    public SharedEnemyManager self;
    private float findPathInterval = 1f; // 路径寻找间隔
    private float findPathIntervalTimer;

    public override void OnStart()
    {
        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Chase;
        findPathIntervalTimer = findPathInterval; // 立即触发第一次寻路
    }

    public override TaskStatus OnUpdate()
    {
        if (self.Value.target == null)
        {
            return TaskStatus.Failure;
        }

        // 路径更新（间隔触发）
        findPathIntervalTimer += Time.deltaTime;
        if (findPathIntervalTimer >= findPathInterval)
        {
            self.Value.agent.SetDestination(self.Value.target.position);
            findPathIntervalTimer = 0f;
        }

        // 移动方向
        Vector3 desiredVelocity = self.Value.agent.desiredVelocity;
        Vector3 moveDir = desiredVelocity.normalized;

        // 平滑移动
        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.position += moveDir * self.Value.moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime);
        }

        // 更新 NavMeshAgent 的 nextPosition
        self.Value.agent.nextPosition = transform.position;

        // 到达目标
        if (Vector3.Distance(self.Value.transform.position, self.Value.target.position) <= self.Value.reachDistance)
        {
            self.Value.animator.CrossFade("Idle", 0.2f);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}

using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Zombie_Patrol : Action
{
    public SharedEnemyManager self;

    public override void OnStart()
    {
        base.OnStart();

        // 随机设置新巡逻点
        self.Value.currentPatrolPoint = GameManager.Instance.RandomSetPatrolPoint(self.Value.currentPatrolPoint);

        // 播放行走动画
        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Patrol;

        // 设置一次路径
        if (self.Value.currentPatrolPoint != null)
        {
            self.Value.agent.SetDestination(self.Value.currentPatrolPoint.position);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (self.Value.currentPatrolPoint != null)
        {
            Vector3 desiredVelocity = self.Value.agent.desiredVelocity;
            Vector3 moveDir = desiredVelocity.normalized;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                // 移动
                transform.position += moveDir * self.Value.moveSpeed * Time.deltaTime;

                // 旋转
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime);
            }

            self.Value.agent.nextPosition = transform.position;

            if (Vector3.Distance(self.Value.transform.position, self.Value.currentPatrolPoint.position) <= self.Value.reachDistance)
            {
                // 到达目标巡逻点身边
                self.Value.animator.CrossFade("Idle", 0.2f);
                return TaskStatus.Success;
            }

            return TaskStatus.Running; // 没有到达，继续前进
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

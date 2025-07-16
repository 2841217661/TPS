using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_Chase : Action
{
    public SharedEnemyManager self;
    public override void OnStart()
    {
        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Chase;
    }

    public override TaskStatus OnUpdate()
    {
        if (self.Value.target != null)
        {
            self.Value.agent.SetDestination(self.Value.target.position); // 持续更新路径

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

            if (Vector3.Distance(self.Value.transform.position, self.Value.target.position) <= self.Value.reachDistance)
            {
                //到达玩家身边
                self.Value.animator.CrossFade("Idle", 0.2f);
                return TaskStatus.Success;
            }

            return TaskStatus.Running; //没有到达，继续前进
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

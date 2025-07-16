using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Zombie_Patrol : Action
{
    public SharedEnemyManager self;

    private Transform currentPatrolPoint;
    private Transform lastPatrolPoint;

    private NavMeshPath navPath;
    private int pathIndex;

    public override void OnStart()
    {
        base.OnStart();

        // 随机设置新巡逻点
        currentPatrolPoint = RandomSetPatrolPoint(lastPatrolPoint);
        lastPatrolPoint = currentPatrolPoint;

        // 播放行走动画
        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Patrol;

        // 创建路径对象
        navPath = new NavMeshPath();
        self.Value.agent.CalculatePath(currentPatrolPoint.position, navPath);
        pathIndex = 0;
    }

    public override TaskStatus OnUpdate()
    {
        // 如果没有路径
        if (navPath == null || navPath.corners.Length == 0)
        {
            Debug.LogWarning("巡逻路径无效");
            return TaskStatus.Failure;
        }

        Vector3 currentPos = self.Value.transform.position;
        Vector3 targetPos = navPath.corners[pathIndex];

        // 移动方向
        Vector3 dir = (targetPos - currentPos);
        dir.y = 0;
        float distance = dir.magnitude;

        if (distance < 0.2f)
        {
            //到达当前路径点，切换到下一个
            pathIndex++;
            if (pathIndex >= navPath.corners.Length)
            {
                //全部到达
                self.Value.animator.CrossFade("Idle", 0.2f);
                self.Value.state = EnemyState.Idle;
                return TaskStatus.Success;
            }
        }
        else
        {
            // 手动移动
            dir.Normalize();
            self.Value.transform.position += dir * self.Value.moveSpeed * Time.deltaTime;
            self.Value.agent.nextPosition = transform.position;
            // 手动旋转
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                self.Value.transform.rotation = Quaternion.RotateTowards(
                    self.Value.transform.rotation,
                    targetRot,
                    self.Value.rotateSpeed * Time.deltaTime
                );
            }
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        currentPatrolPoint = null;
        navPath = null;
    }

    private Transform RandomSetPatrolPoint(Transform exclude)
    {
        Transform[] points = self.Value.patrolPoint;

        if (points.Length <= 1)
            return points[0];

        Transform next;
        do
        {
            next = points[Random.Range(0, points.Length)];
        }
        while (next == exclude);

        return next;
    }
}

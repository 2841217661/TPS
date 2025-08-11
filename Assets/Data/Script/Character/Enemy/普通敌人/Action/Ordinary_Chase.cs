using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Ordinary_Chase : Action
{
    public SharedEnemyManager self;
    private float findPathInterval = 0.5f; // 路径寻找间隔
    private float findPathIntervalTimer;
    private Rigidbody rb;
    public override void OnStart()
    {
        self.Value.agent.enabled = true;

        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Chase;
        findPathIntervalTimer = findPathInterval;

        rb = self.Value.GetComponent<Rigidbody>(); // 获取刚体

        self.Value.agent.updatePosition = false;
        self.Value.agent.updateRotation = false;

        self.Value.agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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

        // 动态加速
        float distanceToTarget = Vector3.Distance(self.Value.transform.position, self.Value.target.position);
        float accelerateDistance = 10f;
        float t = 1f - Mathf.Clamp01(distanceToTarget / accelerateDistance);
        t = t * t;
        float currentSpeed = Mathf.Lerp(self.Value.moveSpeed, self.Value.moveSpeed * 2f, t);

        // 移动
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Vector3 newPosition = rb.position + moveDir * currentSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);
        }

        // 判断是否无遮挡
        bool hasLOS = !NavMesh.Raycast(
            self.Value.transform.position,
            self.Value.target.position,
            out _,
            NavMesh.AllAreas
        );

        // 决定旋转方向
        Vector3 rotationDir;
        if (hasLOS)
        {
            rotationDir = (self.Value.target.position - self.Value.transform.position).normalized;
        }
        else
        {
            rotationDir = moveDir; // 用路径方向
        }

        rotationDir.y = 0;
        if (rotationDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime));
        }

        // 同步 NavMeshAgent
        self.Value.agent.nextPosition = transform.position;

        // 到达目标
        if (distanceToTarget <= self.Value.reachDistance)
        {
            self.Value.animator.CrossFade("Idle", 0.2f);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();

        self.Value.agent.enabled = false;
    }
}
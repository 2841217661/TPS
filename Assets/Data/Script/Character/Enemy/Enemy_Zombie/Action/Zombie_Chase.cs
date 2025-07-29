using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_Chase : Action
{
    public SharedEnemyManager self;
    private float findPathInterval = 0.5f; // 路径寻找间隔
    private float findPathIntervalTimer;
    private Rigidbody rb;
    public override void OnStart()
    {
        self.Value.animator.CrossFade("Walk", 0.2f);
        self.Value.state = EnemyState.Chase;
        findPathIntervalTimer = findPathInterval;

        rb = self.Value.GetComponent<Rigidbody>(); // 获取刚体

        self.Value.agent.updatePosition = false;
        self.Value.agent.updateRotation = false;

        rb.useGravity = false;
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

        // 计算距离并动态调整速度（非线性加速：越近越快，远一点就慢很多）
        float distanceToTarget = Vector3.Distance(self.Value.transform.position, self.Value.target.position);
        float accelerateDistance = 10f; // 小于10米开始加速（越小越敏感）

        float t = 1f - Mathf.Clamp01(distanceToTarget / accelerateDistance);
        t = t * t; // 非线性加速曲线，变化更陡峭

        float currentSpeed = Mathf.Lerp(self.Value.moveSpeed, self.Value.moveSpeed * 2f, t);


        // 使用 Rigidbody 进行移动
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Vector3 newPosition = rb.position + moveDir * currentSpeed * Time.deltaTime;
            rb.MovePosition(newPosition); // 替代 transform.position += ...

            Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z);
            if (flatDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDir);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime));
            }

            // 根据Y方向决定是否使用重力
            rb.useGravity = moveDir.y <= 0f;
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


    public override void OnEnd()
    {
        rb.useGravity = true;
    }
}
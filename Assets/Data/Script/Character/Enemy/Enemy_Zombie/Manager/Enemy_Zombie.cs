using UnityEngine;
using UnityEngine.AI;

public class Enemy_Zombie : EnemyManager,IDamageable
{
    [Header("设置相关")]
    public GameObject boomEffect; //死亡爆炸VFX

    public float backResistance; //击退抗性
    private float currentBackResistance;

    //检测范围分为两个检测
    //1：前方一定视角
    //2：周围近距离
    [Header("玩家检测范围")]
    public float forwardCheckDistance;
    public float forwardCheckAngle; //前方检测可见角度(左右都有，例如30°表示前方左侧30° - 右侧30°)
    public float roundCheckRaious; //周围检测半径
    public Vector3 checkOffset; //只能改变y的值
    private float checkInterval = 1f;  // 每隔1秒检测一次
    private float checkTimer = 0f;

    public void TakeDamage(float _value, CharacterManager _source, TakeDamageType _type)
    {
        Debug.Log($"{gameObject.name}受到来自{_source}的{_value}点伤害，伤害类型:{_type}");
        switch (_type)
        {
            case TakeDamageType.Heavy:
                state = EnemyState.Damage;
                currentBackResistance = 100f;
                break;
            case TakeDamageType.Light:
                currentBackResistance -= _value;
                if (currentBackResistance <= 0f)
                {
                    currentBackResistance = backResistance;
                    state = EnemyState.Damage;
                }
                break;
        }

        currentHealthValue -= _value;
    }

    protected override void Awake()
    {
        base.Awake();

        currentBackResistance = backResistance;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;   // 不自动更新 Transform 位置
        agent.updateRotation = false;   // 不自动更新 Transform 旋转

        patrolPointParent.SetParent(null); //分离巡逻点父物体
    }

    public override void OnDeath()
    {
        base.OnDeath();

        GameObject _boomEffect = Instantiate(boomEffect, transform.position + Vector3.up, transform.rotation);
        Destroy(_boomEffect, 2f);
    }



    protected override void Update()
    {
        base.Update();

        CheckPlayerIsAround(target);
    }

    /// <summary>
    /// 尝试发现玩家
    /// </summary>
    protected virtual void CheckPlayerIsAround(Transform _currentTarget)
    {
        if (_currentTarget != null)
        {
            checkTimer = 0f;
            return;
        }

        checkTimer += Time.deltaTime;
        if (checkTimer < checkInterval)
        {
            return;
        }
        checkTimer = 0f;

        Transform player = GameManager.Instance.playerManager.transform;
        Vector3 origin = transform.position + checkOffset;
        Vector3 playerPos = player.position + checkOffset;
        Vector3 dirToPlayer = playerPos - origin;
        float distanceToPlayer = dirToPlayer.magnitude;

        // 距离超过最大范围，直接返回
        if (distanceToPlayer > forwardCheckDistance)
        {
            return;
        }

        // 检测玩家在近距离
        Collider[] targets = Physics.OverlapSphere(origin, roundCheckRaious, targetLayer);
        if (targets.Length > 0)
        {
            target = targets[0].transform;
        }

        // 前方扇形视野检测:直接由检测点向玩家发射射线，如果第一个命中了玩家，说明看见了玩家
        if (Physics.Raycast(origin, GameManager.Instance.playerManager.transform.position - transform.position, out RaycastHit hit, forwardCheckDistance))
        {
            if (hit.transform == player && Vector3.Angle(transform.forward,hit.transform.position - transform.position) <= forwardCheckAngle)
            {
                target = player;
                return;
            }
        }
    }

    /// <summary>
    /// 绘制检测范围
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        // 绘制颜色
        Color forwardColor = new Color(0, 1, 0, 0.3f); //绿色
        Color aroundColor = new Color(1, 0, 0, 0.2f);  //红色

        Vector3 origin = transform.position + checkOffset;

        // 1. 绘制前方检测扇形
        Vector3 forward = transform.forward;
        float halfAngle = forwardCheckAngle;

        int segments = 10; //扇形分段数
        float angleStep = (halfAngle * 2) / segments;

        Vector3 prevPoint = origin + Quaternion.Euler(0, -halfAngle, 0) * forward * forwardCheckDistance;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -halfAngle + angleStep * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, angle, 0) * forward * forwardCheckDistance;

            //画线
            Gizmos.color = forwardColor;
            Gizmos.DrawLine(origin, nextPoint);
            Gizmos.DrawLine(prevPoint, nextPoint);

            prevPoint = nextPoint;
        }

        // 2. 绘制周围近距离检测范围
        Gizmos.color = aroundColor;
        Gizmos.DrawWireSphere(origin, roundCheckRaious);
    }
}
